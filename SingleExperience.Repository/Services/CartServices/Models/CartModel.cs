using SingleExperience.Domain.Enums;
using System;
using System.Linq;

namespace SingleExperience.Repository.Services.CartServices.Models
{
    public class CartModel
    {
        public int ProductId { get; set; }
        public string Cpf { get; set; }
        public string Name { get; set; }
        public CategoryEnum CategoryId { get; set; }
        public StatusProductEnum StatusId { get; set; }
        public decimal Price { get; set; }

        public void Validator()
        {
            if (!this.Cpf.All(char.IsDigit) || this.Cpf.Length != 11 || this.Cpf == null)
                throw new Exception("Invalid CPF");
        }
    }
}
