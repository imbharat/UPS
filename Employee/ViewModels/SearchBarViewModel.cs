using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Employee.ViewModels
{
    public class SearchBarViewModel : INotifyPropertyChanged
    {
        private string _searchId;
        private string _searchName;
        private string _searchEmail;
        private string _searchGender;
        private string _searchStatus;

        public string SearchId
        {
            get { return _searchId; }
            set { _searchId = value; OnPropertyChanged(); }
        }
        public string SearchName
        {
            get { return _searchName; }
            set { _searchName = value; OnPropertyChanged(); }
        }
        public string SearchEmail
        {
            get { return _searchEmail; }
            set { _searchEmail = value; OnPropertyChanged(); }
        }
        public string SearchGender
        {
            get { return _searchGender; }
            set { _searchGender = value; OnPropertyChanged(); }
        }
        public string SearchStatus
        {
            get { return _searchStatus; }
            set { _searchStatus = value; OnPropertyChanged(); }
        }

        public ICommand SearchCommand { get; }
        public ICommand ClearCommand { get; }

        public event EventHandler<string> OnEmployeeSearch;
        public event EventHandler OnClearSearch;

        public SearchBarViewModel()
        {
            SearchCommand = new RelayCommand(param => Search(), param => true);
            ClearCommand = new RelayCommand(param => Clear(), param => true);
        }

        private void Search()
        {
            string searchString = string.Empty;
            if (!string.IsNullOrEmpty(SearchId))
            {
                searchString += $"&id={SearchId}";
            }
            if (!string.IsNullOrEmpty(SearchName))
            {
                searchString += $"&name={SearchName}";
            }
            if (!string.IsNullOrEmpty(SearchEmail))
            {
                searchString += $"&email={SearchEmail}";
            }
            if (!string.IsNullOrEmpty(SearchGender))
            {
                searchString += $"&gender={SearchGender}";
            }
            if (!string.IsNullOrEmpty(SearchStatus))
            {
                searchString += $"&status={SearchStatus}";
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                OnEmployeeSearch?.Invoke(this, searchString);
            }
        }

        private void Clear()
        {
            SearchId = string.Empty;
            SearchName = string.Empty;
            SearchEmail = string.Empty;
            SearchGender = null;
            SearchStatus = null;
            OnClearSearch?.Invoke(this, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
