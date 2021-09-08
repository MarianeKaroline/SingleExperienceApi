using SingleExperience.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Repository.Services.ProductServices.Models
{
    public class CategoriesModel
    {
        public CategoryEnum CategoryEnum { get; set; }
        public string Description { get; set; }
    }
}
