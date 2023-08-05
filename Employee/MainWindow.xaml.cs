using System.Windows;
using EmployeeView = Employee.Views.Employee;


namespace Employee
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.Navigate(new EmployeeView.Employee());
        }
    }
}
