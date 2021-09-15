using SingleExperience.Repository.Services.EmployeeServices.Models;
using SingleExperience.Repository.Services.ClientServices.Models;
using SingleExperience.Services.UserServices;
using SingleExperience.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using SingleExperience.Domain;
using System.Threading.Tasks;
using System.Linq;

namespace SingleExperience.Services.EmployeeServices
{
    public class EmployeeService : UserService
    {
        protected readonly Context contexts;

        public EmployeeService(Context context) : base(context)
        {
            contexts = context;
        }

        public async Task<List<RegisteredModel>> Get()
        {
            return await contexts.Enjoyer
                     .Where(i => i.Employee == true)
                     .Select(i => new RegisteredModel()
                     {
                         Cpf = i.Cpf,
                         FullName = i.Name,
                         Email = i.Email,
                         AccessInventory = contexts.AccessEmployee.FirstOrDefault(j => j.Cpf == i.Cpf).AccessInventory,
                         RegisterEmployee = contexts.AccessEmployee.FirstOrDefault(j => j.Cpf == i.Cpf).AccessRegister
                     })
                     .ToListAsync();
        }

        public async Task<AccessEmployeeModel> GetAccess(string sessionId)
        {
            return await contexts.AccessEmployee
                .Where(i => i.Cpf == sessionId)
                .Select(i => new AccessEmployeeModel
                {
                    AccessInventory = i.AccessInventory,
                    AccessRegister = i.AccessRegister
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> Register(SignUpModel employee)
        {
            employee.Validator();

            var existEmployee = await context.Enjoyer
                .Where(i => i.Cpf == employee.Cpf && i.Employee == employee.Employee)
                .FirstOrDefaultAsync();

            if (existEmployee == null)
            {
                await SignUp(employee);

                var access = new AccessEmployee()
                {
                    Cpf = employee.Cpf,
                    AccessInventory = employee.AccessInventory,
                    AccessRegister = employee.AccessRegister
                };

                await contexts.AccessEmployee.AddAsync(access);
                await contexts.SaveChangesAsync();
            }

            return existEmployee == null;
        }                   
    }
}
