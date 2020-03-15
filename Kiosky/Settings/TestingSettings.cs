using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kiosky.Settings
{
    /// <summary>
    /// Class to generate a test set of settings
    /// </summary>
    public class TestingSettings
    {
        private const string defaultTitle = "Default";

        /// <summary>
        /// Get settings that will lock down the browser to a single site
        /// </summary>
        /// <param name="AllowSubframe"> Whether or not to allow embedded frame domains that don't match the single allowed site</param>
        /// <returns>Settings to allow accessing github.com</returns>
        public static Settings GetDefaultLockdownSettings(bool AllowSubframe = false)
        {
            Settings s = new Settings();
            s.StartURL = "github.com";
            s.Title = defaultTitle;
            s.PopupWhenDomainBlocked = true;
            s.AllowedDomains = new string[] { "github.com" };
            s.AllowAllSubframeDomains = AllowSubframe;
            return s;
        }

        /// <summary>
        /// Settings that are used for a cycling kiosk style browser such as for digital signage or a dashboard
        /// </summary>
        /// <returns>Settings configured to cycle through three sites every 5 seconds</returns>
        public static Settings GetDefaultCycleSettings()
        {
            Settings s = new Settings();
            s.Title = defaultTitle;
            s.PopupWhenDomainBlocked = false;
            s.CycleTime = 5;
            s.CycleURLs = new string[] { "github.com", "microsoft.com", "https://www.google.com/chrome/" };
            return s;
        }
    }
}
