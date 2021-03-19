using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Saga.Choreography.Core.Extensions;
using Saga.Choreography.Core.MessageBrokers.Abstract;
using Saga.Choreography.Shared.MessageBrokers.Consumers.Models.Shipment;
using Shipment.Consumer.Concrete;
using Shipment.Consumer.Consumers;
using Shipment.Infrastructure.Extensions;
using System;

namespace Shipment.Consumer
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
                                        .AddShipment(hostContext.Configuration);

                    services.AddTransient<IEventHandler<CreateShipmentEvent>, CreateShipmentConsumer>();

                    services.AddHostedService<ConsumeService>();
                });
        }
    }
}
