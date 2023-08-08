using Employee.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Employee.ViewModels
{
    public class AddEditEmployeeViewModel : INotifyPropertyChanged
    {
        private EmployeeModel _employee;
        private int? _id;
        private string _name;
        private string _email;
        private string _gender;
        private string _status;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }
        public string Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                OnPropertyChanged();
            }
        }
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }
        public int? Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }


        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        public EmployeeModel Employee
        {
            get { return _employee; }
            set { _employee = value; OnPropertyChanged(); }
        }

        public event EventHandler<EmployeeModel> OnSubmit;
        public event EventHandler OnCancel;

        public AddEditEmployeeViewModel()
        {
            SubmitCommand = new RelayCommand(param => SaveEmployee(), param => true);
            CancelCommand = new RelayCommand(param => Cancel(), param => true);
        }

        public void SetValues(EmployeeModel employee = null)
        {
            Id = employee?.id;
            Name = employee?.name;
            Email = employee?.email;
            Gender = employee?.gender;
            Status = employee?.status;
        }

        private void SaveEmployee()
        {
            EmployeeModel employee = new EmployeeModel()
            {
                id = Id.HasValue ? Id.Value : 0,
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
