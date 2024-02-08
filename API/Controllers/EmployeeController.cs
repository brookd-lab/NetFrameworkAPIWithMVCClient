using API.Data;
using API.Model;
using API.Services;
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
    public class EmployeeController : ApiController
    {
        private readonly EmployeeService _service;

        public EmployeeController()
        {
            _service = new EmployeeService(new ApplicationDbContext());
        }

        // GET api/employee
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllEmployees()
        {
            var employees = await _service.GetAllEmployees();
            return this.Request.CreateResponse(HttpStatusCode.OK, employees);
        }

        // GET api/employee/5
        [HttpGet]
        public async Task<HttpResponseMessage> GetEmployeeById(int id)
        {
            try
            {
                var employee = await _service.GetEmployeeById(id);
                if (employee == null) return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Employee with id {id} Not found");
                return this.Request.CreateResponse(HttpStatusCode.OK, employee);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // POST api/employee
        [HttpPost]
        public async Task<HttpResponseMessage> AddEmployee([FromBody] Employee employee)
        {
            try
            {
                await _service.AddEmployee(employee);
                return this.Request.CreateResponse(HttpStatusCode.Created, employee);
            }
            catch (Exception ex)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // PUT api/employee
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateEmployee([FromBody] Employee employee)
        {

            var existingEmployee = await _service.UpdateEmployee(employee);
            if (existingEmployee == null)
                return this.Request.CreateResponse(HttpStatusCode.NotFound, 
                    $"Employee with Id {employee.Id} not found to update");

            return this.Request.CreateResponse(HttpStatusCode.OK, employee);
        }

        // DELETE api/employee/5
        [HttpDelete]
        public async Task<HttpResponseMessage> RemoveEmployeeById(int id)
        {
            try
            {
                var employee = await _service.RemoveEmployeeById(id);
                if (employee == null)
                    return this.Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        $"Employee with Id = {id}  not found to delete");

                return this.Request.CreateResponse(HttpStatusCode.OK, employee);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
