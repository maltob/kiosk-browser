using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kiosky.Settings
{
    /// <summary>
    /// Settings for the Kiosk Browser actions
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Application title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The URL to launch the browser to first.
        /// If using CycleURLs leave this empty
        /// </summary>
        public string StartURL { get; set; }
        /// <summary>
        /// An array of URLs to have the browser cycle through
        /// </summary>
        public string[] CycleURLs { get; set; }

        /// <summary>
        /// How long to spend on each URL in the cycling page
        /// </summary>
        public int CycleTime { get; set; }
        /// <summary>
        /// An array of domain names such as "github.com" and "www.github.com" to allow the browser to navigate to
        /// </summary>
        public string[] AllowedDomains { get; set; }
        /// <summary>
        /// Close these programs if they are running
        /// </summary>
        public string[] BlockPrograms { get; set; }
            /// <summary>
            /// Close ALL programs except these programs running in the current users session
            /// </summary>
        public string[] AllowedPrograms { get; set; }

        /// <summary> Whether we should filter the domains on subframes such as advertisments. </summary>
        [DefaultValue(true)]
        public bool AllowAllSubframeDomains { get; set; } = true;

        /// <summary> Whether to display a popup when a page is blocked, or just islently fail. </summary>
        [DefaultValue(true)]
        public bool PopupWhenDomainBlocked { get; set; } = true;

        /// <summary> Whether we should use GPO to block taskmanager </summary>
        [DefaultValue(false)]
        public bool BlockTaskManager { get; set; } = false;
    }
}
