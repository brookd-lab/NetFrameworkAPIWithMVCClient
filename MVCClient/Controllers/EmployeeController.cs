using MVCClient.Data;
using MVCClient.Models;
using MVCClient.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVCClient.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _service;

        public EmployeeController()
        {
            _service = new EmployeeService(new ApplicationDbContext());
        }

        public async Task<ActionResult> Index()
        {
            var employees = await _service.GetEmployees();
            return View(employees);
        }

        public async Task<Employee> GetEmployeeById(int Id)
        {
            var employee = await _service.GetEmployeeById(Id);
            return employee;
        }

        public async Task<ActionResult> Details(int Id)
        {
            var employee = await GetEmployeeById(Id);

            return View(employee);
        }

        public async Task<ActionResult> Update(int Id)
        {
            var employee = await GetEmployeeById(Id);
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(Employee employee)
        {
            var data = await GetEmployeeById(employee.Id);
            if (data != null)
            { 
               await _service.UpdateEmployee(employee);
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int Id)
        {
            var employee = await GetEmployeeById(Id);
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Employee employee)
        {
            await _service.RemoveEmployee(employee);
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            var employee = new Employee();
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Employee employee)
        {
            await _service.CreateEmployee(employee);
            return RedirectToAction("Index");
        }
    }
}