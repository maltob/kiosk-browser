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
    /// Interaction logic for CloseDialog.xaml
    /// </summary>
    public partial class CloseDialog : Window
    {
        /// <summary>
        /// Whether or not the user confirmed closing, the main app checks this to see if it should close
        /// </summary>
        public bool ConfirmClose = false;
        /// <summary>
        /// 
        /// </summary>
        public CloseDialog()
        {
            InitializeComponent();
        }

        private void Button_Click_Yes(object sender, RoutedEventArgs e)
        {
            ConfirmClose = true;
            this.Close();
        }

        private void Button_Click_No(object sender, RoutedEventArgs e)
        {
            ConfirmClose = false;
            this.Close();
        }
    }
}
