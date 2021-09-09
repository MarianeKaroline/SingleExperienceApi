using System;

namespace SingleExperience.Repository.Services.ClientServices.Models
{
    public class ShowCardModel
    {
        public int CreditCardId { get; set; }
        public string CardNumber { get; set; }
        public string Name { get; set; }
        public string ShelfLife { get; set; }
    }
}
