using Microsoft.EntityFrameworkCore;
using SingleExperience.Domain.Entities;

namespace SingleExperience.Domain
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<User> Enjoyer { get; set; }
        public DbSet<AccessEmployee> AccessEmployee { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<CreditCard> CreditCard { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<ProductCart> ProductCart { get; set; }
        public DbSet<StatusProductCart> StatusProductCart { get; set; }
        public DbSet<Bought> Bought { get; set; }
        public DbSet<ProductBought> ProductBought { get; set; }
        public DbSet<StatusBought> StatusBought { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }

    }
}
