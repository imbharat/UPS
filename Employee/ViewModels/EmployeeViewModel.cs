using CsvHelper;
using CsvHelper.Configuration;
using Employee.Models;
using Employee.Services;
using Employee.Views;
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
        private readonly AddEditEmployeeViewModel _addEditEmployeeViewModel;
        private readonly SearchBarViewModel _searchBarViewModel;
        private AddEditEmployee _addEditEmployeeWindow;

        private string _searchText;
        private bool _isPreviousButtonEnabled;
        private bool _isNextButtonEnabled;
        private int _page = 1;
        private bool isLoading;
        private bool _showEditDeleteButton = false;
        private EmployeeModel _selectedEmployee;

        public ObservableCollection<EmployeeModel> EmployeeData
        {
            get { return _employeeData; }
            set { _employeeData = value; OnPropertyChanged(); }
        }
        public EmployeeModel SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { _selectedEmployee = value;
                if (value != null)
                    ShowEditDeleteButton = true;
                else
                    ShowEditDeleteButton = false;
                OnPropertyChanged(); 
            }
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
            set { _page = value; IsPreviousButtonEnabled = Page > 1; OnPropertyChanged(); } 
        }
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; OnPropertyChanged(); }
        }
        public bool IsLoading
        {
            get { return isLoading; }
            set { isLoading = value; OnPropertyChanged(); }
        }
        public bool ShowEditDeleteButton
        {
            get { return _showEditDeleteButton; }
            set { _showEditDeleteButton = value; OnPropertyChanged(); }
        }

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand AddEmployeeCommand { get; }
        public ICommand EditEmployeeCommand { get; }
        public ICommand DeleteEmployeeCommand { get; }
        public ICommand ExportToCsvCommand { get; }

        public EmployeeViewModel(IEmployeeService employeeService, 
            AddEditEmployeeViewModel addEditEmployeeViewModel, SearchBarViewModel searchBarViewModel)
        {
            _employeeService = employeeService;
            _addEditEmployeeViewModel = addEditEmployeeViewModel;
            _searchBarViewModel = searchBarViewModel;

            _addEditEmployeeViewModel.OnSubmit += OnEmployeeSubmit;
            _addEditEmployeeViewModel.OnCancel += OnEmployeeAddCancel;

            _searchBarViewModel.OnEmployeeSearch += OnSearch;
            _searchBarViewModel.OnClearSearch += OnClearSearch;

            NextPageCommand = new RelayCommand(async param => await LoadNextPage(), param => true);
            PreviousPageCommand = new RelayCommand(async param => await LoadPreviousPage(), param => Page > 0);
            AddEmployeeCommand = new RelayCommand(OnAddEmployee);
            EditEmployeeCommand = new RelayCommand(OnEditEmployee);
            DeleteEmployeeCommand = new RelayCommand(OnDeleteEmployee);
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
                }
                finally
                {
                    isLoading = false;
                }
            }
        }
        private async Task GetData_PopulateGrid(int pageNum)
        {
            string pageQuery = $"?page={pageNum}";
            string searchQuery = string.IsNullOrEmpty(SearchText) ? "" : SearchText;
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
            if (_addEditEmployeeWindow != null)
            {
                _addEditEmployeeWindow.Close();
                _addEditEmployeeWindow = null;
            }
        }

        private void OnAddEmployee(object parameter)
        {
            _addEditEmployeeViewModel.SetValues();
            _addEditEmployeeWindow = new AddEditEmployee(_addEditEmployeeViewModel);
            _addEditEmployeeWindow.ShowDialog();
        }
        private void OnEditEmployee(object parameter)
        {
            _addEditEmployeeViewModel.SetValues(SelectedEmployee);
            _addEditEmployeeWindow = new AddEditEmployee(_addEditEmployeeViewModel);
            _addEditEmployeeWindow.ShowDialog();
        }
        private async void OnDeleteEmployee(object parameter)
        {
            EmployeeModel emp_deleted = await _employeeService.DeleteEmployeeAsync(SelectedEmployee.id);
            if(emp_deleted?.id != 0)
            {
                await LoadEmployeesAsync();
            }
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

        //callbacks subscriptions
        private async void OnEmployeeSubmit(object sender, EmployeeModel employee)
        {
            bool success = false;
            AddEditEmployeeModel employeePayload = new AddEditEmployeeModel()
            {
                name = employee.name,
                email = employee.email,
                gender = employee.gender,
                status = employee.status
            };
            if (employee?.id != null && employee?.id != 0)
            {
                EmployeeModel emp_edited = await _employeeService.EditEmployeeAsync(employeePayload, employee.id);
                if (emp_edited?.id != 0) success = true;
            }
            else
            {
                EmployeeModel emp_added = await _employeeService.AddEmployeeAsync(employeePayload);
                if (emp_added?.id != 0) success = true;
            }
            if (success)
            {
                await LoadEmployeesAsync();
                CloseDialog();
            }
        }
        private void OnEmployeeAddCancel(object sender, EventArgs e)
        {
            CloseDialog();
        }

        private async void OnSearch(object sender, string searchString)
        {
            SearchText = searchString;
            Page = 1;
            await LoadEmployeesAsync();
        }
        private async void OnClearSearch(object sender, EventArgs e)
        {
            SearchText = string.Empty;
            Page = 1;
            await LoadEmployeesAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
