﻿using System;

namespace SingleExperience.Repository.Services.UserServices.Models
{
    public class UserModel
    {
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Employee { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpires { get; set; }
    }
}
