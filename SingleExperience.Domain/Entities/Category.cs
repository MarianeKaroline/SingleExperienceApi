using SingleExperience.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SingleExperience.Domain.Entities
{
    public class Category
    {
        [Key]
        [Column("CategoryId")]
        public CategoryEnum CategoryEnum { get; set; }
        public string Description { get; set; }
    }
}
