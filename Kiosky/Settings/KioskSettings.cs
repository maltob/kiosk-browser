using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace Kiosky.Settings
{
    /// <summary>
    /// Default handler to load in the settings for the Kiosk app
    /// </summary>
    public class KioskSettings
    {
        /// <summary>
        /// Load in the Kiosk app settings, taking into account any command line parameters
        /// </summary>
        /// <returns></returns>
        public static Settings GetSettings()
        {
            Settings settings = TestingSettings.GetDefaultLockdownSettings();

            var settingsFileName = GetConfigPath();

            //Load in the settings file from disk or URL
            if (System.IO.File.Exists(settingsFileName))
            {
                settings = YAMLSettings.FromFile(settingsFileName);
            }
            else if (settingsFileName.Contains("//") && (new Uri(settingsFileName)).Scheme.Contains("http"))
            {
                settings = YAMLSettings.FromURL(settingsFileName);

            }

            if(settings == null)
            {
                settings = TestingSettings.GetDefaultLockdownSettings();
            }

            // Allow overriding the start URL
            Parser.Default.ParseArguments<KioskOptions>(Environment.GetCommandLineArgs()).WithParsed<KioskOptions>(
            ko =>
            {
                if (ko.StartURL != null)
                {
                    settings.CycleURLs[0] = ko.StartURL;
                }
            }
            );

            return settings;
        }

        /// <summary>
        /// Read in command line arguments to find the configuration path
        /// </summary>
        /// <returns>Path to the configuration file</returns>
        public static string GetConfigPath()
        {
            string settingsFileName = "settings.yaml";
            Parser.Default.ParseArguments<KioskOptions>(Environment.GetCommandLineArgs()).WithParsed<KioskOptions>(
                ko =>
                {
                    if (ko.SettingsFile != null)
                    {
                        settingsFileName = ko.SettingsFile;
                    }
                }
                );

            return settingsFileName;
        }
    }
    class KioskOptions
    {
        [Option('s', "settings", Required = false, HelpText = "Settings file path can be a local file, or HTTP/HTTPS")]
        public string SettingsFile { get; set; }

        [Option('u',"url", Required = false, HelpText = "The URL to launch")]
        public string StartURL { get; set; }
    }
}
