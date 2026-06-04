using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cars.Data;
using Cars.Data.Models;

namespace Cars.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            await _context.Database.EnsureCreatedAsync();

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "ALTER TABLE Dealers DROP CONSTRAINT IF EXISTS FK_Dealers_AspNetUsers_UserId;");

                await _context.Database.ExecuteSqlRawAsync(
                    "ALTER TABLE Dealers ALTER COLUMN UserId NVARCHAR(450) NULL;");
            }
            catch
            {
            }


            if (!await _context.Cars.AnyAsync())
            {
                if (!await _context.Categories.AnyAsync())
                {
                    await _context.Categories.AddAsync(new Category { Name = "Sports Car" });
                    await _context.SaveChangesAsync();
                }

                if (!await _context.Dealers.AnyAsync())
                {
                    await _context.Dealers.AddAsync(new Dealer
                    {
                        PhoneNumber = "+1-555-0199",
                        UserId = null
                    });
                    await _context.SaveChangesAsync();
                }

                var targetCategory = await _context.Categories.FirstAsync();
                var targetDealer = await _context.Dealers.FirstAsync();

                var premiumStock = new List<Car>
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
                        CategoryId = targetCategory.Id,
                        DealerId = targetDealer.Id
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
                        CategoryId = targetCategory.Id,
                        DealerId = targetDealer.Id
                    }
                };

                await _context.Cars.AddRangeAsync(premiumStock);
                await _context.SaveChangesAsync();
            }

            var cars = await _context.Cars.ToListAsync();
            return View(cars);
        }
    }
}