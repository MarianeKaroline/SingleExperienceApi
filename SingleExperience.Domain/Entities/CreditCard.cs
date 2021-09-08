using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SingleExperience.Domain.Entities
{
    public class CreditCard
    {
        [Key]
        public int CreditCardId { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string ShelfLife { get; set; }
        public string Cvv { get; set; }

        //FK - Cpf
        public string Cpf { get; set; }
        [ForeignKey(nameof(Cpf))]
        public User User { get; set; }
    }
}
