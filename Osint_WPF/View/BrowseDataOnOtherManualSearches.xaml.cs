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
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace Osint_WPF
{
    public partial class BrowseDataOnOtherManualSearches : Window
    {
        public BrowseDataOnOtherManualSearches()
        {
            InitializeComponent();
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //Return button
            var UpdateKeys = new MainWindow();
            UpdateKeys.Show();
            this.Close();
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            //opens hyperlink in browser
            Process.Start(new ProcessStartInfo
            {
                FileName = e.Uri.AbsoluteUri,
                UseShellExecute = true
            });
            e.Handled = true;
        }
    }
}