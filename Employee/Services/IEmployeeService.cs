using Employee.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Services
{
    public interface IEmployeeService
    {
        Task<List<EmployeeModel>> GetEmployeesAsync(string searchQuery, string pageQuery);
        Task<EmployeeModel> AddEmployeeAsync(AddEditEmployeeModel employee);
    }
}
