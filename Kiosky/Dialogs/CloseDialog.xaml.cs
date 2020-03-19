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
        bool unlockedWithAdmin = false;
        Settings.Settings _settings;
        /// <summary>
        /// 
        /// </summary>
        public CloseDialog(Settings.Settings s)
        {
            InitializeComponent();

            _settings = s;
            unlockedWithAdmin = !s.RequirePasswordToExit;


            //Hide the admin password boxes
            if(!s.RequirePasswordToExit)
            {
                
                HideAdmin();
            }

            this.DataContext = this;
        }

        private void HideAdmin()
        {
            this.AdminPasswordBox.Visibility = Visibility.Collapsed;
            this.AdminPasswordLabel.Visibility = Visibility.Collapsed;
            this.CheckAdminButton.Visibility = Visibility.Collapsed;
            NoButton.IsEnabled = true;
            YesButton.IsEnabled = true;
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

        

        private void AdminPasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab)
                if (Settings.PasswordManager.ComparePassword(AdminPasswordBox.Password, _settings))
                {
                    unlockedWithAdmin = true;
                    HideAdmin();
                }
        }

        private void CheckAdminButton_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.PasswordManager.ComparePassword(AdminPasswordBox.Password, _settings))
            {
                unlockedWithAdmin = true;
                HideAdmin();
            }
        }
    }
}
