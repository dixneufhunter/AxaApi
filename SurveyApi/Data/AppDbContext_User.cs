using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
    public class AppDbContext_User : DbContext
    {
        public AppDbContext_User(DbContextOptions<AppDbContext_User> options) : base(options)
        {
        }


        public DbSet<UserAccount> userAccount { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserAccount>()
                .HasKey(c => c.User_Name);

        }





    }
}
