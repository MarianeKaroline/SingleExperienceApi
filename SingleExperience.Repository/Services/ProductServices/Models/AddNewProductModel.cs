using SingleExperience.Domain.Enums;
using System;

namespace SingleExperience.Repository.Services.ProductServices.Models
{
    public class AddNewProductModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Detail { get; set; }
        public int Amount { get; set; }
        public CategoryEnum CategoryId { get; set; }
        public int Ranking { get; set; }
        public bool Available { get; set; }
        public decimal Rating { get; set; }
        public string Image { get; set; }

        public void Validator()
        {
            if (this.Name == null)
                throw new Exception("Name required");

            if (this.Price == 0)
                throw new Exception("Price required");

            if (this.Detail == null)
                throw new Exception("Detail required");

            if (this.Amount == 0)
                throw new Exception("Amount required");

            if (this.CategoryId == 0)
                throw new Exception("Category required");
        }
    }
}
