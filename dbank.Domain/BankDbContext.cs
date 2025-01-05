using dbank.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace dbank.Domain
{
    public sealed class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
        {
            if(Database.GetPendingMigrations().Any())
            {
                Database.Migrate();
            }
        }      
    
        public DbSet<CustomerEntity> Customers { get; set; }
        
        public DbSet<PaymentEntity> Payments { get; set; }
    }
}

