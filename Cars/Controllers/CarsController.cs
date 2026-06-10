using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cars.Data.Models;
using Cars.Services.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace Cars.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Buy(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null) return NotFound();

            var success = await _carService.BuyCarAsync(id);
            if (!success)
            {
                TempData["Error"] = "This car has already been sold!";
                return RedirectToAction("Index", "Home");
            }

            TempData["Message"] = $"Congratulations! You have successfully purchased the {car.Make} {car.Model}!";
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> ToggleSoldStatus(int id)
        {
            var success = await _carService.ToggleSoldStatusAsync(id);
            if (!success) return NotFound();

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _carService.GetAllCategoriesAsync();
            ViewBag.Dealers = await _carService.GetAllDealersAsync();
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Create(Car car)
        {
            ModelState.Remove("Category");
            ModelState.Remove("Dealer");

            if (ModelState.IsValid)
            {
                await _carService.CreateCarAsync(car);
                TempData["Message"] = $"Successfully added the {car.Make} {car.Model} to the showroom grid!";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Categories = await _carService.GetAllCategoriesAsync();
            ViewBag.Dealers = await _carService.GetAllDealersAsync();
            return View(car);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null) return NotFound();

            await _carService.DeleteCarAsync(id);
            TempData["Message"] = $"Successfully removed the {car.Make} {car.Model} from the showroom database.";
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null) return NotFound();

            ViewBag.Categories = await _carService.GetAllCategoriesAsync();
            ViewBag.Dealers = await _carService.GetAllDealersAsync();
            return View(car);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Car car)
        {
            if (id != car.Id) return NotFound();

            ModelState.Remove("Category");
            ModelState.Remove("Dealer");

            if (ModelState.IsValid)
            {
                try
                {
                    await _carService.UpdateCarAsync(car);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _carService.CarExistsAsync(car.Id)) return NotFound();
                    throw;
                }

                TempData["Message"] = $"Successfully updated specifications for the {car.Make} {car.Model}!";dsdsdsdsd
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Categories = await _carService.GetAllCategoriesAsync();
            ViewBag.Dealers = await _carService.GetAllDealersAsync();
            return View(car);
        }
}