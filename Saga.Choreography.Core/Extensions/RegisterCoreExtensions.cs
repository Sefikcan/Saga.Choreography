using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Saga.Choreography.Core.Mappings.Abstract;
using Saga.Choreography.Core.Mappings.Concrete.Mapster;
using Saga.Choreography.Core.MessageBrokers.Abstract;
using Saga.Choreography.Core.MessageBrokers.Concrete.RabbitMQ.MassTransit;
using Saga.Choreography.Core.Settings.Concrete;

namespace Saga.Choreography.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class RegisterCoreExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMapping, MapsterMapping>();

            var massTransitSettings = new MassTransitSettings();
            configuration.GetSection(nameof(MassTransitSettings)).Bind(massTransitSettings);
            services.Configure<MassTransitSettings>(configuration.GetSection(nameof(MassTransitSettings)));
            services.AddSingleton(massTransitSettings);

            services.AddSingleton<IEventBus, MassTransitEventBus>();

            return services;
        }
    }
}
