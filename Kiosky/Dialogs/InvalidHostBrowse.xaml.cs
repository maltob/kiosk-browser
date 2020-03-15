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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class InvalidHostBrowseDialog : Window
    {
        public InvalidHostBrowseDialog(string Host)
        {
            InitializeComponent();
            this.HostMessageTextBlock.Text = "You attempted to browse to an invalid host : "+Host;
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            this.Topmost = false;
            this.Topmost = true;
            this.Activate();
              
        }
    }
}
