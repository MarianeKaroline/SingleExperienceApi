using System;
using System.Linq;

namespace SingleExperience.Repository.Services.ClientServices.Models
{
    public class CardModel
    {
        public string CardNumber { get; set; }
        public string Name { get; set; }
        public DateTime ShelfLife { get; set; }
        public string Cvv { get; set; }
        public string Cpf { get; set; }

        public void Validator()
        {
            if (this.CardNumber == null)
                throw new Exception("Card Number Required");

            if (!this.CardNumber.All(char.IsDigit) || this.CardNumber.Length != 16)
                throw new Exception("Invalid Card Number");

            if (this.Name == null)
                throw new Exception("Name Required");

            if (this.ShelfLife == null)
                throw new Exception("Shelf Life Required");

            if (this.Cvv == null)
                throw new Exception("CVV Required");

            if (!this.Cvv.All(char.IsDigit) || this.Cvv.Length != 3)
                throw new Exception("Invalid CVV");

            if (this.Cpf == null)
                throw new Exception("CPF Required");

            if (!this.Cpf.All(char.IsDigit) || this.Cpf.Length != 11)
                throw new Exception("Invalid CPF");


        }
    }
}
