using SingleExperience.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Services.ProductServices.Model
{
    public class CategoryModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public CategoryEnum CategoryId { get; set; }
        public bool? Available { get; set; }
    }
}
