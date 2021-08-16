using Microsoft.EntityFrameworkCore;
using SingleExperience.Domain;
using SingleExperience.Domain.Entities;
using SingleExperience.Repository.Services.ClientServices.Models;
using SingleExperience.Repository.Services.EmployeeServices.Models;
using SingleExperience.Services.UserServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingleExperience.Services.EmployeeServices
{
    public class EmployeeService : UserService
    {
        protected readonly Context contexts;

        public EmployeeService(Context context) : base(context)
        {
            contexts = context;
        }


        public async Task<AccessEmployeeModel> GetAccess()
        {
            return await contexts.AccessEmployee
                .Where(i => i.Cpf == SessionId)
                .Select(i => new AccessEmployeeModel
                {
                    AccessInventory = i.AccessInventory,
                    AccessRegister = i.AccessRegister
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<RegisteredModel>> List()
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

        public async Task<bool> SingUp(SignUpModel employee)
        {
            var existEmployee = GetUser();
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
