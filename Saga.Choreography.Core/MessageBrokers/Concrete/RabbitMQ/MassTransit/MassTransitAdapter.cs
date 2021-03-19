using MassTransit;
using Saga.Choreography.Core.MessageBrokers.Abstract;
using Saga.Choreography.Core.MessageBrokers.Models;
using System.Threading.Tasks;

namespace Saga.Choreography.Core.MessageBrokers.Concrete.RabbitMQ.MassTransit
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MassTransitAdapter<T> : IConsumer<T> where T : EventModel
    {
        private readonly IEventHandler<T> _eventHandler;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="eventHandler"></param>
        public MassTransitAdapter(IEventHandler<T> eventHandler)
        {
            _eventHandler = eventHandler;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<T> context)
        {
            await _eventHandler.Consume(context.Message);
        }
    }
}
