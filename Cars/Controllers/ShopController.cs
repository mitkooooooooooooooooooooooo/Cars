using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cars.Services.Contracts;

namespace Cars.Controllers
{
    [Authorize(Roles = "User")]
    public class ShopController : Controller
    {
        private readonly ICarService _carService;

        public ShopController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpPost]
        public async Task<IActionResult> Buy(int id)
        {
            await _carService.BuyCarAsync(id);
            return RedirectToAction("Index", "Home");
        }
    }
}