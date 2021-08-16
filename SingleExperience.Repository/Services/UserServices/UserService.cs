using SingleExperience.Domain;
using SingleExperience.Domain.Entities;
using SingleExperience.Repository.Common.Domain;
using SingleExperience.Repository.Services.ClientServices.Models;
using SingleExperience.Repository.Services.UserSevices.Models;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

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

        public async Task<UserModel> SignIn(SignInModel signIn)
        {
            var client = GetUserEmail(signIn.Email);
            UserModel user = null;

            if (client != null)
            {
                if (client.Password == signIn.Password)
                    user = client;
            }

            if (user != null)
                SessionId = user.Cpf;

            return user;
        }

        //Sair
        public async Task<string> SignOut()
        {
            return await GetIP();
        }

        public User GetUser()
        {
            return context.Enjoyer
                .FirstOrDefault(i => i.Cpf == SessionId);
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
    }
}
