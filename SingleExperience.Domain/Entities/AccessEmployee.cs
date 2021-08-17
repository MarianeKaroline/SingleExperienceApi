using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SingleExperience.Domain.Entities
{
    public class AccessEmployee
    {
        [Key]
        public string Cpf { get; set; }

        // FK
        [ForeignKey(nameof(Cpf))]
        public User User { get; set; }

        public bool AccessInventory { get; set; }
        public bool AccessRegister { get; set; }
    }
}
