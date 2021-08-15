namespace SingleExperience.Repository.Services.ProductServices.Models
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
