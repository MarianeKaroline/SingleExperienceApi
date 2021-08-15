using SingleExperience.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SingleExperience.Domain.Entities
{
    public class ProductCart
    {
        [Key]
        public int ProductCartId { get; set; }

        //FK - Product
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        //FK - Cart
        public int CartId { get; set; }
        [ForeignKey(nameof(CartId))]
        public Cart Cart { get; set; }

        public int Amount { get; set; }

        //FK - Status Product Cart
        [Column("StatusProductCartId")]
        public StatusProductEnum StatusProductEnum { get; set; }
        [ForeignKey(nameof(StatusProductEnum))]
        public StatusProductCart StatusProductCart { get; set; }
    }
}
