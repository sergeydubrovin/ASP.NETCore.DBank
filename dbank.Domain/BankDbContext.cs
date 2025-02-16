using DBank.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DBank.Domain
{
    public class BankDbContext : IdentityDbContext<UserEntity, IdentityRoleEntity, long>
    {
        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
        {
           
        }

        public virtual DbSet<CustomerEntity> Customers { get; set; }
        
        public virtual DbSet<CardEntity> Cards { get; set; }

        public virtual DbSet<BalanceEntity> Balances { get; set; }

        public virtual DbSet<TransactionEntity> Transactions { get; set; }

        public virtual DbSet<CashDepositEntity> CashDeposits { get; set; }
        
        public virtual DbSet<CreditEntity> Credits { get; set; }
    }
}
