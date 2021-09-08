using SingleExperience.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Repository.Services.CartServices.Models
{
    public class PaymentModel
    {
        public PaymentEnum PaymentEnum { get; set; }
        public string Description { get; set; }
    }
}
