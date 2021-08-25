using System;
using System.IO;
using System.Threading.Tasks;
using Cosmos.Enums;
using Cosmos.Models;
using GSXApi;
using GSXApi.ApiCalls;
using GSXApi.Models;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;

namespace Cosmos
{
    class Program
    {
        static Configuration _config;
        static Logger _logger;

        static async Task Main(string[] args)
        {
            try
            {
                _config = await getConfiguration();
            }
            catch (Exception e)
            {
                _logger.Fatal($"Unexcepted error. {e.Message}");
                return;
            }
            
            _logger = buildLogger();

            _logger.Information("Cosmos is starting....");
            
            // Check basic validation.
            if (_config.GSXConfiguration.AuthConfiguration.CertPassword == null)
            {
                _logger.Fatal("Unforunatly cannot validate certification bundles. Please configure them.\nNote check the COSMOS_CONFIG_PATH Enviorment variable to see path for the configuration.");
                return;
            }

            _logger.Information("Cosmos started. Authenticating with GSX");

            GSXApiClient client = new GSXApiClient(_config.GSXConfiguration);

            var response = await client.GetExampleAsync();

            _logger.Information($"{response.FirstName} secret is {response.Secret}");
        }

        /// <summary>
        /// Grabs the configuration file and creates one if there isn't one.
        /// </summary>
        private static async Task<Configuration> getConfiguration()
        {
            string filePath = "appsettings.json";

            // Check for custom file path.
            if (Environment.GetEnvironmentVariable("COSMOS_CONFIG_PATH") != null)
                filePath = Environment.GetEnvironmentVariable("COSMOS_CONFIG_PATH");

            // Configuration object
            Configuration config;

            // Create the configuration file if it doesn't exist.
            if (!File.Exists(filePath))
            {
                // Create default configuration
                config = new Configuration();
                config.Loggers = new LoggerType[2] { LoggerType.Console, LoggerType.File };
                config.GSXConfiguration = new GSXConfiguration();
                config.GSXConfiguration.AuthConfiguration = new AuthConfiguration();

                string payload = JsonConvert.SerializeObject(config, Formatting.Indented);

                await File.WriteAllTextAsync(filePath, payload);

                return config;
            }

            // Read existing configuration
            string content = await File.ReadAllTextAsync(filePath);
            config = JsonConvert.DeserializeObject<Configuration>(content);

            return config;
        }

        private static Logger buildLogger()
        {
            LoggerConfiguration loggerConfiguration = new LoggerConfiguration();

            // Loop through through the loggers and add them
            foreach (LoggerType logger in _config.Loggers)
            {
                switch (logger)
                {
                    case LoggerType.Console:
                        loggerConfiguration.WriteTo.Console();
                        break;
                    case LoggerType.File:
                        loggerConfiguration.WriteTo.RollingFile($"{_config.LoggingPath}/log-{{Date}}.txt");
                        break;
                }
            }

            return loggerConfiguration.CreateLogger();
        }
    }
}
