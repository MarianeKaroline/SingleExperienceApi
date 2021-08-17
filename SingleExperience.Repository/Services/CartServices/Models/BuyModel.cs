using SingleExperience.Domain.Enums;
using System.Collections.Generic;

namespace SingleExperience.Repository.Services.CartServices.Models
{
    public class BuyModel
    {
        public PaymentEnum Method { get; set; }
        public string Confirmation { get; set; }
        public List<int> Ids { get; set; }
    }
}
