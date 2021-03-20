using Saga.Choreography.Core.Settings.Abstract;

namespace Saga.Choreography.Core.Settings.Concrete.MessageBrokers
{
    /// <summary>
    /// 
    /// </summary>
    public class EasyNetQSettings : ISettings
    {
        /// <summary>
        /// 
        /// </summary>
        public string Uri { get; set; }
    }
}
