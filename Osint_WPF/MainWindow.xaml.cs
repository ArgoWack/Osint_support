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
            // Add similar conditions for other expanders if any
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
            // Add similar conditions for other expanders if any
        }
    }
}