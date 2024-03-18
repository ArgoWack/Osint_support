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
using System.IO;
using Osint_WPF.ViewModels;

namespace Osint_WPF
{
    public partial class UpdateKeys : Window
    {
        public UpdateKeys()
        {
            InitializeComponent();
            DataContext = new UpdateKeysViewModel();
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
            //Confirms changes overriting .env file unless no changes
            //logic handled in ViewModel
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Return button
            var MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
        }
    }
}