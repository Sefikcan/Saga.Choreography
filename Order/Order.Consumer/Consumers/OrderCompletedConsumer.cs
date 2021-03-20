using EasyNetQ.AutoSubscribe;
using Microsoft.EntityFrameworkCore;
using Order.Infrastructure.DataAccess.EntityFramework;
using Saga.Choreography.Core.Enums;
using Saga.Choreography.Shared.MessageBrokers.Consumers.Models.Order;
using System.Threading;
using System.Threading.Tasks;

namespace Order.Consumer.Consumers
{
    public class OrderCompletedConsumer : IConsumeAsync<OrderCompletedEvent>
    {
        private readonly OrderDbContext _dbContext;

        public OrderCompletedConsumer(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ConsumeAsync(OrderCompletedEvent context, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == context.OrderId);
            if (order == null)
                throw new System.Exception("Order not found!");

            order.OrderStatus = (int)OrderStatus.Success;
            await _dbContext.SaveChangesAsync();
        }
    }
}
