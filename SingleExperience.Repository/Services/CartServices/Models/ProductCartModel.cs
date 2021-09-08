using SingleExperience.Domain.Entities;
using SingleExperience.Domain.Enums;

namespace SingleExperience.Repository.Services.CartServices.Models
{
    public class ProductCartModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public CategoryEnum CategoryId { get; set; }
        public Category Category { get; set; }
        public int Amount { get; set; }
        public StatusProductEnum StatusId { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Extension { get; set; }

    }
}
