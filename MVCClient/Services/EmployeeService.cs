using MVCClient.Data;
using MVCClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace MVCClient.Services
{
    public class EmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _client;
        private string _url = WebConfigurationManager.AppSettings["baseUrl"];

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            _client = new HttpClient(handler);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            var employees = JsonConvert.DeserializeObject<List<Employee>>(await _client.GetStringAsync(_url)).ToList();
            return employees;
        }

        public async Task<Employee> GetEmployeeById(int Id)
        {
            var employee = JsonConvert.DeserializeObject<Employee>(await _client.GetStringAsync(_url + "/" + Id));
            return employee;
        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            var data = await GetEmployeeById(employee.Id);
            if (data != null)
            {
                await _client.PutAsJsonAsync<Employee>(_url + "/" + employee.Id, employee);
                return employee;
            }
            return data;
        }

        public async Task<Employee> RemoveEmployee(Employee employee)
        {
            await _client.DeleteAsync(_url + "/" + employee.Id);
            return employee; 
        }

        public async Task<Employee> CreateEmployee(Employee employee)
        {
            await _client.PostAsJsonAsync<Employee>(_url, employee);
            return employee;
        }
    }
}