using Microsoft.AspNetCore.Mvc;
using SingleExperience.Domain.Entities;
using SingleExperience.Repository.Services.ClientServices.Models;
using SingleExperience.Repository.Services.EmployeeServices.Models;
using SingleExperience.Services.EmployeeServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SingleExperience.WebAPI.Controllers
{
    [Route("singleexperience/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        protected readonly EmployeeService employee;

        public EmployeeController(EmployeeService employee) => this.employee = employee;


        // GET: api/<EmployeeController>
        [HttpGet("acess")]
        public async Task<AccessEmployeeModel> GetAccess()
        {
            return await employee.GetAccess();
        }

        // GET: api/<EmployeeController>
        [HttpGet("registered-employee")]
        public async Task<List<RegisteredModel>> List()
        {
            return await employee.List();
        }

        // POST api/<EmployeeController>
        [HttpPost("signup")]
        public async Task<bool> SignUp([FromBody] SignUpModel employeeModel)
        {
            return await employee.SingUp(employeeModel);
        }
    }
}
