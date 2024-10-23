using ParkingHouse.Services;
using ParkingHouse.Models;
using ParkingHouse.Tools;

namespace Tests
{
    [TestClass]
    public class ParkingServiceTests
    {
        private IParkingService _parkingService = new ParkingService();
        // Parking place count in testing
        private const int ParkingPlaceCount = 5;
        // Parking costs
        private ParkingCost _calculator = new ParkingCost();
        
        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        public async Task TestEnterParkingHall_Success()
        {
            var car = new Car { LicensePlate = "ABC-123" };
            var result = await _parkingService.EnterParkingHallAsync(car);
            Assert.AreEqual("Entry successful", result.Substring(0, 16));
        }

        [TestMethod]
        public async Task TestParkCar_Success()
        {
            var car = new Car { LicensePlate = "ABC-123" };
            var spot = new ParkingSpot { SpotNumber = 0, ParkedCar = car };
            var result = await _parkingService.ParkCarAsync(spot);
            Assert.AreEqual("Parking successful", result);
        }

        [TestMethod]
        public async Task TestExitParkingHall_Success()
        {
            var car = new Car { LicensePlate = "ABC-123" };
            var spot = new ParkingSpot { SpotNumber = 0, ParkedCar = car };
            await _parkingService.ParkCarAsync(spot);
            var result = await _parkingService.ExitParkingHallAsync(car);
            Assert.IsTrue(result.StartsWith("Exit successful"));
        }

        [TestMethod]
        public async Task TestGetParkingSpots()
        {
            var result = await _parkingService.GetParkingSpotsAsync();
            Assert.AreEqual(ParkingPlaceCount, result.Count);
        }

        [TestMethod]
        public async Task TestEnterParkingHall_AllSpotsTaken()
        {
            var cars = new List<Car>
            {
                new Car { LicensePlate = "ABC-123" },
                new Car { LicensePlate = "ABC-234" },
                new Car { LicensePlate = "ABC-345" },
                new Car { LicensePlate = "ABC-456" },
                new Car { LicensePlate = "ABC-567" },
                new Car { LicensePlate = "ABC-678" }
            };

            foreach (var car in cars)
            {
                var spot = new ParkingSpot { SpotNumber = cars.IndexOf(car), ParkedCar = car };
                await _parkingService.ParkCarAsync(spot);
            }

            var newCar = new Car { LicensePlate = "ABC-678" };
            var result = await _parkingService.EnterParkingHallAsync(newCar);

            Assert.AreEqual("Parking hall is full", result);

            // Should not be any free parking spots left
            var freeOnes = await _parkingService.GetFreeParkingSpotsAsync();
            Assert.IsTrue(freeOnes.Count == 0);
        }

        /// <summary>
        ///   Testing what parking time costs.
        ///   "Pysäköinnin hinta on: 50snt/alkava 10min ensimmäisen 3h ajan. 30snt/alkava 10min seuraavilta tunneilta."
        ///   "Eli ensimmäiset 3 tuntia 10min alkava 50 snt ja sen jälkeen lisäksi kolmen tunnin ylittävältä osalta 10 min 
        ///   hinnaltaan 30snt."
        /// </summary>

        [TestMethod]
        public void CalculateParkingCost_UnderThreeHours_ReturnsCorrectCost()
        {
            // 2 h ja 15 min (135 min)
            TimeSpan parkingDuration = new TimeSpan(2, 15, 0);
            // 14  10 min periods * 0.50€
            double expectedCost = 7.00; 
            double actualCost = _calculator.Calculate(parkingDuration);

            Assert.AreEqual(expectedCost, actualCost);
        }

        [TestMethod]
        public void CalculateParkingCost_ExactlyThreeHours_ReturnsCorrectCost()
        {
            // 3 h
            TimeSpan parkingDuration = new TimeSpan(3, 0, 0);
            // 18 10 min periods * 0.50€
            double expectedCost = 9.00; 
            double actualCost = _calculator.Calculate(parkingDuration);

            Assert.AreEqual(expectedCost, actualCost);
        }

        [TestMethod]
        public void CalculateParkingCost_OverThreeHours_ReturnsCorrectCost()
        {
            // 4 h ja 20 min (260 min)
            TimeSpan parkingDuration = new TimeSpan(4, 20, 0);
            // 18 10 min (3h) * 0.50€ + 8 10 min * 0.30€
            double expectedCost = 11.40; 
            double actualCost = _calculator.Calculate(parkingDuration);

            Assert.AreEqual(expectedCost, actualCost);
        }

        [TestMethod]
        public void CalculateParkingCost_ZeroMinutes_ReturnsZeroCost()
        {
            TimeSpan parkingDuration = TimeSpan.Zero;
            // 0 10 min * 0.50€
            double expectedCost = 0.0; 
            double actualCost = _calculator.Calculate(parkingDuration);

            Assert.AreEqual(expectedCost, actualCost);
        }

        [TestMethod]
        public void CalculateParkingCost_NineMinutes_ReturnsMinimumCost()
        {
            // 9 min
            TimeSpan parkingDuration = new TimeSpan(0, 9, 0); 
            // 1 10 min * 0.50€
            double expectedCost = 0.50; 
            double actualCost = _calculator.Calculate(parkingDuration);

            Assert.AreEqual(expectedCost, actualCost);
        }

        [TestMethod]
        public void CalculateParkingCost_ThreeHoursAndOneMinute_ReturnsCorrectCost()
        {
            // 3 h ja 1 min (181 min)
            TimeSpan parkingDuration = new TimeSpan(3, 1, 0);
            // 18 10 min * 0.50€ + 1 10 min * 0.30€
            double expectedCost = 9.30; 
            double actualCost = _calculator.Calculate(parkingDuration);

            Assert.AreEqual(expectedCost, actualCost);
        }
    }
}
