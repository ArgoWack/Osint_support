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
    /// Logika interakcji dla klasy UpdateKeys.xaml
    /// </summary>
    public partial class UpdateKeys : Window
    {
        public UpdateKeys()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //haveibeenpwned api key to be received from user
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            //dehashed username to be received from user
        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {
            //dehashed api key to be received from user
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Return button
            var UpdateKeys = new MainWindow();
            UpdateKeys.Show();
            this.Close();
        }
    }
}
