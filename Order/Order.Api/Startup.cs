using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Order.Infrastructure.Extensions;
using Order.Services.Extensions;
using Saga.Choreography.Core.Extensions;
using Saga.Choreography.Core.MessageBrokers.Abstract;
using Saga.Choreography.Core.MessageBrokers.Concrete.RabbitMQ.MassTransit;
using Saga.Choreography.Core.Settings.Concrete;

namespace Order.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCore(Configuration)
                    .AddSwagger(Configuration)
                    .AddOrder(Configuration)
                    .AddServices();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(Configuration);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
