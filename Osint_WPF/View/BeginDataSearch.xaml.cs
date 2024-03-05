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
            foreach (var control in TextBoxesGrid.Children)
            {
                if (control is TextBox textBox)
                {
                    textBox.Text = "";
                    EmailTextBox.Text = "";
                    BreachDateDatePickerBox.Text = "";
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                UpdateTextBoxesVisibility();
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                UpdateTextBoxesVisibility();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // had to add this to prevent errors resulting from trying to acces UpdateTextBoxesVisibility before form was loaded
            UpdateTextBoxesVisibility();
        }

        private void UpdateTextBoxesVisibility()
        {
            bool isHaveibeenpwnedChecked = HaveibeenpwnedCheckBox.IsChecked ?? false;
            bool isDehashedChecked = DehashedCheckBox.IsChecked ?? false;

            // all textboxes (except for email) are in the same grid named 'TextBoxesGrid', there's also DataPicker
            foreach (var control in TextBoxesGrid.Children)
            {
                if (control is TextBox textBox)
                {
                    // Default to enabled
                    EmailTextBox.IsEnabled = true;
                    BreachDateDatePickerBox.IsEnabled = true;
                    textBox.IsEnabled = true;

                    if (isHaveibeenpwnedChecked && isDehashedChecked)
                    {
                        // when both are checked, all textboxes should be enabled
                    }

                    // when Dehashed is checked and Haveibeenpwned is unchecked, all textboxes excluding "Breach date" should be enabled
                    else if (isDehashedChecked && !isHaveibeenpwnedChecked)
                    {
                        BreachDateDatePickerBox.IsEnabled = false;
                    }
                    // when Dehashed is unchecked and Haveibeenpwned is checked, only email, password, and breach date should be enabled
                    else if (!isDehashedChecked && isHaveibeenpwnedChecked)
                    {
                        textBox.IsEnabled = false;
                        if (textBox.Name == "EmailTextBox" || textBox.Name == "PasswordTextBox")
                        {
                            textBox.IsEnabled = true;
                            BreachDateDatePickerBox.IsEnabled = true;
                        }
                    }

                    else
                    {
                        // when neither is checked
                        textBox.IsEnabled = false;
                        EmailTextBox.IsEnabled = false;
                        BreachDateDatePickerBox.IsEnabled = false;
                    }
                }
            }
        }
    }
}