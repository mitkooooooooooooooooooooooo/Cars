using Microsoft.EntityFrameworkCore;
using Cars.Data;
using Cars.Data.Models;
using Cars.Services.Contracts;

namespace Cars.Services
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext dbContext;

        public CarService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Car>> GetAllAvailableCarsAsync()
        {
            return await dbContext.Cars
                .Where(c => !c.IsSold)
                .Include(c => c.Category)
                .ToListAsync();
        }
    }
}