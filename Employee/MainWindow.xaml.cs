using Employee.Services;
using Employee.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;
using EmployeePage = Employee.Views;


namespace Employee
{
    public partial class MainWindow : Window
    {
        public ICommand CloseCommand { get; private set; }
        public ICommand MinimizeCommand { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //injecting all required dependencies
            EmployeeService employeeService = new EmployeeService();
            AddEditEmployeeViewModel addEmployeeViewModel = new AddEditEmployeeViewModel();
            SearchBarViewModel searchBarViewModel = new SearchBarViewModel();
            EmployeeViewModel employeeViewModel = new EmployeeViewModel(employeeService, 
                addEmployeeViewModel, searchBarViewModel);
            mainFrame.Navigate(new EmployeePage.Employee(employeeViewModel, searchBarViewModel));
        }
    }
}
