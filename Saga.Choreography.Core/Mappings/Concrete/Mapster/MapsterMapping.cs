using Mapster;
using Saga.Choreography.Core.Mappings.Abstract;

namespace Saga.Choreography.Core.Mappings.Concrete.Mapster
{
    /// <summary>
    /// 
    /// </summary>
    public class MapsterMapping : IMapping
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return source.Adapt<TDestination>();
        }
    }
}
