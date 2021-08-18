using SingleExperience.Repository.Services.ClientServices.Models;
using SingleExperience.Repository.Services.UserServices.Models;
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

        public UserService(Context context)
        {
            this.context = context;
        }

        public async Task<string> GetIP()
        {
            Itens = new List<ProductCart>();
            var host = await Dns.GetHostEntryAsync(Dns.GetHostName());
            string session = "";

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    session = ip.ToString().Replace(".", "");
                }
            }

            SessionId = session;
            return session;
        }

        public User GetUser()
        {
            return context.Enjoyer
                .FirstOrDefault(i => i.Cpf == SessionId);
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
                SessionId = user.Cpf;

            return user;
        }

        public async Task<string> SignOut()
        {
            return await GetIP();
        }
    }
}
