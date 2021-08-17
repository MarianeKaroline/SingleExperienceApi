using System.Linq;
using System;

namespace SingleExperience.Repository.Services.ClientServices.Models
{
    public class SignUpModel
    {
        public string Cpf { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Password { get; set; }
        public bool Employee { get; set; }
        public bool AccessInventory { get; set; }
        public bool AccessRegister { get; set; }

        public void Validator()
        {
            if (this.Cpf == null)
                throw new Exception("CPF Required");

            if (!this.Cpf.All(char.IsDigit) || this.Cpf.Length != 11)
                throw new Exception("Invalid CPF");

            if (this.FullName == null)
                throw new Exception("Name Required");

            if (this.Phone == null)
                throw new Exception("Phone Required");

            if (!this.Phone.All(char.IsDigit) || this.Phone.Length != 11)
                throw new Exception("Invalid Phone");

            if (this.Email == null)
                throw new Exception("Email Required");

            if (!this.Email.Contains('@'))
                throw new Exception("Invalid Email");

            if (this.BirthDate == null)
                throw new Exception("BirthDate Required");

            if (this.Password == null)
                throw new Exception("Password Required");
        }
    }
}
