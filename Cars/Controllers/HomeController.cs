using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cars.Data;

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
            await DatabaseSeeder.SeedAsync(_context);
            var cars = await _context.Cars.ToListAsync();

            return View(cars);
        }
    }
}