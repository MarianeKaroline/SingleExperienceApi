using SingleExperience.Repository.Services.CartServices.Models;
using SingleExperience.Domain.Enums;
using System.Collections.Generic;
using System;

namespace SingleExperience.Repository.Services.BoughtServices.Models
{
    public class AddBoughtModel
    {
        public PaymentEnum PaymentId { get; set; }
        public int CreditCardId { get; set; }
        public int AddressId { get; set; }

        public void Validator()
        { 
            if (this.PaymentId == 0)
                throw new Exception("Payment Id Required");

            if (this.AddressId == 0)
                throw new Exception("Address Id Required");
        }
    }
}
