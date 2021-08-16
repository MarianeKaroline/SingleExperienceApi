using SingleExperience.Domain.Enums;

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
    }
}
