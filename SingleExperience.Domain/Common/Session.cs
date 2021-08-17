using SingleExperience.Domain.Entities;
using System.Collections.Generic;

namespace SingleExperience.Domain.Common
{
    public class Session
    {
        public static int CountProduct { get; set; }
        public static string SessionId { get; set; }
        public static List<ProductCart> Itens { get; set; }
    }
}
