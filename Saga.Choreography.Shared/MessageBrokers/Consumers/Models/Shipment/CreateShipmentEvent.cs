using Saga.Choreography.Core.MessageBrokers.Models;

namespace Saga.Choreography.Shared.MessageBrokers.Consumers.Models.Shipment
{
    public class CreateShipmentEvent : EventModel
    {
        public int OrderId { get; set; }

        public int ShipmentType { get; set; }
    }
}
