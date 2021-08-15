using SingleExperience.Domain;
using SingleExperience.Domain.Entities;
using SingleExperience.Repository.Services.CartServices.Models;
using SingleExperience.Repository.Services.ClientServices.Models;
using SingleExperience.Repository.Services.UserSevices.Models;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace SingleExperience.Services.UserServices
{
    public class UserService : SessionModel
    {
        protected readonly Context context;

        public UserService(Context context)
        {
            this.context = context;
        }

        public string GetIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            string session = "";

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    session = ip.ToString().Replace(".", "");
                }
            }
            return session;
        }

        public UserModel SignIn(SignInModel signIn)
        {
            var client = GetUserEmail(signIn.Email);
            UserModel session = null;

            if (client != null)
            {
                if (client.Password == signIn.Password)
                {
                    session = client;
                }
            }

            return session;
        }

        //Sair
        public string SignOut()
        {
            return GetIP();
        }

        public User GetUser()
        {
            return context.Enjoyer
                .FirstOrDefault(i => i.Cpf == Session);
        }

        public UserModel GetUserEmail(string email)
        {
            return context.Enjoyer
                .Where(i => i.Email == email)
                .Select(i => new UserModel()
                {
                    Email = i.Email,
                    Cpf = i.Cpf,
                    Password = i.Password,
                    Employee = i.Employee
                })
                .FirstOrDefault();
        }

        public void SignUp(SignUpModel enjoyer)
        {
            var user = new User()
            {
                Cpf = enjoyer.Cpf,
                Name = enjoyer.FullName,
                Phone = enjoyer.Phone,
                Email = enjoyer.Email,
                BirthDate = enjoyer.BirthDate,
                Password = enjoyer.Password,
                Employee = enjoyer.Employee
            };

            context.Enjoyer.Add(user);
            context.SaveChanges();
        }
    }
}
