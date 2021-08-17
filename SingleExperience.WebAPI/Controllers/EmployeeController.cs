using SingleExperience.Repository.Services.EmployeeServices.Models;
using SingleExperience.Repository.Services.ClientServices.Models;
using SingleExperience.Services.EmployeeServices;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SingleExperience.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        protected readonly EmployeeService employee;

        public EmployeeController(EmployeeService employee) => this.employee = employee;

        [HttpGet("employees")]
        public async Task<List<RegisteredModel>> Get()
        {
            return await employee.Get();
        }

        [HttpGet("access")]
        public async Task<AccessEmployeeModel> GetAccess()
        {
            return await employee.GetAccess();
        }

        [HttpPost("signup")]
        public async Task<bool> Signup([FromBody] SignUpModel employeeModel)
        {
            return await employee.Singup(employeeModel);
        }
    }
}
