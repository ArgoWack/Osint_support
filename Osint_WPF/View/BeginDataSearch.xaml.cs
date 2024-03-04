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
    /// Logika interakcji dla klasy BeginDataSearch.xaml
    /// </summary>
    public partial class BeginDataSearch : Window
    {
        public BeginDataSearch()
        {
            InitializeComponent();
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            if (sender == expander1)
            {
                expanderRow1.Height = GridLength.Auto;
            }
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            if (sender == expander1)
            {
                expanderRow1.Height = new GridLength(30);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //Return button
            var UpdateKeys = new MainWindow();
            UpdateKeys.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //return
            //Return button
            var MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //search
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //clear
        }
    }
}
