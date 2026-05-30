using Cars.Services.Contracts;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICarService carService;

        public HomeController(ICarService carService)
        {
            this.carService = carService;
        }

        public async Task<IActionResult> Index()
        {
            // gets cars from services
            var cars = await carService.GetAllAvailableCarsAsync();

            // sends visual to html
            return View(cars);
        }
    }
}