using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Services.UserSevices.Models
{
    public class UserModel
    {
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Employee { get; set; }
    }
}
