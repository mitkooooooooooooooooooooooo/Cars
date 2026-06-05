using Microsoft.EntityFrameworkCore;
using Cars.Data.Models;

namespace Cars.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Sedan" },
                    new Category { Name = "SUV" },
                    new Category { Name = "Sports Car" },
                    new Category { Name = "Electric" }
                };
                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            if (!context.Dealers.Any())
            {
                var defaultDealer = new Dealer
                {
                    PhoneNumber = "+1-555-0199",
                    UserId = null
                };
                await context.Dealers.AddAsync(defaultDealer);
                await context.SaveChangesAsync();
            }

            // Inventory 
            if (!context.Cars.Any())
            {
                var sportsCat = await context.Categories.FirstAsync(c => c.Name == "Sports Car");
                var electricCat = await context.Categories.FirstAsync(c => c.Name == "Electric");
                var dealerId = (await context.Dealers.FirstAsync()).Id;

                var sampleCars = new List<Car>
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
                        CategoryId = sportsCat.Id,
                        DealerId = dealerId
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
                        CategoryId = electricCat.Id,
                        DealerId = dealerId
                    },
                    new Car
                    {
                        Make = "Lamborghini",
                        Model = "Huracan EVO Spyder",
                        Description = "Naturally aspirated V10 beast with cutting-edge vehicle dynamics control.",
                        Price = 200000.00m,
                        Year = 2023,
                        IsSold = false,
                        ImageUrl = "https://www.marshallgoldmanbh.com/imagetag/3996/4/l/Used-2020-Lamborghini-Huracan-EVO-Spyder-1728921891.jpg",
                        CategoryId = sportsCat.Id, // Perfectly categorized under Sports Car!
                        DealerId = dealerId
                    }
                };

                await context.Cars.AddRangeAsync(sampleCars);
                await context.SaveChangesAsync();
            }
        }
    }
}