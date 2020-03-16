using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace Kiosky.Settings
{
    public class KioskSettings
    {
        public static Settings GetSettings()
        {
            Settings settings = TestingSettings.GetDefaultLockdownSettings();
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

            //Load in the settings file from disk or URL
            if (System.IO.File.Exists(settingsFileName))
            {
                settings = YAMLSettings.FromFile(settingsFileName);
            }
            else if (settingsFileName.Contains("//") && (new Uri(settingsFileName)).Scheme.Contains("http"))
            {
                settings = YAMLSettings.FromURL(settingsFileName);

            }

            // Allow overriding the start URL
            Parser.Default.ParseArguments<KioskOptions>(Environment.GetCommandLineArgs()).WithParsed<KioskOptions>(
            ko =>
            {
                if (ko.StartURL != null)
                {
                    settings.StartURL = ko.StartURL;
                }
            }
            );

            return settings;
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
