using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Cosmos.Enums;
using GSXApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Cosmos.Models
{
    public class Configuration
    {
        /// <summary>
        /// GSX Configuration from GSXApi
        /// </summary>
        [JsonProperty("GSXConfiguration")]
        public GSXConfiguration GSXConfiguration { get; set; }
        
        /// <summary>
        /// Loggers to be used by Cosmos for info, debug, and errors.
        /// </summary>
        [JsonProperty("Loggers")]
        public LoggerType[] Loggers { get; set; }
        
        /// <summary>
        /// Gets the logging path
        /// </summary>
        [JsonProperty("LoggingPath")]
        public string LoggingPath { get; set; }
    }
}