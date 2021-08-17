using System.Linq;
using System;

namespace SingleExperience.Repository.Services.ClientServices.Models
{
    public class AddressModel
    {
        public string Postcode { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Cpf { get; set; }

        public void Validator()
        {
            if (this.Postcode == null)
                throw new Exception("Postcode Required");

            if (!this.Postcode.All(char.IsDigit) || this.Postcode.Length != 8)
                throw new Exception("Invalid Postcode");

            if (this.Street == null)
                throw new Exception("Invalid Street");

            if (this.Number == null)
                throw new Exception("Number Required");

            if (!this.Number.All(char.IsDigit) || this.Number.Length > 5)
                throw new Exception("Invalid Number");

            if (this.City == null)
                throw new Exception("City Required");

            if (this.State == null)
                throw new Exception("State Required");

            if (this.Cpf == null)
                throw new Exception("CPF Required");

            if (!this.Cpf.All(char.IsDigit) || this.Cpf.Length != 11)
                throw new Exception("Invalid CPF");
        }
    }
}
