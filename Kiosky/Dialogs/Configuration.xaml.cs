using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kiosky.Dialogs
{
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class Configuration : Window
    {
        Settings.Settings _settings;
        bool _hasUnlocked;
        /// <summary>
        /// Create a dialog window to allow for modifying the Kiosk Browsers configuration
        /// </summary>
        public Configuration()
        {
            InitializeComponent(); 
            this.Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            //Load in the settings and enable the password change if required
            this._settings = Settings.KioskSettings.GetSettings();
            if(this._settings.AdminPasswordHash == null || this._settings.AdminPasswordHash.Length == 0)
            {
                EnableSettings(true);
            }
            else
            {
                EnableSettings(false);
            }
            
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            this.Activate();
        }

        private void EnableSettings(bool enable)
        {
            _hasUnlocked = enable;
            this.ChangeButton.IsEnabled = enable;
            this.SettingsTabControl.IsEnabled = enable;
        }

        private void AdminPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if(Settings.PasswordManager.ComparePassword(AdminPasswordBox.Password,_settings))
            {
                EnableSettings(true);
            }
            else
            {
                EnableSettings(false);
            }
        }
    }
}
