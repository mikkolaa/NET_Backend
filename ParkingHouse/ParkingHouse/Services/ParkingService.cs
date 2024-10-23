using ParkingHouse.Models;
using ParkingHouse.Tools;
using System.Collections.Concurrent;

namespace ParkingHouse.Services
{
    /// <summary>
    /// Service class that has methods for handling business logic of this Parking case.
    /// </summary>
    public class ParkingService : IParkingService
    {
        // Thread-safe collection for saving ParkingSpot information 
        private readonly ConcurrentDictionary<int, ParkingSpot> _parkingSpots = 
                new ConcurrentDictionary<int, ParkingSpot>();
        // Parking places in Parkhouse
        private readonly int _totalSpots = 5;

        /// <summary>
        /// Constructor for service that set total count of parking spots.
        /// </summary>
        public ParkingService()
        {
            for (int i = 0; i < _totalSpots; i++)
            {
                _parkingSpots[i] = new ParkingSpot { SpotNumber = i };
            }
        }

        /// <summary>
        /// Method that checks if ParkingHall has free parking spots available.
        /// NOTE: this method DOES'N reserve any Parking spots yet. Just checks that there are spots available.
        /// </summary>
        /// <param name="car"></param>
        /// <returns>Status text</returns>
        public async Task<string> EnterParkingHallAsync(Car car)
        {
            if (_parkingSpots.Values.Any(spot => spot.ParkedCar == null))
            {
                car.EntryTime = DateTime.Now;
                return await Task.FromResult($"Entry successful for {car.LicensePlate} {car.EntryTime.ToString("dd.MM.yyyy HH:mm")} is possible");
            }

            return await Task.FromResult("Parking hall is full");
        }

        /// <summary>
        /// Method that handles parking car into parking spot.
        /// </summary>
        /// <param name="spot"></param>
        /// <returns>Status text</returns>
        public async Task<string> ParkCarAsync(ParkingSpot spot)
        {
            if (_parkingSpots.TryGetValue(spot.SpotNumber, out var existingSpot) && existingSpot.ParkedCar == null)
            {
                existingSpot.ParkedCar = spot.ParkedCar;
                existingSpot.ParkedTime = DateTime.Now;
                _parkingSpots.TryAdd(spot.SpotNumber, spot);

                return await Task.FromResult("Parking successful");
            }
            return await Task.FromResult("Spot is already taken");
        }

        /// <summary>
        /// Method that handles exiting car from parking spot and calculates cost.
        /// </summary>
        /// <param name="car"></param>
        /// <returns>Status text and cost</returns>
        public async Task<string> ExitParkingHallAsync(Car car)
        {
            var spot = _parkingSpots.Values.FirstOrDefault(s => s.ParkedCar?.LicensePlate == car.LicensePlate);
            if (spot != null && spot.ParkedTime != null)
            {
                var parkedDuration = DateTime.Now - spot.ParkedTime.Value;
                var cost = CalculateParkingCost(parkedDuration);
                spot.ParkedCar = null;
                spot.ParkedTime = null;
                return await Task.FromResult($"Exit successful. Parking cost: {cost} euros");
            }
            return await Task.FromResult("Car not found");
        }

        /// <summary>
        /// Method that get information of all parking spots.
        /// </summary>
        /// <returns>Information of all parking spots</returns>
        public async Task<List<ParkingSpot>> GetParkingSpotsAsync()
        {
            return await Task.FromResult(_parkingSpots.Values.ToList());
        }

        /// <summary>
        /// Method that gets free parking spots
        /// </summary>
        /// <returns>List of free parking spots</returns>
        public async Task<List<ParkingSpot>> GetFreeParkingSpotsAsync()
        {
            var freeSpots = _parkingSpots.Values.Where(x => x.ParkedCar == null || 
                                                       x.ParkedCar != null && x.ParkedCar.LicensePlate == null);
            return await Task.FromResult(freeSpots.ToList());
        }
      
        /// <summary>
        /// Method that calculates cost of parking
        /// </summary>
        /// <param name="duration"></param>
        /// <returns>Cost of parking in parking spot</returns>
        private double CalculateParkingCost(TimeSpan duration)
        {
            return new ParkingCost().Calculate(duration);             
        }
    }
}
