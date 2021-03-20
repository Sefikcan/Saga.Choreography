using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Saga.Choreography.Core.Enums;
using Saga.Choreography.Shared.MessageBrokers.Consumers.Models.Order;
using Saga.Choreography.Shared.MessageBrokers.Consumers.Models.Shipment;
using Shipment.Infrastructure.DataAccess.EntityFramework;
using System.Threading;
using System.Threading.Tasks;

namespace Shipment.Consumer.Consumers
{
    public class CreateShipmentConsumer : IConsumeAsync<CreateShipmentEvent>
    {
        private readonly ShipmentDbContext _dbContext;
        private readonly IBus _bus;

        public CreateShipmentConsumer(ShipmentDbContext dbContext,
            IBus bus)
        {
            _dbContext = dbContext;
            _bus = bus;
        }

        public async Task ConsumeAsync(CreateShipmentEvent context, CancellationToken cancellationToken)
        {
            if (context.ShipmentType == (int)ShipmentType.MNG || context.ShipmentType == (int)ShipmentType.Yurtici)
            {
                await _bus.PubSub.PublishAsync(new OrderCompletedEvent 
                {
                    OrderId = context.OrderId
                });
            }
            else
            {
                await _bus.PubSub.PublishAsync(new OrderFailedEvent
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
