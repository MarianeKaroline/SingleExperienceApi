using SingleExperience.Domain.Enums;

namespace SingleExperience.Repository.Services.CartServices.Models
{
    public class BuyModel
    {
        public PaymentEnum Method { get; set; }
        public string Confirmation { get; set; }
        public int CreditCardId { get; set; }
        public int AddressId { get; set; }
    }
}
