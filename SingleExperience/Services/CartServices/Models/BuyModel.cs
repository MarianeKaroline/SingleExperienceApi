using SingleExperience.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Services.CartServices.Models
{
    public class BuyModel
    {
        public string Session { get; set; }
        public PaymentEnum Method { get; set; }
        public string Confirmation { get; set; }
        public int CreditCardId { get; set; }
        public StatusProductEnum Status { get; set; }
        public List<int> Ids { get; set; }
    }
}
