using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Saga.Choreography.Core.Extensions;
using Saga.Choreography.Core.MessageBrokers.Abstract;
using Saga.Choreography.Shared.MessageBrokers.Consumers.Models.Stock;
using Stock.Consumer.Concrete;
using Stock.Consumer.Consumers;
using Stock.Infrastructure.Extensions;
using System;

namespace Stock.Consumer
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
                                       .AddStock(hostContext.Configuration);

                    services.AddTransient<IEventHandler<UpdateStockEvent>, UpdateStockConsumer>();

                    services.AddHostedService<ConsumeService>();
                });
        }
    }
}
