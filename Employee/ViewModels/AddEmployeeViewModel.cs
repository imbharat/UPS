using Employee.Models;
using Employee.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Employee.ViewModels
{
    public class AddEmployeeViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _email;
        private string _gender;
        private string _status;
        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        public string Gender
        {
            get { return _gender; }
            set { _gender = value; OnPropertyChanged(); }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged(); }
        }

        public event EventHandler<EmployeeModel> OnSubmit;
        public event EventHandler OnCancel;

        public AddEmployeeViewModel()
        {
            SubmitCommand = new RelayCommand(param => SaveEmployee(), param => true);
            CancelCommand = new RelayCommand(param => Cancel(), param => true);
        }

        public void Reset()
        {
            Name = string.Empty;
            Email = string.Empty;
            Gender = string.Empty;
            Status = string.Empty;
        }

        private void SaveEmployee()
        {
            EmployeeModel employee = new EmployeeModel
            {
                name = Name,
                email = Email,
                gender = Gender,
                status = Status
            };
            OnSubmit?.Invoke(this, employee);
        }

        private void Cancel()
        {
            OnCancel?.Invoke(this, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
