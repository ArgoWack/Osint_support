﻿using Osint_WPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
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

        public async Task<string> CheckEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return "Invalid email.";

            StringBuilder resultsBuilder = new StringBuilder();
            try
            {
                var (breaches, pastes) = await hibpService.CheckIfEmailHasBeenPwned(email);
                Breaches.Clear();
                Pastes.Clear();
                foreach (var breach in breaches)
                {
                    Breaches.Add(breach);
                    resultsBuilder.AppendLine($"Breach: {breach.Name}");
                }
                foreach (var paste in pastes)
                {
                    Pastes.Add(paste);
                    resultsBuilder.AppendLine($"Paste: {paste.Source}");
                }
            }
            catch (Exception ex)
            {
                resultsBuilder.AppendLine($"Error: {ex.Message}");
            }

            return resultsBuilder.ToString();
        }

        public async Task<string> CheckPasswordAsync(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return "Invalid password.";

            StringBuilder resultsBuilder = new StringBuilder();
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