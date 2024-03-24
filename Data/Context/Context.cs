using Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Data
{
    public class Context : DbContext
    {
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<BasketModel> Baskets { get; set; }
        public DbSet<TransactionModel> Transactions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions
                .Remove<PluralizingTableNameConvention>();
        }
    }
}
