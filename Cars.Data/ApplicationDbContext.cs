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

        public async Task SeedShowroomInventoryAsync()
        {

            await this.Database.EnsureCreatedAsync();

            if (await this.Cars.AnyAsync()) return;

            var category = await this.Categories.FirstOrDefaultAsync();
            if (category == null)
            {
                category = new Category { Name = "Supercar" };
                await this.Categories.AddAsync(category);
                await this.SaveChangesAsync();
            }

            var dealer = await this.Dealers.FirstOrDefaultAsync();
            if (dealer == null)
            {
  
                var fallbackUserId = "00000000-0000-0000-0000-000000000000";

                try
                {
                    var rawUser = await this.Database.SqlQueryRaw<string>("SELECT Id FROM AspNetUsers").FirstOrDefaultAsync();
                    if (!string.IsNullOrEmpty(rawUser))
                    {
                        fallbackUserId = rawUser;
                    }
                }
                catch
                {
                }

                dealer = new Dealer
                {
                    PhoneNumber = "555-AUTO",
                    UserId = fallbackUserId
                };

                await this.Dealers.AddAsync(dealer);
                await this.SaveChangesAsync();
            }
            var cars = new List<Car>
            {
                new Car
                {
                    Make = "Porsche",
                    Model = "911 GT3 RS",
                    Description = "Track-focused precision instrument. Naturally aspirated 4.0L flat-six engine.",
                    Price = 223800.00m,
                    Year = 2024,
                    IsSold = false,
                    ImageUrl = "https://images.unsplash.com/photo-1614162692292-7ac56d7f7f1e?auto=format&fit=crop&q=80&w=800",
                    CategoryId = category.Id,
                    DealerId = dealer.Id
                },
                new Car
                {
                    Make = "Tesla",
                    Model = "Model S Plaid",
                    Description = "Tri-motor all-wheel drive setup delivering 1,020 horsepower with neck-snapping acceleration.",
                    Price = 89990.00m,
                    Year = 2024,
                    IsSold = false,
                    ImageUrl = "https://images.unsplash.com/photo-1617788138017-80ad40651399?auto=format&fit=crop&q=80&w=800",
                    CategoryId = category.Id,
                    DealerId = dealer.Id
                },
                 new Car
                {
                    Make = "Lamborghini",
                    Model = "Huracan EVO Spyder",
                    Description = "Tri-motor all-wheel drive setup delivering 1,020 horsepower with neck-snapping acceleration.",
                    Price = 200000.00m,
                    Year = 2023,
                    IsSold = false,
                    ImageUrl = "https://www.marshallgoldmanbh.com/imagetag/3996/4/l/Used-2020-Lamborghini-Huracan-EVO-Spyder-1728921891.jpg",
                    CategoryId = category.Id,
                    DealerId = dealer.Id
                }
            };

            await this.Cars.AddRangeAsync(cars);
            await this.SaveChangesAsync();
        }
    }
}