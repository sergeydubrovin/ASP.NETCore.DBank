using DBank.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DBank.Domain
{
    public sealed class BankDbContext : IdentityDbContext<UserEntity, IdentityRoleEntity, long>
    {
        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
        {
            if (Database.GetPendingMigrations().Any())
            {
                Database.Migrate();
            }
        }

        public DbSet<CustomerEntity> Customers { get; set; }
        
        public DbSet<CardEntity> Cards { get; set; }

        public DbSet<BalanceEntity> Balances { get; set; }

        public DbSet<TransactionEntity> Transactions { get; set; }

        public DbSet<CashDepositEntity> CashDeposits { get; set; }
        
        public DbSet<CreditEntity> Credits { get; set; }
    }
}
