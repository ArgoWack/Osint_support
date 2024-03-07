using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Osint_WPF.ViewModel
{
    public class DehashedViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Model.Dehashed.Entry> _entries;
        public ObservableCollection<Model.Dehashed.Entry> Entries
        {
            get => _entries;
            set
            {
                _entries = value;
                OnPropertyChanged(nameof(Entries));
            }
        }

        // ICommand implementation to trigger search
        public ICommand SearchCommand { get; }

        public DehashedViewModel()
        {
            Entries = new ObservableCollection<Model.Dehashed.Entry>();
            SearchCommand = new AsyncCommand(async (param) => await ExecuteSearchAsync(null));
        }

        private async Task ExecuteSearchAsync(object parameter)
        {
            //parameter is the search query
            var data = parameter.ToString();
            var results = await Model.Dehashed.CheckIfDataHasBeenLeaked(data);
            Entries.Clear();
            foreach (var entry in results)
            {
                Entries.Add(entry);
            }
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
