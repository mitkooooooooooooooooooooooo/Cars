using Cars.Data.Models;

namespace Cars.Services.Contracts
{
    public interface ICarService
    {
        Task<IEnumerable<Car>> GetAllAvailableCarsAsync();
    }
}