using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Saga.Choreography.Core.Extensions;
using Saga.Choreography.Core.MessageBrokers.Concrete.RabbitMQ.EasyNetQ;
using Saga.Choreography.Core.Settings.Concrete.MessageBrokers;
using Stock.Consumer.Concrete;
using Stock.Consumer.Consumers;
using Stock.Infrastructure.Extensions;
using System;
using System.Reflection;

namespace Stock.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            Console.ReadKey();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddCore(hostContext.Configuration)
                                       .AddStock(hostContext.Configuration);

                    EasyNetQSettings easyNetQSettings = new EasyNetQSettings();
                    hostContext.Configuration.GetSection(nameof(EasyNetQSettings)).Bind(easyNetQSettings);
                    services.AddSingleton(easyNetQSettings);

                    var bus = RabbitHutch.CreateBus(easyNetQSettings.Uri);
                    services.AddSingleton(bus);

                    services.AddSingleton<MessageDispatcher>();
                    services.AddSingleton(_ =>
                    {
                        return new AutoSubscriber(_.GetRequiredService<IBus>(), Assembly.GetExecutingAssembly().GetName().Name)
                        {
                            AutoSubscriberMessageDispatcher = _.GetRequiredService<MessageDispatcher>()
                        };
                    });

                    services.AddScoped<UpdateStockConsumer>();

                    services.AddHostedService<ConsumeService>();
                });
    }
}
