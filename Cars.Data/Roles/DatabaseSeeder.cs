using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Cars.Data.Models;

namespace Cars.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            await context.Database.EnsureCreatedAsync();

            string adminRole = "Admin";
            string userRole = "User";

            if (!await roleManager.RoleExistsAsync(adminRole))
                await roleManager.CreateAsync(new IdentityRole(adminRole));

            if (!await roleManager.RoleExistsAsync(userRole))
                await roleManager.CreateAsync(new IdentityRole(userRole));

            string adminEmail = "admin@dealership.com";
            var defaultAdmin = await userManager.FindByEmailAsync(adminEmail);

            if (defaultAdmin == null)
            {
                defaultAdmin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(defaultAdmin, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultAdmin, adminRole);
                }
            }

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
                    Name = "Apex Motorsports Management",
                    PhoneNumber = "+1-555-0199",
                    UserId = defaultAdmin.Id
                };
                await context.Dealers.AddAsync(defaultDealer);
                await context.SaveChangesAsync();
            }

            if (!context.Cars.Any())
            {
                var sportsCat = context.Categories.First(c => c.Name == "Sports Car");
                var electricCat = context.Categories.First(c => c.Name == "Electric");
                var suvCat = context.Categories.First(c => c.Name == "SUV");
                var dealerId = context.Dealers.First().Id;

                var sampleCars = new List<Car>
                {
                    new Car
                    {
                        Make = "Porsche", Model = "911 GT3 RS",
                        Description = "Track-focused precision instrument. Naturally aspirated 4.0L flat-six engine.",
                        Price = 223800.00m, Year = 2024, IsSold = false,
                        ImageUrl = "https://images.unsplash.com/photo-1614162692292-7ac56d7f7f1e?auto=format&fit=crop&q=80&w=800",
                        CategoryId = sportsCat.Id, DealerId = dealerId
                    },
                    new Car
                    {
                        Make = "Tesla", Model = "Model S Plaid",
                        Description = "Tri-motor all-wheel drive setup delivering 1,020 horsepower.",
                        Price = 89990.00m, Year = 2024, IsSold = false,
                        ImageUrl = "https://images.unsplash.com/photo-1617788138017-80ad40651399?auto=format&fit=crop&q=80&w=800",
                        CategoryId = electricCat.Id, DealerId = dealerId
                    }
                };
                await context.Cars.AddRangeAsync(sampleCars);
                await context.SaveChangesAsync();
            }
        }// aaaaaa
    }
}