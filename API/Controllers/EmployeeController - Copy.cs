using API.Data;
using API.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace API.Controllers
{
    public class EmployeeController_old : ApiController
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController_old()
        {
            _context = new ApplicationDbContext();
        }

        // GET api/employee
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllEmployees()
        {
            var employees = await _context.Employee.ToListAsync();
            return this.Request.CreateResponse(HttpStatusCode.OK, employees);
        }

        // GET api/employee/5
        [HttpGet]
        public async Task<HttpResponseMessage> GetEmployeeById(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            if (employee == null) return this.Request.CreateResponse(HttpStatusCode.NotFound, employee);
            return this.Request.CreateResponse(HttpStatusCode.OK, employee);
        }

        // POST api/employee
        [HttpPost]
        public async Task<HttpResponseMessage> AddEmployee([FromBody] Employee employee)
        {
            try
            {
                _context.Employee.Add(employee);
                await _context.SaveChangesAsync();
                return this.Request.CreateResponse(HttpStatusCode.Created, employee);
            }
            catch
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, employee);
            }
        }

        // PUT api/employee
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateEmployee([FromBody] Employee employee)
        {
            var existingEmployee = await _context.Employee.AsNoTracking().FirstOrDefaultAsync(e => e.Id == employee.Id);
            if (existingEmployee == null)
                return this.Request.CreateResponse(HttpStatusCode.NotFound, employee);

            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return this.Request.CreateResponse(HttpStatusCode.OK, employee);
        }

        // DELETE api/employee/5
        [HttpDelete]
        public async Task<HttpResponseMessage> RemoveEmployeeById(int id)
        {
            var employee = _context.Employee.Find(id);
            if (employee == null)
                return this.Request.CreateResponse(HttpStatusCode.NotFound, employee);

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return this.Request.CreateResponse(HttpStatusCode.OK, employee);
        }


    }
}
