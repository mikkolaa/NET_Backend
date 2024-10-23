using Microsoft.AspNetCore.Mvc;
using ParkingHouse.Models;
using ParkingHouse.Services;

namespace ParkingHouse.Controllers
{
    /// <summary>
    /// REST API for handling Parking Hall's costs for Cars.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ParkingController : ControllerBase
    {
        private readonly IParkingService _parkingService;

        public ParkingController(IParkingService parkingService)
        {
            _parkingService = parkingService;
        }

        [HttpPost("enter")]
        public async Task<IActionResult> EnterParkingHall([FromBody] Car car)
        {
            var result = await _parkingService.EnterParkingHallAsync(car);
            return Ok(result);
        }

        [HttpPost("park")]
        public async Task<IActionResult> ParkCar([FromBody] ParkingSpot spot)
        {
            var result = await _parkingService.ParkCarAsync(spot);
            return Ok(result);
        }

        [HttpPost("exit")]
        public async Task<IActionResult> ExitParkingHall([FromBody] Car car)
        {
            var result = await _parkingService.ExitParkingHallAsync(car);
            return Ok(result);
        }

        [HttpGet("spots")]
        public async Task<IActionResult> GetParkingSpots()
        {
            var spots = await _parkingService.GetParkingSpotsAsync();
            return Ok(spots);
        }

        [HttpGet("freespots")]
        public async Task<IActionResult> GetFreeParkingSpots()
        {
            var spots = await _parkingService.GetFreeParkingSpotsAsync();
            return Ok(spots);
        }
    }
}