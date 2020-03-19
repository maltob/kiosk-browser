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
using System.Windows.Shapes;

namespace Kiosky.Dialogs
{
    /// <summary>
    /// Interaction logic for BlankWindow.xaml
    /// </summary>
    public partial class BlankWindow : Window
    {
        Screen _screenCovering;
        /// <summary>
        /// Creates a new blank window to cover a screen
        /// </summary>
        /// <param name="ScreenToCover">The screen that should be covered over</param>
        public BlankWindow(Screen ScreenToCover)
        {
            InitializeComponent();

            _screenCovering = ScreenToCover;



        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Normal;
            this.Left = _screenCovering.Bounds.Left;

            this.Height = _screenCovering.Bounds.Height;
            this.Top = _screenCovering.Bounds.Top;
            this.Width = _screenCovering.Bounds.Width;
        }
    }
}
