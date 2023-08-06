using CsvHelper;
using CsvHelper.Configuration;
using Employee.Models;
using Employee.Services;
using Employee.Views.Employee.AddWindow;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Employee.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<EmployeeModel> _employeeData;
        private readonly IEmployeeService _employeeService;
        private readonly AddEmployeeViewModel _addEmployeeViewModel;
        private AddEmployee _addEmployeeWindow;
        private string _searchText;
        private bool _isPreviousButtonEnabled;
        private bool _isNextButtonEnabled;
        private int _page = 1;
        private bool isLoading;

        public ObservableCollection<EmployeeModel> EmployeeData
        {
            get { return _employeeData; }
            set { _employeeData = value; OnPropertyChanged(); }
        }

        public bool IsPreviousButtonEnabled
        {
            get { return _isPreviousButtonEnabled; }
            set { _isPreviousButtonEnabled = value; OnPropertyChanged(); }
        }
        public bool IsNextButtonEnabled
        {
            get { return _isNextButtonEnabled; }
            set { _isNextButtonEnabled = value; OnPropertyChanged(); }
        }
        public int Page { 
            get { return _page; } 
            set { _page = value; OnPropertyChanged(); } 
        }
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; OnPropertyChanged(); }
        }
        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                OnPropertyChanged();
            }
        }

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand AddEmployeeCommand { get; }
        public ICommand ExportToCsvCommand { get; }

        public EmployeeViewModel(IEmployeeService employeeService, AddEmployeeViewModel addEmployeeViewModel)
        {
            _employeeService = employeeService;
            _addEmployeeViewModel = addEmployeeViewModel;
            _addEmployeeViewModel.OnSubmit += OnEmployeeSubmit;
            _addEmployeeViewModel.OnCancel += OnEmployeeAddCancel;

            NextPageCommand = new RelayCommand(async param => await LoadNextPage(), param => true);
            PreviousPageCommand = new RelayCommand(async param => await LoadPreviousPage(), param => Page > 0);
            AddEmployeeCommand = new RelayCommand(OnAddEmployee);
            ExportToCsvCommand = new RelayCommand(OnExportToCsv);

            EmployeeData = new ObservableCollection<EmployeeModel>();
        }

        public async Task LoadEmployeesAsync()
        {
            await GetData_PopulateGrid(Page);
        }
        private async Task LoadNextPage()
        {
            if (!isLoading)
            {
                isLoading = true;
                try
                {
                    await GetData_PopulateGrid(Page + 1);
                    Page++;
                    IsPreviousButtonEnabled = Page > 1;
                }
                finally
                {
                    isLoading = false;
                }
            }
        }
        private async Task LoadPreviousPage()
        {
            if (!isLoading)
            {
                isLoading = true;
                try
                {
                    await GetData_PopulateGrid(Page - 1);
                    Page--;
                    IsPreviousButtonEnabled = Page > 1;
                }
                finally
                {
                    isLoading = false;
                }
            }
        }
        private async Task GetData_PopulateGrid(int pageNum)
        {
            string searchQuery = string.IsNullOrEmpty(SearchText) ? "" : $"?q={SearchText}";
            string pageQuery = $"?page={pageNum}";
            List<EmployeeModel> employees = await _employeeService.GetEmployeesAsync(searchQuery, pageQuery);
            IsNextButtonEnabled = employees.Count >= 10;
            EmployeeData.Clear();
            foreach (var employee in employees)
            {
                EmployeeData.Add(employee);
            }
        }


        private void CloseDialog()
        {
            if (_addEmployeeWindow != null)
            {
                _addEmployeeWindow.Close();
                _addEmployeeWindow = null;
            }
        }

        private void OnAddEmployee(object parameter)
        {
            _addEmployeeWindow = new AddEmployee(_addEmployeeViewModel);
            _addEmployeeWindow.ShowDialog();
        }
        private void OnExportToCsv(object parameter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                DefaultExt = ".csv"
            };

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string fileName = saveFileDialog.FileName;

                ObservableCollection<EmployeeModel> employees = EmployeeData;
                var data = employees.Select(emp => new {
                    Id = emp.id,
                    Name = emp.name,
                    Email = emp.email,
                    Gender = emp.gender,
                    Status = emp.status
                });
                using (var writer = new StreamWriter(fileName))
                {
                    using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
                    {
                        csv.WriteRecords(data);
                    }
                }
            }
            
        }


        private async void OnEmployeeSubmit(object sender, EmployeeModel employee)
        {
            AddEditEmployeeModel addEmployee = new AddEditEmployeeModel()
            {
                name = employee.name,
                email = employee.email,
                gender = employee.gender,
                status = employee.status
            };
            EmployeeModel emp_added = await _employeeService.AddEmployeeAsync(addEmployee);
            if (emp_added?.id != 0)
            {
                await LoadEmployeesAsync();
                CloseDialog();
            }
        }
        private void OnEmployeeAddCancel(object sender, EventArgs e)
        {
            CloseDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
