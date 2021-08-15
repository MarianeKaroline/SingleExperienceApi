namespace SingleExperience.Repository.Services.EmployeeServices.Models
{
    public class RegisteredModel
    {
        public string Cpf { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool AccessInventory { get; set; }
        public bool RegisterEmployee { get; set; }
    }
}
