using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Order.Consumer.Concrete;
using Order.Consumer.Consumers;
using Order.Infrastructure.Extensions;
using Saga.Choreography.Core.Extensions;
using Saga.Choreography.Core.MessageBrokers.Abstract;
using Saga.Choreography.Shared.MessageBrokers.Consumers.Models.Order;
using System;

namespace Order.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            Console.ReadKey();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.json", true, true);
                config.AddEnvironmentVariables();

                if (args != null)
                    config.AddCommandLine(args);
            })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddCore(hostContext.Configuration)
                                       .AddOrder(hostContext.Configuration);

                    services.AddTransient<IEventHandler<OrderCompletedEvent>, OrderCompletedConsumer>();
                    services.AddTransient<IEventHandler<OrderFailedEvent>, OrderFailedConsumer>();

                    services.AddHostedService<ConsumeService>();
                });
        }
    }
}
