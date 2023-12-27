using System.Diagnostics;

public class Booking
{
    public int BookingId { get; set; }
    public int FlightId { get; set; }
   // public Flight SelectedFlight { get; set; }
    public FlightClass SelectedClass { get; set; }
    public string PassengerName { get; set; }
    public DateTime BookingDate { get; set; }

    public void PrintBookingDetails()
    {
        Console.WriteLine("Booking Details:");
        Console.WriteLine($"Booking ID: {BookingId}");
        Console.WriteLine($"Flight ID: {FlightId}");
        Console.WriteLine($"Selected Class: {SelectedClass}");
        Console.WriteLine($"Passenger Name: {PassengerName}");
        Console.WriteLine($"Booking Date: {BookingDate:yyyy-MM-dd HH:mm:ss}");
    }
}