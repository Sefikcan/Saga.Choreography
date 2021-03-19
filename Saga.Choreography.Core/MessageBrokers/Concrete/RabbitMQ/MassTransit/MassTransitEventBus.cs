using MassTransit;
using Saga.Choreography.Core.MessageBrokers.Abstract;
using Saga.Choreography.Core.MessageBrokers.Models;
using Saga.Choreography.Core.Settings.Concrete;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Saga.Choreography.Core.MessageBrokers.Concrete.RabbitMQ.MassTransit
{
    /// <summary>
    /// 
    /// </summary>
    public class MassTransitEventBus : IEventBus
    {
        private IBusControl _bus;
        private readonly MassTransitSettings _massTransitSettings;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="massTransitSettings"></param>
        /// <param name="serviceProvider"></param>
        public MassTransitEventBus(MassTransitSettings massTransitSettings, IServiceProvider serviceProvider)
        {
            _massTransitSettings = massTransitSettings;
            _bus = BusConfigurator.Instance.ConfigureBus(_massTransitSettings);
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task Publish<T>(T model) where T : EventModel
        {
            return _bus.Publish(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="THandler"></typeparam>
        /// <returns></returns>
        public async Task Subscribe<T, THandler>()
            where T : EventModel
            where THandler : IEventHandler<T>
        {
            if (_bus == null)
                throw new Exception("MassTransit could not be initialized!");

            var queueName = typeof(T).Name;
            _bus = BusConfigurator.Instance.ConfigureBus(_massTransitSettings, (cfg) =>
            {
                cfg.ReceiveEndpoint(queueName, e =>
                {
                    e.Consumer(() =>
                    {
                        var handler = _serviceProvider.GetService<IEventHandler<T>>();
                        return new MassTransitAdapter<T>(handler);
                    });
                });
            });

            await _bus.StartAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="THandler"></typeparam>
        /// <returns></returns>
        public async Task UnSubscribe<T, THandler>()
            where T : EventModel
            where THandler : IEventHandler<T>
        {
            if (_bus == null)
                throw new Exception("MassTransit could not be initialized!");

            await _bus.StopAsync();
            _bus = null;
        }
    }
}
