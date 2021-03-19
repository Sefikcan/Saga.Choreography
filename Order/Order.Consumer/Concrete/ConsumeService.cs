using Microsoft.Extensions.Hosting;
using Order.Consumer.Consumers;
using Saga.Choreography.Core.MessageBrokers.Abstract;
using Saga.Choreography.Shared.MessageBrokers.Consumers.Models.Order;
using System.Threading;
using System.Threading.Tasks;

namespace Order.Consumer.Concrete
{
    public class ConsumeService : IHostedService
    {
        private readonly IEventBus _eventBus;

        public ConsumeService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _eventBus.Subscribe<OrderCompletedEvent, OrderCompletedConsumer>();
            _eventBus.Subscribe<OrderFailedEvent, OrderFailedConsumer>();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _eventBus.UnSubscribe<OrderCompletedEvent, OrderCompletedConsumer>();
            _eventBus.UnSubscribe<OrderFailedEvent, OrderFailedConsumer>();

            return Task.CompletedTask;
        }
    }
}
