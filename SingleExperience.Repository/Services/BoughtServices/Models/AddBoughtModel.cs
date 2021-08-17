using SingleExperience.Repository.Services.CartServices.Models;
using SingleExperience.Domain.Enums;
using System.Collections.Generic;
using System;

namespace SingleExperience.Repository.Services.BoughtServices.Models
{
    public class AddBoughtModel
    {
        public List<BuyProductModel> BuyProducts { get; set; }
        public PaymentEnum PaymentId { get; set; }
        public int CreditCardId { get; set; }
        public string ReferenceCode { get; set; }
        public decimal TotalPrice { get; set; }
        public int AddressId { get; set; }

        public void Validator()
        {
            if (this.BuyProducts.Count == 0)
                throw new Exception("Products Required");

            if (this.PaymentId == 0)
                throw new Exception("Payment Id Required");

            if (this.CreditCardId == 0)
                throw new Exception("Credit Card Required");

            if (this.TotalPrice == 0)
                throw new Exception("Total Price Required");

            if (this.AddressId == 0)
                throw new Exception("Address Id Required");
        }
    }
}
