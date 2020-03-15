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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kiosky
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {



            InitializeComponent();
            Browser.RequestHandler = new Handlers.KioskyRequestHandler(new string[]{ ""});
            ConfigureBrowserSettings();

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

        private void Browser_FrameLoadStart(object sender, CefSharp.FrameLoadStartEventArgs e)
        {
            e.Frame.ExecuteJavaScriptAsync("window.print=function(){}");
        }

        private void ConfigureBrowserSettings()
        {
            this.Browser.BrowserSettings.Databases = CefSharp.CefState.Disabled;
            this.Browser.BrowserSettings.JavascriptAccessClipboard = CefSharp.CefState.Disabled;
            this.Browser.BrowserSettings.JavascriptDomPaste = CefSharp.CefState.Disabled;
            this.Browser.BrowserSettings.Plugins = CefSharp.CefState.Disabled;
            this.Browser.BrowserSettings.WebSecurity = CefSharp.CefState.Enabled;
        }
    }
}