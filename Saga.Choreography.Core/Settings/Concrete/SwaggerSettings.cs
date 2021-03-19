using Microsoft.OpenApi.Models;
using Saga.Choreography.Core.Settings.Abstract;

namespace Saga.Choreography.Core.Settings.Concrete
{
    /// <summary>
    /// 
    /// </summary>
    public class SwaggerSettings : OpenApiInfo, ISettings
    {
        /// <summary>
        /// 
        /// </summary>
        public string VersionName { get; set; } = "v1";

        /// <summary>
        /// 
        /// </summary>
        public string RoutePrefix { get; set; } = "";
    }
}
