using SingleExperience.Enums;

namespace SingleExperience.Services.CartServices.Models
{
    public class BuyProductModel
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public StatusProductEnum Status { get; set; }
    }
}
