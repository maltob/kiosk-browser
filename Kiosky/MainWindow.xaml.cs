﻿using System;
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
        /// <summary>
        /// The primary application window, called by the App
        /// </summary>
        public MainWindow()
        {



            InitializeComponent();
            _browserSettings = Settings.TestingSettings.GetDefaultLockdownSettings();
            Browser.RequestHandler = new Handlers.KioskyRequestHandler(_browserSettings);
            ConfigureBrowserSettings(_browserSettings);
           
            this.Visibility = Visibility.Visible;

            _lockdown = new Lockdown.WindowsLockdown();
            

        }
        private void Browser_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            e.Handled = false;


        }

        private void Browser_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // If they hit pause, show the Configuration dialog
            if(e.Key == Key.Pause)
            {
                Dialogs.Configuration configurationDialog = new Dialogs.Configuration();
                configurationDialog.ShowDialog();
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
    }
}