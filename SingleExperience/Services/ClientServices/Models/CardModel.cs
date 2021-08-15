using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Services.ClientServices.Models
{
    public class CardModel
    {
        public string CardNumber { get; set; }
        public string Name { get; set; }
        public DateTime ShelfLife { get; set; }
        public string CVV { get; set; }
        public string Cpf { get; set; }
    }
}
