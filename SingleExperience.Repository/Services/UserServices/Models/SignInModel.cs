using System;

namespace SingleExperience.Repository.Services.UserServices.Models
{
    public class SignInModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public void Validator()
        {
            if (this.Email == null)
                throw new Exception("Email required");

            if (!this.Email.Contains('@'))
                throw new Exception("Invalid Email");

            if (this.Password == null)
                throw new Exception("Password required");
        }
    }
}
