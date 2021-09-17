using Microsoft.AspNetCore.Mvc;
using SingleExperience.Repository.Services.EmailServices.Models;
using System.Threading.Tasks;
using SingleExperience.Repository.Services.EmailServices;
using System.Net.Mail;
using System.Net;

namespace SingleExperience.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost]
        public async Task<bool> SendEmail([FromBody] EmailMessageModel emailMessageModel)
        {
            EmailService email = new EmailService();

            return await email.SendEmail(emailMessageModel);
        }
    }
}
