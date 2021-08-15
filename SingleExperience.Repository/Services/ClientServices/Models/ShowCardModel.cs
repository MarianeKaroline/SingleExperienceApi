using System;

namespace SingleExperience.Repository.Services.ClientServices.Models
{
    public class ShowCardModel
    {
        public string CardNumber { get; set; }
        public string Name { get; set; }
        public DateTime ShelfLife { get; set; }
    }
}
