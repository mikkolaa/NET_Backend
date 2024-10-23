namespace ParkingHouse.Tools
{
    public class ParkingCost
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parkingDuration"></param>
        /// <returns>totalCost</returns>
        public double Calculate(TimeSpan parkingDuration)
        {
            // 50 senttiä per alkava 10 minuuttia
            double firstThreeHoursRate = 0.50;
            // 30 senttiä per alkava 10 minuuttia
            double afterThreeHoursRate = 0.30;
            // 3 tuntia
            TimeSpan firstThreeHoursLimit = TimeSpan.FromHours(3);

            // Laske alkavat 10 minuutin jaksot
            // Pysäköinnin kokonaisminuutit pyöristettynä ylöspäin
            int totalMinutes = (int) Math.Ceiling(parkingDuration.TotalMinutes);
            // Alkavat 10 minuutin jaksot
            int totalTenMinuteIntervals = (int) Math.Ceiling(totalMinutes / 10.0); 

            // Laske alkavat 10 minuutin jaksot ensimmäisille 3 tunnille (180 minuuttia)
            int firstThreeHoursIntervals = Math.Min(totalTenMinuteIntervals, (int) Math.Ceiling(180 / 10.0));

            // Laske alkavat 10 minuutin jaksot yli 3 tunnin ajalta
            int afterThreeHoursIntervals = totalTenMinuteIntervals - firstThreeHoursIntervals;

            // Laske kokonaiskustannukset
            double totalCost = (firstThreeHoursIntervals * firstThreeHoursRate) + (afterThreeHoursIntervals * afterThreeHoursRate);

            return totalCost;            
        }
    }
}
