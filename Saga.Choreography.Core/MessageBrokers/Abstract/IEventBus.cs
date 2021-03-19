using Saga.Choreography.Core.MessageBrokers.Models;
using System.Threading.Tasks;

namespace Saga.Choreography.Core.MessageBrokers.Abstract
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Publish<T>(T model) where T : EventModel;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="THandler"></typeparam>
        /// <returns></returns>
        Task Subscribe<T, THandler>()
           where T : EventModel
           where THandler : IEventHandler<T>;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="THandler"></typeparam>
        /// <returns></returns>
        Task UnSubscribe<T, THandler>()
            where T : EventModel
            where THandler : IEventHandler<T>;
    }
}
