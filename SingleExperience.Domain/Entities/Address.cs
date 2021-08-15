using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SingleExperience.Domain.Entities
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }
        public string PostCode { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        //FK - Cpf
        public string Cpf { get; set; }
        [ForeignKey(nameof(Cpf))]
        public User User { get; set; }
    }
}
