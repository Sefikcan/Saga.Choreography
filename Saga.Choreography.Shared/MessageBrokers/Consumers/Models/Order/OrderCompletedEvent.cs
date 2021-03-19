using Saga.Choreography.Core.MessageBrokers.Models;

namespace Saga.Choreography.Shared.MessageBrokers.Consumers.Models.Order
{
    public class OrderCompletedEvent : EventModel
    {
        public int OrderId { get; set; }
    }
}
