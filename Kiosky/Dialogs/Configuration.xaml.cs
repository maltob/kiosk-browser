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
        /// <summary>
        /// Create a dialog window to allow for modifying the Kiosk Browsers configuration
        /// </summary>
        public Configuration()
        {
            InitializeComponent(); 
            this.Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            this.Activate();
        }
    }
}
