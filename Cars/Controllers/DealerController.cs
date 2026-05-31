using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Cars.Data;
using Cars.Data.Models;

namespace Cars.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DealerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DealerController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Car car)
        {
            var userId = _userManager.GetUserId(User);
            var dealer = _context.Dealers.FirstOrDefault(d => d.UserId == userId);

            if (dealer != null)
            {
                car.DealerId = dealer.Id;
                car.IsSold = false;

                _context.Cars.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return View(car);
        }
    }
}