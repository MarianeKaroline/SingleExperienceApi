using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SingleExperience.Domain.Entities
{
    [Table("Enjoyer")]
    public class User
    {
        [Key]
        public string Cpf { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Password { get; set; }
        public bool Employee { get; set; }

        public List<CreditCard> CredictCard { get; set; }
    }
}
