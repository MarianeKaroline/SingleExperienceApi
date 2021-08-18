using SingleExperience.Repository.Services.UserServices.Models;
using SingleExperience.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SingleExperience.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        protected readonly UserService user;

        public UserController(UserService user) => this.user = user;       

        [HttpPost("signin")]
        public async Task<UserModel> SignIn([FromBody] SignInModel signIn)
        {
            return await user.SignIn(signIn);
        }

        [HttpGet("signout")]
        public async Task<string> SignOut()
        {
            return await user.SignOut();
        }
    }
}
