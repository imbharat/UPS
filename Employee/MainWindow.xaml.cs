using Employee.Services;
using Employee.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;
using EmployeePage = Employee.Views.Employee;


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
            CloseCommand = new RelayCommand(CloseWindow);
            MinimizeCommand = new RelayCommand(MinimizeWindow);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var employeeService = new EmployeeService();
            var addEmployeeViewModel = new AddEmployeeViewModel();
            var employeeViewModel = new EmployeeViewModel(employeeService, addEmployeeViewModel);
            mainFrame.Navigate(new EmployeePage.Employee(employeeViewModel));
        }

        private void CloseWindow(object obj)
        {
            Application.Current.Shutdown();
        }

        // Minimize the window
        private void MinimizeWindow(object obj)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
