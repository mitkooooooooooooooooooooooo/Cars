using Microsoft.EntityFrameworkCore;
using Cars.Data.Models;

namespace Cars.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Dealer> Dealers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Dealer>()
                .Ignore(d => d.User);

            modelBuilder.Entity<Dealer>()
                .Property(d => d.UserId)
                .IsRequired(false);
        }
    }
}