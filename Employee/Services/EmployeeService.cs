using Employee.Models;
using Employee.Shared;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Employee.Services
{
    public class EmployeeService : IEmployeeService
    {
        private HttpClient _httpClient;

        public EmployeeService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Constants.ApiKey}");
        }

        public async Task<EmployeeModel> AddEmployeeAsync(AddEditEmployeeModel employee)
        {
            try
            {
                var emp_payload = JsonContent.Create(employee);
                var response = await _httpClient.PostAsync(Constants.AddUser, emp_payload);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<EmployeeModel>();
                }
                else
                {
                    return new EmployeeModel();
                }
            }
            catch (HttpRequestException)
            {
                return new EmployeeModel();
            }
        }

        public async Task<List<EmployeeModel>> GetEmployeesAsync(string searchQuery, string pageQuery)
        {
            try
            {
                string finalQuery = string.IsNullOrEmpty(searchQuery) ? pageQuery : $"{searchQuery}&${pageQuery}"; 
                var response = await _httpClient.GetAsync(Constants.GetUsers + finalQuery);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<EmployeeModel>>();
                }
                else
                {
                    return new List<EmployeeModel>();
                }
            }
            catch (HttpRequestException)
            {
                return new List<EmployeeModel>();
            }
        }
    }
}
