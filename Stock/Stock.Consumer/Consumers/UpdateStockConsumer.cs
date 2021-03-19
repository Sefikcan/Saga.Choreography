using Microsoft.EntityFrameworkCore;
using Saga.Choreography.Core.Enums;
using Saga.Choreography.Core.MessageBrokers.Abstract;
using Saga.Choreography.Shared.MessageBrokers.Consumers.Models.Order;
using Saga.Choreography.Shared.MessageBrokers.Consumers.Models.Shipment;
using Saga.Choreography.Shared.MessageBrokers.Consumers.Models.Stock;
using Stock.Infrastructure.DataAccess.EntityFramework;
using System;
using System.Threading.Tasks;

namespace Stock.Consumer.Consumers
{
    public class UpdateStockConsumer : IEventHandler<UpdateStockEvent>
    {
        private readonly StockDbContext _dbContext;
        private readonly IEventBus _eventBus;

        public UpdateStockConsumer(StockDbContext dbContext, IEventBus eventBus)
        {
            _dbContext = dbContext;
            _eventBus = eventBus;
        }

        public async Task Consume(UpdateStockEvent context)
        {
            var stock = await _dbContext.Stocks.FirstOrDefaultAsync(x => x.ProductId == context.ProductId);
            if (stock == null)
            {
                await _eventBus.Publish(new OrderFailedEvent
                {
                    OrderId = context.OrderId
                });

                throw new Exception("Product not found!");
            }

            stock.Quantity -= context.Quantity;
            if (stock.Quantity < 0)
            {
                await _eventBus.Publish(new OrderFailedEvent
                {
                    OrderId = context.OrderId
                });

                throw new Exception("Quantity must be greater than or equal to zero!");
            }

            _dbContext.Stocks.Attach(stock);
            _dbContext.Entry(stock).Property(x => x.Quantity).IsModified = true;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                await _eventBus.Publish(new CreateShipmentEvent
                {
                    ShipmentType = (int)ShipmentType.MNG,
                    OrderId = context.OrderId
                });
            }
        }
    }
}
