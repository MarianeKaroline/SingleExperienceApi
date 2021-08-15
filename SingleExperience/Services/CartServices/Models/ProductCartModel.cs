using SingleExperience.Enums;

namespace SingleExperience.Services.CartServices.Models
{
    public class ProductCartModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public CategoryEnum CategoryId { get; set; }
        public int Amount { get; set; }
        public StatusProductEnum StatusId { get; set; }
        public decimal Price { get; set; }
    }
}
