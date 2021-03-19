using Microsoft.Extensions.Hosting;
using Saga.Choreography.Core.MessageBrokers.Abstract;
using Saga.Choreography.Shared.MessageBrokers.Consumers.Models.Stock;
using Stock.Consumer.Consumers;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Consumer.Concrete
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
            _eventBus.Subscribe<UpdateStockEvent, UpdateStockConsumer>();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _eventBus.UnSubscribe<UpdateStockEvent, UpdateStockConsumer>();

            return Task.CompletedTask;
        }
    }

}
