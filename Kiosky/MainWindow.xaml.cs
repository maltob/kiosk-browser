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
using System.Windows.Threading;

namespace Kiosky
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Settings.Settings _browserSettings;
        Lockdown.WindowsLockdown _lockdown;
        DispatcherTimer _cycleURLTimer;
        DispatcherTimer _hideMouseTimer;
        DateTime _lastMouseMove;
        int _urlIndex = 0;
        List<Dialogs.BlankWindow> _blankWindows = new List<Dialogs.BlankWindow>();
        /// <summary>
        /// The primary application window, called by the App
        /// </summary>
        public MainWindow()
        {

            _lastMouseMove = DateTime.Now;

            InitializeComponent();

            LoadSettings();


            //Show the browser
            this.Visibility = Visibility.Visible;

            //Cover over the other screens
            foreach (Screen s in Screen.AllScreens)
            {
                if(!(this.Left >= s.Bounds.Left && this.Top >= s.Bounds.Top && this.Top+this.Height <= s.Bounds.Top+s.Bounds.Height))
                {
                    var bw = new Dialogs.BlankWindow(s, _browserSettings.BlankPageComment);
                    bw.Show();
                    _blankWindows.Add(bw);
                }
                
            }

            //Hides the cursor when idle
            _hideMouseTimer = new DispatcherTimer();
            _hideMouseTimer.Interval = TimeSpan.FromSeconds(60);
            _hideMouseTimer.Tick += _hideMouseTimer_Tick;
            _hideMouseTimer.IsEnabled = true;
            _hideMouseTimer.Start();

        }

        private void _hideMouseTimer_Tick(object sender, EventArgs e)
        {
            // Hide the cursor when it is idle
            if (_browserSettings.IdleCursorHideTime > 0 && DateTime.Now.Subtract(_lastMouseMove).TotalMinutes > _browserSettings.IdleCursorHideTime)
                Mouse.OverrideCursor = System.Windows.Input.Cursors.None;
        }

        private void LoadSettings()
        {


            //Confgure the browser
            _browserSettings = Settings.KioskSettings.GetSettings();

            Browser.RequestHandler = new Handlers.KioskyRequestHandler(_browserSettings);

            //Setup the lockdown
            ConfigureBrowserSettings(_browserSettings);
            if (_lockdown != null)
            {
                _lockdown = null;
            }


            _lockdown = new Lockdown.WindowsLockdown(DisableTaskManager: _browserSettings.BlockTaskManager, DisableAltTab: _browserSettings.BlockWindowSwitching);


            //If there was an old timer, tear it down
            if (_cycleURLTimer != null)
            {
                _cycleURLTimer.Stop();
                _cycleURLTimer.IsEnabled = false;
                _cycleURLTimer = null;


            }

            //Setup the timer if the cycle time isn't 0 and we have more than one URL
            if (_browserSettings.CycleTime > 0 && _browserSettings.CycleURLs.Length > 1)
            {
                _cycleURLTimer = new DispatcherTimer();
                _cycleURLTimer.Interval = TimeSpan.FromSeconds(_browserSettings.CycleTime);
                _cycleURLTimer.Tick += CycleURL_Tick;
                _cycleURLTimer.IsEnabled = true;
                _cycleURLTimer.Start();
            }

            //Setup the top bar, if we show no buttons or url bar hide it
            if(_browserSettings.ShowCloseButton || _browserSettings.ShowHelpButton || _browserSettings.ShowURLBar)
            {

                //Show the top bar
                this.TopBar.Visibility = Visibility.Visible;


                //See if we should show the buttons
                if (_browserSettings.ShowCloseButton)
                    this.CloseButton.Visibility = Visibility.Visible;
                else
                    this.CloseButton.Visibility = Visibility.Collapsed;

                if (_browserSettings.ShowHelpButton)
                    this.HelpButton.Visibility = Visibility.Visible;
                else
                    this.HelpButton.Visibility = Visibility.Collapsed;

                //Show the URL bar?
                if (_browserSettings.ShowURLBar)
                    this.URLBar.Visibility = Visibility.Visible;
                else
                    this.URLBar.Visibility = Visibility.Hidden;
            }
            else
            {
                //Collapse the top bar so the browser takes up the full screen
                this.TopBar.Visibility = Visibility.Collapsed;
            }
        }

        private void CycleURL_Tick(object sender, EventArgs e)
        {
            _urlIndex++;
            if (_browserSettings.CycleURLs.Length == _urlIndex)
            {
                _urlIndex = 0;
            }
            this.Browser.Address = _browserSettings.CycleURLs[_urlIndex];
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

                if (Settings.KioskSettings.GetConfigPath().Contains("://") == false)
                {
                    Dialogs.Configuration configurationDialog = new Dialogs.Configuration();
                    configurationDialog.ShowDialog();
                    LoadSettings();
                }
            }
            else if (e.Key == Key.Escape)
            {

                this.Close();
            }else if(e.Key == Key.F1)
            {
                var HelpDialog = new Dialogs.HelpDialog(_browserSettings);
                HelpDialog.ShowDialog();
            }
        }


        /// <summary>
        /// Sets the embedded browser up for use
        /// </summary>
        /// <param name="browserSettings">Settings to use to configure the browser</param>
        private void ConfigureBrowserSettings(Settings.Settings browserSettings)
        {
            try
            {
                this.Browser.BrowserSettings.Databases = CefSharp.CefState.Disabled;
                this.Browser.BrowserSettings.JavascriptAccessClipboard = CefSharp.CefState.Disabled;
                this.Browser.BrowserSettings.JavascriptDomPaste = CefSharp.CefState.Disabled;
                this.Browser.BrowserSettings.Plugins = CefSharp.CefState.Disabled;
                this.Browser.BrowserSettings.WebSecurity = CefSharp.CefState.Enabled;
            }
            catch (Exception e)
            {
                //This tends to happen after config has reloaded
                Logger.DefaultLogger.LogWarning(String.Format("Failed to set browser settings. {0}", e.Message));
            }

            //Setup with the first URL first
            if (browserSettings.CycleURLs.Length >= 1)
            {
                this.Browser.Address = browserSettings.CycleURLs[0];
                _urlIndex = 0;
            }
        }

        private void Browser_Loaded(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Visible;
        }



       
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (_browserSettings.PromptOnExit)
            {
                var cw = new Dialogs.CloseDialog(_browserSettings);
                cw.ShowDialog();
                if (cw.ConfirmClose)
                {
                    //Close all the cover windows
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
            else
            {

                //Close all the cover windows
                foreach (var bw in _blankWindows)
                {
                    bw.Close();
                }
                
            }
            
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            var HelpDialog = new Dialogs.HelpDialog(_browserSettings);
            HelpDialog.ShowDialog();
        }

        private void UpdateAddressBar()
        {
            // The loading state is in a seperate thread, we need to use dispatcher to call back into the UI thread
            this.URLBar.Dispatcher.BeginInvoke(new Action(() => URLBar.Text = Browser.Address));
        }

        private void Browser_LoadingStateChanged(object sender, CefSharp.LoadingStateChangedEventArgs e)
        {

            UpdateAddressBar();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // The close button doesn't care
            this.Close();
        }

        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _lastMouseMove = DateTime.Now;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }
    }
}