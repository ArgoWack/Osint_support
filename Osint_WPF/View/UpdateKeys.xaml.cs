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
            //Confirms changes overriting .env file unless no changes
            if (txtHibpApiKey.Text != "")
            {
                SaveInputToEnvFile(txtHibpApiKey.Text, "hibp_ApiKey=");
            }
            if (txtDehashedUsername.Text != "")
            {
                SaveInputToEnvFile(txtDehashedUsername.Text, "dehashedApiUsername=");
            }
            if (txtDehashedApiKey.Text != "")
            {
                SaveInputToEnvFile(txtDehashedApiKey.Text, "dehashedApiKey=");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Return button
            var UpdateKeys = new MainWindow();
            UpdateKeys.Show();
            this.Close();
        }

        private void SaveInputToEnvFile(string userInput, string dotenvKey)
        {
            DirectoryInfo currentDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            while (currentDirectory != null && !currentDirectory.GetFiles("*.csproj", SearchOption.TopDirectoryOnly).Any())
            {
                currentDirectory = currentDirectory.Parent;
            }

            if (currentDirectory == null)
            {
                MessageBox.Show("Project directory not found.");
                return;
            }
            string envFilePath = System.IO.Path.Combine(currentDirectory.FullName, ".env");

            if (File.Exists(envFilePath))
            {
                MessageBox.Show(envFilePath);

                // reads all lines in .env file
                string[] lines = File.ReadAllLines(envFilePath);

                // updates .env file lines which match the input, to update them
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith(dotenvKey))
                    {
                        lines[i] = dotenvKey + userInput;
                        break;
                    }
                }
                File.WriteAllLines(envFilePath, lines);
            }
            else
            {
                MessageBox.Show(".env file not found.");
            }
        }
    }
}