using dotenv.net;
using Osint_WPF.View;
using Osint_WPF.ViewModel;
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
using static Osint_WPF.BeginDataSearch;

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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //return
            var MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //search

            var userData = CollectUserData();
            var searchResultsText = await UserData.CheckBreachesAsync(userData);

            //opens SearchResults window and display results
            var SearchResults = new SearchResults(searchResultsText);
            SearchResults.Show();
            this.Close();
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

        public UserData CollectUserData()
        {
            return new UserData
            {
                Email = EmailTextBox.Text,
                Username = UsernameTextBox.Text,
                Password = PasswordTextBox.Text,
                Phone = PhoneTextBox.Text,
                Address = AddressTextBox.Text,
                HashedPassword = HashedPasswordTextBox.Text,
                HashType = HashTypeTextBox.Text,
                Name = NameTextBox.Text,
                Id = IdTextBox.Text,
                IpAddress = IpAddressTextBox.Text,
                Vin = VinTextBox.Text,
                DatabaseName = DatabaseNameTextBox.Text,
                BreachDate = BreachDateDatePickerBox.SelectedDate,
                HaveIBeenPwnedChecked = HaveibeenpwnedCheckBox.IsChecked ?? false,
                DehashedChecked = DehashedCheckBox.IsChecked ?? false
            };
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

        public class UserData
        {
            public string Email { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public string HashedPassword { get; set; }
            public string HashType { get; set; }
            public string Name { get; set; }
            public string Id { get; set; }
            public string IpAddress { get; set; }
            public string Vin { get; set; }
            public string DatabaseName { get; set; }
            public DateTime? BreachDate { get; set; }
            public bool HaveIBeenPwnedChecked { get; set; }
            public bool DehashedChecked { get; set; }

            public static async Task<string> CheckBreachesAsync(UserData userData)
            {
                // to be expanded, also no BreachDate considerd yet, locking resources, loadingbar to be done, logic to be finished

                //installed NuGet packed to be able to load sensitive data from .env file
                DotEnv.Load();

                //name of my api for identification purposes
                string apiName = Environment.GetEnvironmentVariable("myApiName");

                string dehashedApiUsername = Environment.GetEnvironmentVariable("dehashedApiUsername");
                string dehashedApiKey = Environment.GetEnvironmentVariable("dehashedApiKey");

                string hibp_ApiKey = Environment.GetEnvironmentVariable("hibp_ApiKey");

                //instances of ViewModels:
                var dehashedViewModel = new DehashedViewModel(dehashedApiUsername, dehashedApiKey, apiName);
                var haveIBeenPwnedViewModel = new HaveIBeenPwnedViewModel(apiName, hibp_ApiKey);

                var resultsBuilder = new StringBuilder();

                if (userData.DehashedChecked && userData.HaveIBeenPwnedChecked)
                {
                    var dehashedResults = await dehashedViewModel.ExecuteSearchAsync(userData);
                    resultsBuilder.Append(dehashedResults);
                    var hibpResultsEmail = await haveIBeenPwnedViewModel.CheckEmailAsync(userData.Email);
                    resultsBuilder.Append(hibpResultsEmail);
                    var hibpResultsPassword = await haveIBeenPwnedViewModel.CheckPasswordAsync(userData.Password);
                    resultsBuilder.Append(hibpResultsPassword);
                }
                else if (userData.DehashedChecked)
                {
                    var dehashedResults = await dehashedViewModel.ExecuteSearchAsync(userData);
                    resultsBuilder.Append(dehashedResults);
                }
                else if (userData.HaveIBeenPwnedChecked)
                {
                    var hibpResultsEmail = await haveIBeenPwnedViewModel.CheckEmailAsync(userData.Email);
                    resultsBuilder.Append(hibpResultsEmail);
                    var hibpResultsPassword = await haveIBeenPwnedViewModel.CheckPasswordAsync(userData.Password);
                    resultsBuilder.Append(hibpResultsPassword);
                }
                return resultsBuilder.ToString();
            }
        }
    }
}