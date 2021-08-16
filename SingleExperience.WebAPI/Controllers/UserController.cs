using Microsoft.AspNetCore.Mvc;
using SingleExperience.Repository.Services.ClientServices.Models;
using SingleExperience.Repository.Services.UserSevices.Models;
using SingleExperience.Services.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SingleExperience.WebAPI.Controllers
{
    [Route("singleexperience/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        protected readonly UserService user;

        public UserController(UserService user) => this.user = user;


        // GET: singleexperience/user
        [HttpGet]
        public async Task<string> GetIP()
        {
            return await user.GetIP();
        }        

        [HttpGet("signin")]
        public async Task<UserModel> SignIn(SignInModel signIn)
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
