using SingleExperience.Domain.Enums;
using SingleExperience.Services.CartServices.Models;
using System.Collections.Generic;

namespace SingleExperience.Repository.Services.BoughtServices.Models
{
    public class AddBoughtModel
    {
        public List<BuyProductModel> BuyProducts { get; set; }
        public PaymentEnum Payment { get; set; }
        public int CreditCardId { get; set; }
        public string ReferenceCode { get; set; }
        public decimal TotalPrice { get; set; }
        public int AddressId { get; set; }
    }
}
