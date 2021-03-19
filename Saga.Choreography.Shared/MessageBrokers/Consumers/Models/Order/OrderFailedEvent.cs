using Saga.Choreography.Core.MessageBrokers.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Saga.Choreography.Shared.MessageBrokers.Consumers.Models.Order
{
    public class OrderFailedEvent : EventModel
    {
        public int OrderId { get; set; }
    }
}
