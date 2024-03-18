using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Input;
using System.Windows;
using dotenv.net;
using System.Diagnostics;
namespace Osint_WPF.ViewModels
{
    public class UpdateKeysViewModel : ViewModelBase
    {
        private string _hibpApiKey;
        private string _dehashedUsername;
        private string _dehashedApiKey;
        public string HibpApiKey
        {
            get => _hibpApiKey;
            set => SetProperty(ref _hibpApiKey, value);
        }
        public string DehashedUsername
        {
            get => _dehashedUsername;
            set => SetProperty(ref _dehashedUsername, value);
        }
        public string DehashedApiKey
        {
            get => _dehashedApiKey;
            set => SetProperty(ref _dehashedApiKey, value);
        }
        public ICommand UpdateCommand { get; }
        public UpdateKeysViewModel()
        {
            UpdateCommand = new RelayCommand(ExecuteUpdateCommand);
        }
        private void ExecuteUpdateCommand()
        {
            // implements the logic to update the .env file based on the properties
            if (!string.IsNullOrWhiteSpace(HibpApiKey))
            {
                SaveInputToEnvFile(HibpApiKey, "hibp_ApiKey=");
            }
            if (!string.IsNullOrWhiteSpace(DehashedUsername))
            {
                SaveInputToEnvFile(DehashedUsername, "dehashedApiUsername=");
            }
            if (!string.IsNullOrWhiteSpace(DehashedApiKey))
            {
                SaveInputToEnvFile(DehashedApiKey, "dehashedApiKey=");
            }
        }
        private void SaveInputToEnvFile(string userInput, string dotenvKey)
        {
            //.env file path
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

            try
            {
                if (File.Exists(envFilePath))
                {
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

                    // restart is required for .env file to update during the same sesion
                    RestartApplication();
                }
                else
                {
                    throw new ApplicationException(".env file is missing");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error while rewriting keys",ex);
            }
        }
        private void RestartApplication()
        {
            // main menu path
            var exePath = Process.GetCurrentProcess().MainModule.FileName;

            // starts new instance of app
            Process.Start(exePath);

            // closes the old one with old .env
            Application.Current.Shutdown();
        }
    }
}