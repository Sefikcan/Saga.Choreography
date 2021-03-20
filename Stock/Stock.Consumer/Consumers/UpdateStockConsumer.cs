using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Microsoft.EntityFrameworkCore;
using Saga.Choreography.Core.Enums;
using Saga.Choreography.Shared.MessageBrokers.Consumers.Models.Order;
using Saga.Choreography.Shared.MessageBrokers.Consumers.Models.Shipment;
using Saga.Choreography.Shared.MessageBrokers.Consumers.Models.Stock;
using Stock.Infrastructure.DataAccess.EntityFramework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Consumer.Consumers
{
    public class UpdateStockConsumer : IConsumeAsync<UpdateStockEvent>
    {
        private readonly StockDbContext _dbContext;
        private readonly IBus _bus;

        public UpdateStockConsumer(StockDbContext dbContext, IBus bus)
        {
            _dbContext = dbContext;
            _bus = bus;
        }

        public async Task ConsumeAsync(UpdateStockEvent context, CancellationToken cancellationToken)
        {
            var stock = await _dbContext.Stocks.FirstOrDefaultAsync(x => x.ProductId == context.ProductId);
            if (stock == null)
            {
                await _bus.PubSub.PublishAsync(new OrderFailedEvent
                {
                    OrderId = context.OrderId
                });

                throw new Exception("Product not found!");
            }

            stock.Quantity -= context.Quantity;
            if (stock.Quantity < 0)
            {
                await _bus.PubSub.PublishAsync(new OrderFailedEvent
                {
                    OrderId = context.OrderId
                });

                throw new Exception("Quantity must be greater than or equal to zero!");
            }

            _dbContext.Stocks.Attach(stock);
            _dbContext.Entry(stock).Property(x => x.Quantity).IsModified = true;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                await _bus.PubSub.PublishAsync(new CreateShipmentEvent
                {
                    ShipmentType = (int)ShipmentType.MNG,
                    OrderId = context.OrderId
                });
            }
        }
    }
}
