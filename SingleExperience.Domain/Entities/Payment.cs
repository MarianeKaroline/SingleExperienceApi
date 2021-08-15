using SingleExperience.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SingleExperience.Domain.Entities
{
    public class Payment
    {
        [Key]
        [Column("PaymentId")]
        public PaymentEnum PaymentEnum { get; set; }
        public string Description { get; set; }
    }
}
