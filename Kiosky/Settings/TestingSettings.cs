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
        /// <summary>
        /// Get settings that will lock down the browser to a single site
        /// </summary>
        /// <param name="AllowSubframe"> Whether or not to allow embedded frame domains that don't match the single allowed site</param>
        /// <returns>Settings to allow accessing github.com</returns>
        public static Settings GetDefaultLockdownSettings(bool AllowSubframe = false)
        {
            Settings s = new Settings();
            s.StartURL = "github.com";
            s.Title = "Default";
            s.PopupWhenDomainBlocked = true;
            s.AllowedDomains = new string[] { "github.com" };
            s.AllowAllSubframeDomains = AllowSubframe;
            return s;
        }
    }
}
