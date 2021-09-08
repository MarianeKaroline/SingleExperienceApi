using SingleExperience.Repository.Services.ClientServices.Models;
using SingleExperience.Repository.Services.UserServices.Models;
using SingleExperience.Services.CartServices;
using SingleExperience.Domain.Entities;
using SingleExperience.Domain.Common;
using Microsoft.EntityFrameworkCore;
using SingleExperience.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Linq;
using System.Net;

namespace SingleExperience.Services.UserServices
{
    public class UserService : Session
    {
        protected readonly Context context;
        private CartService cartService;

        public UserService(Context context)
        {
            this.context = context;
            cartService = new CartService(context);
        }

        public User GetUser(string sessionId)
        {
            return context.Enjoyer
                .FirstOrDefault(i => i.Cpf == sessionId);
        }

        public async Task SignUp(SignUpModel user)
        {
            var registerUser = new User()
            {
                Cpf = user.Cpf,
                Name = user.FullName,
                Phone = user.Phone,
                Email = user.Email,
                BirthDate = user.BirthDate,
                Password = user.Password,
                Employee = user.Employee
            };

            await context.Enjoyer.AddAsync(registerUser);
            await context.SaveChangesAsync();
        }

        public async Task<UserModel> SignIn(SignInModel signIn)
        {
            signIn.Validator();

            var user = await context.Enjoyer
                .Where(i => i.Email == signIn.Email && i.Password == signIn.Password)
                .Select(i => new UserModel()
                {
                    Email = i.Email,
                    Cpf = i.Cpf,
                    Password = i.Password,
                    Employee = i.Employee
                })
                .FirstOrDefaultAsync();


            if (user != null)
            {
                if (Itens != null && Itens.Count > 0)
                {
                    cartService.PassProducts(user.Cpf); 
                }
            }

            return user;
        }

        public async Task<string> SignOut()
        {
            return "";
        }
    }
}
