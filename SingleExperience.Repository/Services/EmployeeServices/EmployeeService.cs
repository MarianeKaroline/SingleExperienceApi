using SingleExperience.Domain;
using SingleExperience.Domain.Entities;
using SingleExperience.Repository.Services.ClientServices.Models;
using SingleExperience.Repository.Services.EmployeeServices.Models;
using SingleExperience.Services.UserServices;
using System.Collections.Generic;
using System.Linq;

namespace SingleExperience.Services.EmployeeServices
{
    public class EmployeeService : UserService
    {
        protected readonly Context context;

        public EmployeeService(Context context) : base(context)
        {
            this.context = context;
        }


        public AccessEmployee Access(string cpf)
        {
            return context.AccessEmployee
                .Select(i => new AccessEmployee
                {
                    Cpf = i.Cpf,
                    AccessInventory = i.AccessInventory,
                    AccessRegister = i.AccessRegister
                })
                .FirstOrDefault(i => i.Cpf == cpf);
        }


        public bool Register(SignUpModel employee)
        {
            var existEmployee = GetUser();
            if (existEmployee == null)
            {
                SignUp(employee);

                var access = new AccessEmployee()
                {
                    Cpf = employee.Cpf,
                    AccessInventory = employee.AccessInventory,
                    AccessRegister = employee.AccessRegister
                };

                context.AccessEmployee.Add(access);
                context.SaveChanges();
            }

            return existEmployee == null;
        }             

        public List<RegisteredModel> List()
        {
            return context.Enjoyer
                     .Where(i => i.Employee == true)
                     .Select(i => new RegisteredModel()
                     {
                         Cpf = i.Cpf,
                         FullName = i.Name,
                         Email = i.Email,
                         AccessInventory = context.AccessEmployee.FirstOrDefault(j => j.Cpf == i.Cpf).AccessInventory,
                         RegisterEmployee = context.AccessEmployee.FirstOrDefault(j => j.Cpf == i.Cpf).AccessRegister
                     })
                     .ToList();
        }
    }
}
