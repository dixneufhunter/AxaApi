using Microsoft.EntityFrameworkCore;
using SurveyApi.Models;

namespace SurveyApi.Data
{
    //public class AppDbContext : IdentityDbContext<IdentityUser>
    //{
    //    public AppDbContext(DbContextOptions options) : base(options)
    //    {
    //    }
    //}
    public class AppDbContext_Product : DbContext
    {
        public AppDbContext_Product(DbContextOptions<AppDbContext_Product> options) : base(options)
        {
        }


        public DbSet<Product> Product { get; set; }

        //public DbSet<Food> Foods { get; set; }

        //public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Product>()
                .HasKey(c => c.ID);

        }





    }
}
