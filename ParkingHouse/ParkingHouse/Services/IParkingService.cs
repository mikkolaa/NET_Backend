using ParkingHouse.Models;

namespace ParkingHouse.Services
{
    public interface IParkingService
    {
        Task<string> EnterParkingHallAsync(Car car);
        Task<string> ParkCarAsync(ParkingSpot spot);
        Task<string> ExitParkingHallAsync(Car car);
        Task<List<ParkingSpot>> GetParkingSpotsAsync();
        Task<List<ParkingSpot>> GetFreeParkingSpotsAsync();
    }
}
