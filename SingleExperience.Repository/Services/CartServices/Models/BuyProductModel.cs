using SingleExperience.Domain.Enums;

namespace SingleExperience.Repository.Services.CartServices.Models
{
    public class BuyProductModel
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public StatusProductEnum Status { get; set; }
    }
}
