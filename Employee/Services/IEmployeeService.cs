using Employee.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Employee.Services
{
    public interface IEmployeeService
    {
        Task<List<EmployeeModel>> GetEmployeesAsync(string searchQuery, string pageQuery);
        Task<EmployeeModel> AddEmployeeAsync(AddEditEmployeeModel employee);
        Task<EmployeeModel> EditEmployeeAsync(AddEditEmployeeModel employee, int empId);
        Task<EmployeeModel> DeleteEmployeeAsync(int employeeId);
    }
}
