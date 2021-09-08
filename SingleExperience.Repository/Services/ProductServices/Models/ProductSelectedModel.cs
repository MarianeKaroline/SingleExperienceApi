using SingleExperience.Domain.Entities;

namespace SingleExperience.Repository.Services.ProductServices.Models
{
    public class ProductSelectedModel
    {
        public int ProductId { get; set; }
        public decimal Rating { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public string Detail { get; set; }
        public string Image { get; set; }
    }
}
