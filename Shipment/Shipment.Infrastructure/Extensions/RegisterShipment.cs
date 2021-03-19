using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Saga.Choreography.Core.Settings.Concrete;
using Shipment.Infrastructure.DataAccess.EntityFramework;

namespace Shipment.Infrastructure.Extensions
{
    public static class RegisterShipment
    {
        public static IServiceCollection AddShipment(this IServiceCollection services, IConfiguration configuration)
        {
            ShipmentDbSettings shipmentDbSettings = new ShipmentDbSettings();
            configuration.GetSection(nameof(ShipmentDbSettings)).Bind(shipmentDbSettings);
            services.AddSingleton(shipmentDbSettings);

            services.AddDbContext<ShipmentDbContext>(c =>
                c.UseSqlServer(shipmentDbSettings.ConnectionStrings), ServiceLifetime.Transient);

            return services;
        }
    }
}
