using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.IO;

namespace CT.Utils
{
    /// <summary>
    /// class to handle configuration settings related code
    /// </summary>
    public static class ApplicationSettings
    {
        /// <summary>
        /// Fetch the global application settings
        /// .NET Framewrok - Keys in "appSettings" section
        /// Others - Keys in specified section in "appsettings.json" specified in settings
        /// </summary>
        /// <typeparam name="T">The type of value expected in the settings</typeparam>
        /// <param name="keyName">Name of the key for which to fetch the value</param>
        /// <param name="sectionName">Name of the section to fetch values from, works only for pulling in the configurations from appsettiongs.json</param>
        /// <param name="env">Name of the config file</param>
        /// <returns>Returns the type of value expected in the settings</returns>
        public static T GetAppSetting<T>(string keyName, string sectionName = null, string env = null)
        {
            object value = null;
            var errorString = string.Empty;

            // var filename construct
            var configName = GetEnvironmentFileName(env);

            if (File.Exists(configName))
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile(configName, optional: true, reloadOnChange: true)
                    .Build();

                if (configuration != null)
                {
                    if (!string.IsNullOrEmpty(sectionName))
                    {
                        var globalSection = configuration.GetSection(sectionName);
                        if (globalSection.Exists())
                        {
                            value = globalSection[keyName];
                        }
                        else
                        {
                            errorString = $"Couldn't find the section or key: '{sectionName}' > '{keyName}' in {configName}";
                        }
                    }
                    else
                    {
                        errorString += $"The sectionName in {configName} cannot be empty.";
                    }
                }
            }
            else
            {
                errorString += $"Couldn't find the file {configName}";
            }

            if (value == null)
            {
                if (string.IsNullOrEmpty(errorString))
                {
                    errorString = $"Couldn't find the section or key: '{sectionName}' > '{keyName}' in appsettings.json";
                }

                throw new SettingsPropertyNotFoundException($"{errorString}. Please check the configuration file.");
            }

            return (T)Extensions.TypeExtensions.ChangeType(value, typeof(T));
        }

        private static string GetEnvironmentFileName(string configName)
        {
            return string.IsNullOrEmpty(configName) ? "appsettings.json" : $"appsettings.{configName}.json";
        }
    }
}