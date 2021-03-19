using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Order.Services.Extensions
{
    public static class RegisterOrderService
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
