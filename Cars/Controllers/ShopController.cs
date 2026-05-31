using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cars.Data;

namespace Cars.Controllers
{
    [Authorize(Roles = "User")]
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Buy(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null && !car.IsSold)
            {
                car.IsSold = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}