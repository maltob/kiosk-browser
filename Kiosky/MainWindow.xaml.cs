using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kiosky
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Settings.Settings _browserSettings;
        Lockdown.WindowsLockdown _lockdown;
        List<Dialogs.BlankWindow> _blankWindows = new List<Dialogs.BlankWindow>();
        /// <summary>
        /// The primary application window, called by the App
        /// </summary>
        public MainWindow()
        {



            InitializeComponent();

            //Confgure the browser
            _browserSettings = Settings.KioskSettings.GetSettings();
            Browser.RequestHandler = new Handlers.KioskyRequestHandler(_browserSettings);
            ConfigureBrowserSettings(_browserSettings);
            _lockdown = new Lockdown.WindowsLockdown(DisableTaskManager: _browserSettings.BlockTaskManager, DisableAltTab: _browserSettings.BlockWindowSwitching);

            
            

            //Show the browser
            this.Visibility = Visibility.Visible;
           
            //Cover over non primary screens
            foreach(Screen s in Screen.AllScreens)
            {
                if(!s.Primary)
                {
                    var bw = new Dialogs.BlankWindow(s);
                    bw.Show();
                    _blankWindows.Add(bw);
                }
            }
            
            
        }
        private void Browser_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            e.Handled = false;


        }

        private void Browser_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // If they hit pause, show the Configuration dialog
            if (e.Key == Key.Pause)
            {
                Dialogs.Configuration configurationDialog = new Dialogs.Configuration();
                configurationDialog.ShowDialog();
            }
            else if (e.Key == Key.Escape)
            {
                
                    this.Close();
            }
        }

      
        /// <summary>
        /// Sets the embedded browser up for use
        /// </summary>
        /// <param name="browserSettings">Settings to use to configure the browser</param>
        private void ConfigureBrowserSettings(Settings.Settings browserSettings)
        {
            this.Browser.BrowserSettings.Databases = CefSharp.CefState.Disabled;
            this.Browser.BrowserSettings.JavascriptAccessClipboard = CefSharp.CefState.Disabled;
            this.Browser.BrowserSettings.JavascriptDomPaste = CefSharp.CefState.Disabled;
            this.Browser.BrowserSettings.Plugins = CefSharp.CefState.Disabled;
            this.Browser.BrowserSettings.WebSecurity = CefSharp.CefState.Enabled;

            if(browserSettings.StartURL != null)
            {
                this.Browser.Address = browserSettings.StartURL;
            }
        }

        private void Browser_Loaded(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Visible;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var cw = new Dialogs.CloseDialog();
            cw.ShowDialog();
            if (cw.ConfirmClose)
            {
                foreach (var bw in _blankWindows)
                {
                    bw.Close();
                }
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}