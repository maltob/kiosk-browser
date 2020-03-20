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

            // Don't allow viewing or editing a web config
            if (Settings.KioskSettings.GetConfigPath().Contains("://"))
            {
                this.Close();
            }

            //Load in the settings and enable the password change if required
            this._settings = Settings.KioskSettings.GetSettings();
            if (this._settings.AdminPasswordHash == null || this._settings.AdminPasswordHash.Length == 0)
            {
                EnableSettings(true);
            }
            else
            {
                EnableSettings(false);
            }

            //handle settings that a binding doesn't work quite right for
            LoadSettings();

            this.DataContext = _settings;
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            this.Activate();
        }

        private void EnableSettings(bool enable)
        {
            _hasUnlocked = enable;

            this.SettingsTabControl.IsEnabled = enable;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.SettingsTabControl); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(this.SettingsTabControl, i);
                if (child != null && child is Control)
                {
                    ((Control)child).IsEnabled = enable;
                }
            }
                 
        }

      

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // we can't change a web endpoint config
            if (!Settings.KioskSettings.GetConfigPath().Contains("://"))
            {

                if (_hasUnlocked)
                {

                    // If the user has changed the password, ask if they want to change it
                    if (((this._settings.AdminPasswordHash == null || this._settings.AdminPasswordHash.Length == 0) && AdminPasswordBox.Password.Length > 0) || (Settings.PasswordManager.ComparePassword(AdminPasswordBox.Password, _settings) == false))
                    {
                        MessageBoxResult response = MessageBox.Show("You have changed the password, would you like to save the new password?", "Change Password?", MessageBoxButton.YesNo);
                        if (response == MessageBoxResult.Yes)
                        {
                            if (AdminPasswordBox.Password.Length == 0)
                            {
                                _settings.AdminPasswordHash = "";
                            }
                            else
                            {
                                //Hash the new password
                                _settings.AdminPasswordHash = Settings.PasswordManager.GeneratePasswordHash(AdminPasswordBox.Password, Settings.PasswordManager.GenerateSalt(16));

                            }
                        }
                    }
                    //handle settings that a simple binding don't work quite right for
                    UpdateSettings();
                    //Save the settings back to file
                    Settings.YAMLSettings.ToFile(Settings.KioskSettings.GetConfigPath(), _settings);
                }

            }
            this.Close();
        }

        private void LoadSettings()
        {
            //Domain list
            this.AllowedDomains.Text = String.Join("\n", _settings.AllowedDomains);

            //Program list
            if (_settings.AllowedPrograms.Length > 0)
            {
                this.AllowRadioButton.IsChecked = true;
                this.ProgramList.Text = String.Join("\n", _settings.AllowedPrograms);
            }
            else
            {
                this.BlockRadioButton.IsChecked = true;
                this.ProgramList.Text = String.Join("\n", _settings.BlockPrograms);
            }


            //URLs to open 
            this.URLsToOpen.Text = String.Join("\n", _settings.CycleURLs);

            //Cycle time
            this.SecondsBetweenPages.Text = _settings.CycleTime.ToString();

            //Seconds before hiding a cursor
            this.CursorAutoHide.Text = _settings.IdleCursorHideTime.ToString(); ;
          
        }

        private void UpdateSettings()
        {
            //Update the domain list
            _settings.AllowedDomains = this.AllowedDomains.Text.Split('\r', '\n', ',').Where(o => o.Length > 0).ToArray();

            //Update the allowed programs list
            if (this.AllowRadioButton.IsChecked == true)
            {
                _settings.BlockPrograms = new string[] { };
                _settings.AllowedPrograms = this.ProgramList.Text.Split('\r', '\n', ',').Where(o => o.Length > 0).ToArray();
            }
            else
            {
                _settings.AllowedPrograms = new string[] { };
                _settings.BlockPrograms = this.ProgramList.Text.Split('\r', '\n', ',').Where(o => o.Length > 0).ToArray();
            }

       
            //Startup URLs
            _settings.CycleURLs = this.URLsToOpen.Text.Split('\r', '\n').Where(o => o.Length > 0).ToArray();

            //Time between pages
            int cT = 30;
            Int32.TryParse(this.SecondsBetweenPages.Text, out cT);
            _settings.CycleTime = cT;

            //time to hide the cursor automatically
            int aH = 0;
            Int32.TryParse(this.CursorAutoHide.Text, out aH);
            _settings.IdleCursorHideTime = aH;

        }

        private void CheckPassword()
        {
            if (Settings.PasswordManager.ComparePassword(AdminPasswordBox.Password, _settings))
            {
                EnableSettings(true);
            }
            else
            {
                EnableSettings(false);

            }
        }

        private void AdminPasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab)
            {
                CheckPassword();
            }
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            CheckPassword();
        }
    }
}
