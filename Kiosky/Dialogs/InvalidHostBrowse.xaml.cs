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
        /// <summary>
        /// An XAML dialog box to display a message when the user browses to a not allowed domain
        /// 
        /// </summary>
        /// <param name="Host">The not allowed domain that as browsed to such as badwebsite.co.uk</param>
        public InvalidHostBrowseDialog(string Host)
        {
            InitializeComponent();
            this.HostMessageTextBlock.Text = String.Format("You attempted to browse to an invalid host : {0}", Host);
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            this.Topmost = false;
            this.Topmost = true;
            this.Activate();
              
        }
    }
}
