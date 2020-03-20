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
    /// Interaction logic for HelpDialog.xaml
    /// </summary>
    public partial class HelpDialog : Window
    {
        /// <summary>
        /// A help window opening the help page
        /// </summary>
        public HelpDialog(Settings.Settings s)
        {
            InitializeComponent();
            this.Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            Browser.Address = s.HelpPageURL;
            if (s.HelpPageURL == null)
                Browser.Address = "about:help";
        }

        private void Browser_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            e.Handled = false;

        }

        private void Browser_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {

                this.Close();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
