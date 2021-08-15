using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Services.ProductServices.Model
{
    public class BestSellingModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool? Available { get; set; }
        public int Ranking { get; set; }
    }
}
