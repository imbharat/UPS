using Employee.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace Employee.Views.Employee.AddWindow
{
    /// <summary>
    /// Interaction logic for AddEmployee.xaml
    /// </summary>
    public partial class AddEmployee : Window
    {
        private readonly AddEmployeeViewModel _addEmployeeViewModel;
        public AddEmployee(AddEmployeeViewModel addEmployeeViewModel)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _addEmployeeViewModel = addEmployeeViewModel;
            DataContext = _addEmployeeViewModel;
        }

        private void CloseWindow(object sender, CancelEventArgs e)
        {
            _addEmployeeViewModel.Reset();
        }
    }
}
