using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Cars.Data.Models;
using Cars.Services.Contracts;

namespace Cars.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DealerController : Controller
    {
        private readonly ICarService _carService;
        private readonly UserManager<IdentityUser> _userManager;

        public DealerController(ICarService carService, UserManager<IdentityUser> userManager)
        {
            _carService = carService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _carService.GetAllCategoriesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Car car)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId)) return Challenge();

            var success = await _carService.CreateCarAsDealerAsync(car, userId);
            if (success)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Categories = await _carService.GetAllCategoriesAsync();
            return View(car);
        }
    }
}