using Saga.Choreography.Core.Enums;
using Saga.Choreography.Core.MessageBrokers.Abstract;
using Saga.Choreography.Shared.MessageBrokers.Consumers.Models.Order;
using Saga.Choreography.Shared.MessageBrokers.Consumers.Models.Shipment;
using Shipment.Infrastructure.DataAccess.EntityFramework;
using System.Threading.Tasks;

namespace Shipment.Consumer.Consumers
{
    public class CreateShipmentConsumer : IEventHandler<CreateShipmentEvent>
    {
        private readonly ShipmentDbContext _dbContext;
        private readonly IEventBus _eventBus;

        public CreateShipmentConsumer(ShipmentDbContext dbContext,
            IEventBus eventBus)
        {
            _dbContext = dbContext;
            _eventBus = eventBus;
        }

        public async Task Consume(CreateShipmentEvent context)
        {
            if (context.ShipmentType == (int)ShipmentType.MNG || context.ShipmentType == (int)ShipmentType.Yurtici)
            {
                await _eventBus.Publish(new OrderCompletedEvent 
                {
                    OrderId = context.OrderId
                });
            }
            else
            {
                await _eventBus.Publish(new OrderFailedEvent
                {
                    OrderId = context.OrderId
                });
            }

            var shipment = new Infrastructure.Entities.Shipment
            {
                OrderId = context.OrderId,
                ShipmentType = context.ShipmentType
            };

            await _dbContext.AddAsync(shipment);
            await _dbContext.SaveChangesAsync();
        }
    }
}
