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

        /// <summary> Whether to display a popup when a page is blocked, or just silently fail. </summary>
        [DefaultValue(true)]
        public bool PopupWhenDomainBlocked { get; set; } = true;

        /// <summary> Whether we should use GPO to block taskmanager </summary>
        [DefaultValue(false)]
        public bool BlockTaskManager { get; set; } = false;

        /// <summary>
        /// Try to block window switching Ex: Alt+Tab
        /// </summary>
        [DefaultValue(true)]
        public bool BlockWindowSwitching { get; set; } = true;

        /// <summary>
        /// The password hash for admin
        /// </summary>
        public string AdminPasswordHash { get; set; }

        /// <summary>
        /// Whether or not to require the Admin password to leave the browser
        /// </summary>
        public bool RequirePasswordToExit { get; set; }


        /// <summary>
        /// Should we prompt to confirm that we want to exit?
        /// </summary>
        public bool PromptOnExit { get; set; }


        /// <summary>
        /// Show the help button at the top of the screen
        /// </summary>
        public bool ShowHelpButton { get; set; }


        /// <summary>
        /// Show the URL in a bar at the top of the screen
        /// </summary>
        public bool ShowURLBar { get; set; }

        /// <summary>
        /// Show a close button at the top of the screen
        /// </summary>
        public bool ShowCloseButton { get; set; }

        /// <summary>
        /// What URL should the help button load?
        /// </summary>
        public string HelpPageURL { get; set; }

        /// <summary>
        /// Should there be a message on any secondary/tertiary screens?
        /// </summary>
        public string BlankPageComment { get; set; }

        /// <summary>
        /// How many minutes before hiding the cursor, 0 to disable hiding
        /// </summary>
        public int IdleCursorHideTime { get; set; }

    
    }
}
