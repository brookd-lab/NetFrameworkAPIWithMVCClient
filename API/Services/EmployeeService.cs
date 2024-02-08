using API.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using API.Data;

namespace API.Services
{
    public class EmployeeService
    {
        private readonly ApplicationDbContext _context;
        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            var employees = await _context.Employee.ToListAsync();
            return employees;
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            return employee;
        }

        public async Task<Employee> AddEmployee([FromBody] Employee employee)
        {
            _context.Employee.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateEmployee([FromBody] Employee employee)
        {
            var existingEmployee = await _context.Employee.AsNoTracking().FirstOrDefaultAsync(e => e.Id == employee.Id);
            if (existingEmployee == null) return null;

            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> RemoveEmployeeById(int id)
        {
            var employee = _context.Employee.Find(id);
            if (employee == null) return null;

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return employee;
        }
    }
}