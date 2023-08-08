using Employee.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Employee.Views
{
    public partial class Employee : Page
    {
        public Employee(EmployeeViewModel employeeViewModel, SearchBarViewModel searchBarViewModel)
        {
            InitializeComponent();
            DataContext = employeeViewModel;
            Loaded += Employee_Loaded;

            SearchBarControl.DataContext = searchBarViewModel;
        }

        private async void Employee_Loaded(object sender, RoutedEventArgs e)
        {
            var employeeViewModel = (EmployeeViewModel)DataContext;
            await employeeViewModel.LoadEmployeesAsync();
        }
    }
}
