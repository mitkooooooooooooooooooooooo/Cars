using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cars.Data;
using Cars.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cars.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Buy(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return NotFound();

            if (car.IsSold)
            {
                TempData["Error"] = "This car has already been sold!";
                return RedirectToAction("Index", "Home");
            }

            car.IsSold = true;
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Congratulations! You have successfully purchased the {car.Make} {car.Model}!";
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> ToggleSoldStatus(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return NotFound();

            car.IsSold = !car.IsSold;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Dealers = await _context.Dealers.ToListAsync();
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
                await _context.Cars.AddAsync(car);
                await _context.SaveChangesAsync();

                TempData["Message"] = $"Successfully added the {car.Make} {car.Model} to the showroom grid!";

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Dealers = await _context.Dealers.ToListAsync();
            return View(car);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return NotFound();
            _context.Cars.Remove(car);

            await _context.SaveChangesAsync();

            TempData["Message"] = $"Successfully removed the {car.Make} {car.Model} from the showroom database.";

            return RedirectToAction("Index", "Home");
        }
        // fir edits
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return NotFound();

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Dealers = await _context.Dealers.ToListAsync();
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
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Cars.Any(e => e.Id == car.Id)) return NotFound();
                    throw;
                }

                TempData["Message"] = $"Successfully updated specifications for the {car.Make} {car.Model}!";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Dealers = await _context.Dealers.ToListAsync();
            return View(car);
        }
    }
}