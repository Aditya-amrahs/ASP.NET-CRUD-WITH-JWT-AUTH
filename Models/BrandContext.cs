using Microsoft.EntityFrameworkCore;

namespace CRUDinNETCORE.Models

{
    public class BrandContext : DbContext
    {
        public BrandContext(DbContextOptions<BrandContext> options) : base(options)
        {
            
        }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "system",
                    LastName = "",
                    Username = "system",
                    Password = "system"
                }
                );
        }
    }
}