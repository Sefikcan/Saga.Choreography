using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Saga.Choreography.Core.Settings.Concrete;
using Stock.Infrastructure.DataAccess.EntityFramework;

namespace Stock.Infrastructure.Extensions
{
    public static class RegisterStock
    {
        public static IServiceCollection AddStock(this IServiceCollection services, IConfiguration configuration)
        {
            StockDbSettings stockDbSettings = new StockDbSettings();
            configuration.GetSection(nameof(StockDbSettings)).Bind(stockDbSettings);
            services.AddSingleton(stockDbSettings);

            services.AddDbContext<StockDbContext>(c =>
                c.UseSqlServer(stockDbSettings.ConnectionStrings), ServiceLifetime.Transient);

            return services;
        }
    }
}
