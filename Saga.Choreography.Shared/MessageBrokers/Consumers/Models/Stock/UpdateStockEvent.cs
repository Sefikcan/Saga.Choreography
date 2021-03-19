using Saga.Choreography.Core.MessageBrokers.Models;

namespace Saga.Choreography.Shared.MessageBrokers.Consumers.Models.Stock
{
    public class UpdateStockEvent : EventModel
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public int OrderId { get; set; }
    }
}
