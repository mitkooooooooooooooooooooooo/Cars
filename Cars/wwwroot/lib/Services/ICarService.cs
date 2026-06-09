using Cars.Data.Models;

namespace Cars.Services.Contracts
{
    public interface ICarService
    {
        Task<List<Car>> GetAllCarsAsync();
        Task<Car?> GetCarByIdAsync(int id);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<List<Dealer>> GetAllDealersAsync();
        Task<bool> CreateCarAsDealerAsync(Car car, string userId);
        Task CreateCarAsync(Car car);
        Task UpdateCarAsync(Car car);
        Task<bool> DeleteCarAsync(int id);
        Task<bool> BuyCarAsync(int id);
        Task<bool> ToggleSoldStatusAsync(int id);
        Task<bool> CarExistsAsync(int id);
    }
}