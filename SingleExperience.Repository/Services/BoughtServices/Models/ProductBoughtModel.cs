namespace SingleExperience.Repository.Services.BoughtServices.Models
{
    public class ProductBoughtModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public int BoughtId { get; set; }
    }
}
