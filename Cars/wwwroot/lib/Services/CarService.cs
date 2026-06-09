using Cars.Data;
using Cars.Data.Models;
using Cars.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Cars.Services
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext _context;

        public CarService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Car>> GetAllCarsAsync() =>
            await _context.Cars.ToListAsync();

        public async Task<Car?> GetCarByIdAsync(int id) =>
            await _context.Cars.FindAsync(id);

        public async Task<List<Category>> GetAllCategoriesAsync() =>
            await _context.Categories.ToListAsync();

        public async Task<List<Dealer>> GetAllDealersAsync() =>
            await _context.Dealers.ToListAsync();

        public async Task<bool> CreateCarAsDealerAsync(Car car, string userId)
        {
            var dealer = await _context.Dealers.FirstOrDefaultAsync(d => d.UserId == userId);
            if (dealer == null) return false;

            car.DealerId = dealer.Id;
            car.IsSold = false;

            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task CreateCarAsync(Car car)
        {
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCarAsync(Car car)
        {
            _context.Update(car);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteCarAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return false;

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BuyCarAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null || car.IsSold) return false;

            car.IsSold = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleSoldStatusAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return false;

            car.IsSold = !car.IsSold;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CarExistsAsync(int id) =>
            await _context.Cars.AnyAsync(e => e.Id == id);
    }
}