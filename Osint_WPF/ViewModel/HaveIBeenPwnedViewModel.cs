using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Windows.Input;
using Osint_WPF.Model;

namespace Osint_WPF.ViewModel
{
    public class HaveIBeenPwnedViewModel : INotifyPropertyChanged
    {
        private readonly HaveIBeenPwned haveIBeenPwnedService;

        public ObservableCollection<HaveIBeenPwned.Breach> Breaches { get; } = new ObservableCollection<HaveIBeenPwned.Breach>();
        public ObservableCollection<HaveIBeenPwned.Paste> Pastes { get; } = new ObservableCollection<HaveIBeenPwned.Paste>();

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private ObservableCollection<string> _pwnedPasswords = new ObservableCollection<string>();
        public ObservableCollection<string> PwnedPasswords
        {
            get => _pwnedPasswords;
            set
            {
                _pwnedPasswords = value;
                OnPropertyChanged(nameof(PwnedPasswords));
            }
        }

        public ICommand CheckEmailCommand { get; }

        public HaveIBeenPwnedViewModel(string hibpApiKey, string myApiName, string apiKey)
        {
            // initializes HaveIBeenPwned with API key and app name
            haveIBeenPwnedService = new HaveIBeenPwned(hibpApiKey, myApiName, apiKey);


            CheckEmailCommand = new AsyncCommand(async (param) => await ExecuteCheckEmailCommand(param), CanExecuteCheckEmailCommand);
            CheckPasswordCommand = new AsyncCommand(async (param) => await ExecuteCheckPasswordCommand(param));
        }

        private bool CanExecuteCheckEmailCommand(object parameter) => !string.IsNullOrWhiteSpace(Email);

        private async Task ExecuteCheckEmailCommand(object parameter)
        {
            var (breaches, pastes) = await haveIBeenPwnedService.CheckIfEmailHasBeenPwned(Email);

            Breaches.Clear();
            foreach (var breach in breaches)
            {
                Breaches.Add(breach);
            }

            Pastes.Clear();
            foreach (var paste in pastes)
            {
                Pastes.Add(paste);
            }
        }

        public ICommand CheckPasswordCommand { get; }

        private async Task ExecuteCheckPasswordCommand(object parameter)
        {
            var password = parameter as string;

            if (!string.IsNullOrWhiteSpace(password))
            {
                var pwnedHashes = await haveIBeenPwnedService.CheckIfPasswordHasBeenPwned(password);
                PwnedPasswords.Clear();
                foreach (var hash in pwnedHashes)
                {
                    PwnedPasswords.Add(hash);
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
