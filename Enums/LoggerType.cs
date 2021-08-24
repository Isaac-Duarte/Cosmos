using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Cosmos.Enums
{
    /// <summary>
    /// Enum to determain which loggers to use.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LoggerType
    {
        File,
        Console
    }
}