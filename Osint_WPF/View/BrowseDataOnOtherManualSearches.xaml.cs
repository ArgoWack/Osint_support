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

namespace Osint_WPF
{
    /// <summary>
    /// Logika interakcji dla klasy BrowseDataOnOtherManualSearches.xaml
    /// </summary>
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
    }
}