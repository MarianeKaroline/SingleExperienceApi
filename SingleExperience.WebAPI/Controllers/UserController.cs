using SingleExperience.Repository.Services.UserServices.Models;
using SingleExperience.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Options;
using SingleExperience.Domain;
using Microsoft.AspNetCore.Authorization;

namespace SingleExperience.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        protected readonly UserService user;
        private readonly AppSettings appSettings;

        public UserController(UserService user, IOptions<AppSettings> appSettings) 
        {
            this.user = user; 
            this.appSettings = appSettings.Value;
        }    

        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<UserModel> SignIn([FromBody] SignInModel signIn)
        {
            var userModel = await user.SignIn(signIn);
            var now = DateTime.UtcNow;
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes(appSettings.SecretKey);
            var tokenDiscription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, userModel.Email)
                }),
                Expires = now.AddDays(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDiscription);
            userModel.Token = tokenHandler.WriteToken(token);
            userModel.TokenExpires = now.AddDays(4);

            return userModel;
        }

        [HttpGet("signout")]
        public async Task<string> SignOut()
        {
            return await user.SignOut();
        }
    }
}
