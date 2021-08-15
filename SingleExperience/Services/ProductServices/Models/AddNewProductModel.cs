using SingleExperience.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Services.ProductServices.Models
{
    public class AddNewProductModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Detail { get; set; }
        public int Amount { get; set; }
        public CategoryEnum CategoryId { get; set; }
        public int Ranking { get; set; }
        public bool Available { get; set; }
        public decimal Rating { get; set; }
    }
}
