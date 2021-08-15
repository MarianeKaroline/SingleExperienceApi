using SingleExperience.Domain.Enums;

namespace SingleExperience.Repository.Services.ProductServices.Models
{
    public class CategoryModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public CategoryEnum CategoryId { get; set; }
        public bool? Available { get; set; }
    }
}
