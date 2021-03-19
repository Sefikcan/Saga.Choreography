using Saga.Choreography.Core.Settings.Abstract;

namespace Saga.Choreography.Core.Settings.Concrete
{
    /// <summary>
    /// 
    /// </summary>
    public class StockDbSettings : ISettings
    {
        /// <summary>
        /// 
        /// </summary>
        public string ConnectionStrings { get; set; }
    }
}
