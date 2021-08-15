using SingleExperience.Domain.Entities;
using System.Collections.Generic;

namespace SingleExperience.Repository.Services.CartServices.Models
{
    public class SessionModel
    {
        public static int CountProduct { get; set; }
        public static string Session { get; set; }
        public static List<ProductCart> Itens { get; set; }
    }
}
