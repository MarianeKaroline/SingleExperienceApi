using SingleExperience.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SingleExperience.Domain.Entities
{
    public class StatusProductCart
    {
        [Key]
        [Column("StatusProductId")]
        public StatusProductEnum StatusProductEnum { get; set; }
        public string Description { get; set; }
    }
}
