using SingleExperience.Domain.Enums;
using System;
using System.Collections.Generic;

namespace SingleExperience.Repository.Services.BoughtServices.Models
{
    public class BoughtModel
    {
        public int BoughtId { get; set; }
        public string ClientName { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Cep { get; set; }        
        public PaymentEnum PaymentMethod { get; set; }        
        public string NumberCard { get; set; }
        public List<ProductBoughtModel> Itens { get; set; }
        public decimal TotalPrice { get; set; }
        public StatusBoughtEnum StatusId { get; set; }
        public DateTime DateBought { get; set; }
    }
}
