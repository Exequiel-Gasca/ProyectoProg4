using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<Category> Category { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Sale> Sale { get; set; }
        public DbSet<SaleDetail> SaleDetail { get; set; }
        public DbSet<StockMovement> StockMovement { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
