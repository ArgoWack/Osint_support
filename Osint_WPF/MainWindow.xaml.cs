using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Osint_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            if (sender == expander1)
            {
                expanderRow1.Height = GridLength.Auto;
            }
            else if (sender == expander2)
            {
                expanderRow2.Height = GridLength.Auto;
            }
            else if (sender == expander3)
            {
                expanderRow3.Height = GridLength.Auto;
            }
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            if (sender == expander1)
            {
                expanderRow1.Height = new GridLength(30);
            }
            else if (sender == expander2)
            {
                expanderRow2.Height = new GridLength(30);
            }
            else if (sender == expander3)
            {
                expanderRow3.Height = new GridLength(30);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //UpdateKeys button
            var UpdateKeys = new UpdateKeys();
            UpdateKeys.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //BeginDataSearch button
            var BeginDataSearch = new BeginDataSearch();
            BeginDataSearch.Show();
            this.Close();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //BrowseDataOnOtherManualSearches button
            var BrowseDataOnOtherManualSearches = new BrowseDataOnOtherManualSearches();
            BrowseDataOnOtherManualSearches.Show();
            this.Close();    

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // Exit button
            Application.Current.Shutdown();
        }
    }
}