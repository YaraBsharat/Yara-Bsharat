public class Flight
{
    public int FlightId { get; set; }
    public string DepartureCountry { get; set; }
    public string DestinationCountry { get; set; }
    public DateTime DepartureDate { get; set; }
    public string DepartureAirport { get; set; }
    public string ArrivalAirport { get; set; }
    public Dictionary<FlightClass, decimal> Prices { get; set; }

    public void PrintFlightDetails()
    {
    
            Console.WriteLine($"Flight ID: {FlightId}");
            Console.WriteLine($"Departure Country: {DepartureCountry}");
            Console.WriteLine($"Destination Country: {DestinationCountry}");
            Console.WriteLine($"Departure Date: {DepartureDate.ToString("yyyy-MM-dd HH:mm:ss")}");
            Console.WriteLine($"Departure Airport: {DepartureAirport}");
            Console.WriteLine($"Arrival Airport: {ArrivalAirport}");

            foreach (var classPrice in Prices)
            {
                Console.WriteLine($"Class: {classPrice.Key}, Price: {classPrice.Value:C}");
            }

            Console.WriteLine();
      
        
    }
}