using Osint_WPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Osint_WPF.ViewModel
{
    public class HaveIBeenPwnedViewModel : INotifyPropertyChanged
    {
        private readonly HaveIBeenPwned hibpService;

        public ObservableCollection<HaveIBeenPwned.Breach> Breaches { get; } = new ObservableCollection<HaveIBeenPwned.Breach>();
        public ObservableCollection<HaveIBeenPwned.Paste> Pastes { get; } = new ObservableCollection<HaveIBeenPwned.Paste>();
        public ObservableCollection<string> PwnedPasswords { get; } = new ObservableCollection<string>();

        public ICommand CheckEmailCommand { get; private set; }
        public ICommand CheckPasswordCommand { get; private set; }

        public HaveIBeenPwnedViewModel(string myApiName, string apiKey)
        {
            hibpService = new HaveIBeenPwned(myApiName, apiKey);
            CheckEmailCommand = new AsyncCommand(async (email) => await CheckEmailAsync(email as string));
            CheckPasswordCommand = new AsyncCommand(async (password) => await CheckPasswordAsync(password as string));
        }

        public async Task<string> CheckEmailAsync(string email, DateTime? userBreachDate = null)
        {
            if (string.IsNullOrWhiteSpace(email)) return ""; // empty field

            StringBuilder resultsBuilder = new StringBuilder();

            resultsBuilder.Append("Results for email: "+email+ "\n");
            try
            {
                var (breaches, pastes) = await hibpService.CheckIfEmailHasBeenPwned(email, userBreachDate);

                Breaches.Clear();
                Pastes.Clear();
                resultsBuilder.Append("Breaches: \n");

                foreach (var breach in breaches)
                {
                    Breaches.Add(breach);
                    resultsBuilder.AppendLine($"Breach name: {breach.Name} date: {breach.BreachDate}");
                }

                resultsBuilder.Append("Pastes: \n");
                foreach (var paste in pastes)
                {
                    Pastes.Add(paste);
                    resultsBuilder.AppendLine($"Paste source: {paste.Source} id: {paste.Id} date: {paste.Date}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                // HTTP request error
                resultsBuilder.AppendLine($"Network error: {httpEx.Message}");
            }
            catch (JsonException jsonEx)
            {
                //JSON parsing errors
                resultsBuilder.AppendLine($"Data parsing error: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                //other errors
                resultsBuilder.AppendLine($"Error: {ex.Message}");
            }
            return resultsBuilder.ToString();
        }

        public async Task<string> CheckPasswordAsync(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return ""; // empty field

            StringBuilder resultsBuilder = new StringBuilder();

            resultsBuilder.Append("Results for password: " + password + "\n");
            try
            {
                var pwnedHashes = await hibpService.CheckIfPasswordHasBeenPwned(password);
                PwnedPasswords.Clear();
                if (pwnedHashes.Any())
                {
                    foreach (var hash in pwnedHashes)
                    {
                        PwnedPasswords.Add(hash);
                        resultsBuilder.AppendLine($"Pwned hash: {hash}");
                    }
                }
                else
                {
                    resultsBuilder.AppendLine("Password not found in any breach.");
                }
            }
            catch (Exception ex)
            {
                resultsBuilder.AppendLine($"Error: {ex.Message}");
            }
            return resultsBuilder.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}