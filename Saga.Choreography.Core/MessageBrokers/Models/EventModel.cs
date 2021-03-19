using System;

namespace Saga.Choreography.Core.MessageBrokers.Models
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class EventModel
    {
        /// <summary>
        /// ctor
        /// </summary>
        public EventModel()
        {
            CreatedDate = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid CorrelationId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}
