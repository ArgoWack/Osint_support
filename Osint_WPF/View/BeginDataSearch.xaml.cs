using dotenv.net;
using Osint_WPF.View;
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //return
            var MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //search
            CollectUserData();

            var SearchResults = new SearchResults();
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

            public static string CheckBreaches(UserData UserData)
            {
                // to be expanded, also no BreachDate considerd yet, locking resources, loadingbar to be done, logic to be finished

                //installed NuGet packed to be able to load sensitive data from .env file
                DotEnv.Load();

                //name of my api for identification purposes
                string apiName = Environment.GetEnvironmentVariable("myApiName");

                string dehashedApiUsername = Environment.GetEnvironmentVariable("dehashedApiUsername");
                string dehashedApiKey = Environment.GetEnvironmentVariable("dehashedApiKey");


                if (UserData.DehashedChecked && UserData.HaveIBeenPwnedChecked)
                {
                    // when both are checked, all textboxes should be enabled

                    //UserData.Email
                    //string hibp_ApiKey = Environment.GetEnvironmentVariable("hibp_ApiKey");
                    //string leak_search = "email:\"" + UserData.Email + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //var (breaches, pastes) = await HaveIBeenPwned.CheckIfEmailHasBeenPwned(UserData.Email, hibp_ApiKey, apiName);
                    //PrintExistingDehashedData(leakedEntries, UserData.Email);
                    //PrintExistingBreachesPastes(UserData.Email, breaches, pastes);

                    //Thread.Sleep(6000);

                    //UserData.Username
                    //string leak_search = "username:\"" + UserData.Username + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //PrintExistingDehashedData(leakedEntries, UserData.Username);

                    //UserData.Password
                    //string leak_search = "password:\"" + UserData.Password + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //var pwnedHashes = await HaveIBeenPwned.CheckIfPasswordHasBeenPwned(UserData.Password);
                    //Thread.Sleep(6000);
                    //PrintExistingDehashedData(leakedEntries, UserData.Password);
                    //PrintExistingPwnedHashes(pwnedHashes);

                    //UserData.Phone
                    //string leak_search = "phone:\"" + UserData.Phone + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //PrintExistingDehashedData(leakedEntries, UserData.Phone);

                    //UserData.Address
                    //string leak_search = "address:\"" + UserData.Address + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //PrintExistingDehashedData(leakedEntries, UserData.Address);

                    //UserData.HashedPassword
                    //string leak_search = "hashed_password:\"" + UserData.HashedPassword + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //PrintExistingDehashedData(leakedEntries, UserData.HashedPassword);

                    //UserData.HashType
                    //string leak_search = "hash_type:\"" + UserData.HashType + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //PrintExistingDehashedData(leakedEntries, UserData.HashType);

                    //UserData.Name
                    //string leak_search = "name:\"" + UserData.Name + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //PrintExistingDehashedData(leakedEntries, UserData.Name);

                    //UserData.Id
                    //string leak_search = "id:\"" + UserData.Id + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //PrintExistingDehashedData(leakedEntries, UserData.Id);

                    //UserData.IpAddress
                    //string leak_search = "ip_address:\"" + UserData.IpAddress + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //PrintExistingDehashedData(leakedEntries, UserData.IpAddress);

                    //UserData.Vin
                    //string leak_search = "vin:\"" + UserData.Vin + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //PrintExistingDehashedData(leakedEntries, UserData.Vin);

                    //UserData.DatabaseName
                    //string leak_search = "database_name:\"" + UserData.DatabaseName + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //PrintExistingDehashedData(leakedEntries, UserData.DatabaseName);

                }
                else if (UserData.DehashedChecked && !UserData.HaveIBeenPwnedChecked)
                {
                    // when Dehashed checked and haveibeenpwned unechecked

                    //UserData.Email
                    //string leak_search = "email:\"" + UserData.Email + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //PrintExistingDehashedData(leakedEntries, UserData.Email);

                    //UserData.Username
                    //string leak_search = "username:\"" + UserData.Username + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //PrintExistingDehashedData(leakedEntries, UserData.Username);

                    //UserData.Password
                    //string leak_search = "password:\"" + UserData.Password + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //PrintExistingDehashedData(leakedEntries, UserData.Password);

                    //UserData.Phone
                    //string leak_search = "phone:\"" + UserData.Phone + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //PrintExistingDehashedData(leakedEntries, UserData.Phone);

                    //UserData.Address
                    //string leak_search = "address:\"" + UserData.Address + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //PrintExistingDehashedData(leakedEntries, UserData.Address);

                    //UserData.HashedPassword
                    //string leak_search = "hashed_password:\"" + UserData.HashedPassword + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //PrintExistingDehashedData(leakedEntries, UserData.HashedPassword);

                    //UserData.HashType
                    //string leak_search = "hash_type:\"" + UserData.HashType + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //PrintExistingDehashedData(leakedEntries, UserData.HashType);

                    //UserData.Name
                    //string leak_search = "name:\"" + UserData.Name + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //PrintExistingDehashedData(leakedEntries, UserData.Name);

                    //UserData.Id
                    //string leak_search = "id:\"" + UserData.Id + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //PrintExistingDehashedData(leakedEntries, UserData.Id);

                    //UserData.IpAddress
                    //string leak_search = "ip_address:\"" + UserData.IpAddress + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //PrintExistingDehashedData(leakedEntries, UserData.IpAddress);

                    //UserData.Vin
                    //string leak_search = "vin:\"" + UserData.Vin + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //PrintExistingDehashedData(leakedEntries, UserData.Vin);

                    //UserData.DatabaseName
                    //string leak_search = "database_name:\"" + UserData.DatabaseName + "\"";
                    //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
                    //PrintExistingDehashedData(leakedEntries, UserData.DatabaseName);
                }
                else if (!UserData.DehashedChecked && UserData.HaveIBeenPwnedChecked)
                {
                    // when Dehashed is unchecked and Haveibeenpwned is checked, only email, password, and breach date should be enabled

                    //UserData.Email
                    //string hibp_ApiKey = Environment.GetEnvironmentVariable("hibp_ApiKey");
                    //var (breaches, pastes) = await HaveIBeenPwned.CheckIfEmailHasBeenPwned(UserData.Email, hibp_ApiKey, apiName);
                    //PrintExistingBreachesPastes(UserData.Email, breaches, pastes);

                    //Thread.Sleep(6000);

                    //UserData.Password
                    //var pwnedHashes = await HaveIBeenPwned.CheckIfPasswordHasBeenPwned(UserData.Password);
                    //PrintExistingPwnedHashes(pwnedHashes);
                }
                else
                {
                    // when neither is checked

                    //nothing to do
                }

                return "";
            }
        }

        private UserData CollectUserData()
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

    }
}