namespace ParkingHouse.Models
{
    public class ParkingSpot
    {    
        public int SpotNumber { get; set; }
        public Car? ParkedCar { get; set; }
        public DateTime? ParkedTime { get; set; }
    }
}
