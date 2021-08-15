using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SingleExperience.Domain.Entities
{
    public class ProductBought
    {
        [Key]
        public int ProductBoughtId { get; set; }

        //FK -Product
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        public int Amount { get; set; }

        //FK - Bought
        public int BoughtId { get; set; }
        [ForeignKey(nameof(BoughtId))]
        public Bought Bought { get; set; }
    }
}
