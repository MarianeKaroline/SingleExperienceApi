using SingleExperience.Domain.Entities;
using SingleExperience.Domain.Enums;

namespace SingleExperience.Repository.Services.ProductServices.Models
{
    public class ProductCategoryModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public bool? Available { get; set; }
        public string Image { get; set; }
    }
}
