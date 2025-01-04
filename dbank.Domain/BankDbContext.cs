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
    }
}
