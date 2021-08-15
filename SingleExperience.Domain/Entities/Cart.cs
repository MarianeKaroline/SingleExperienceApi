using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SingleExperience.Domain.Entities
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        //FK - Cpf
        public string Cpf { get; set; }
        [ForeignKey(nameof(Cpf))]
        public User User { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
