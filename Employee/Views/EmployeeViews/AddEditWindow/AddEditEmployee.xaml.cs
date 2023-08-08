using Employee.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace Employee.Views
{
    public partial class AddEditEmployee : Window
    {
        public AddEditEmployee(AddEditEmployeeViewModel addEditEmployeeViewModel)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = addEditEmployeeViewModel;
        }
    }
}
