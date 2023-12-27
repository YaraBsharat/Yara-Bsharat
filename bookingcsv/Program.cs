using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

public class Booking
{
    public int BookingId { get; set; }
    public int FlightId { get; set; }
    public FlightClass SelectedClass { get; set; }
    public string PassengerName { get; set; }
    public DateTime BookingDate { get; set; }
}

public class Program
{
    public static void Main()
    {
        // Sample data for bookings
        var bookings = new List<Booking>
        {
            new Booking
            {
                BookingId = 1,
                FlightId = 1,
                SelectedClass = FlightClass.Economy,
                PassengerName = "John Doe",
                BookingDate = DateTime.Now
            },
            new Booking
            {
                BookingId = 2,
                FlightId = 2,
                SelectedClass = FlightClass.Business,
                PassengerName = "Jane Smith",
                BookingDate = DateTime.Parse("2023-02-20T14:30:00", CultureInfo.InvariantCulture)
            },
            new Booking
            {
                BookingId = 3,
                FlightId = 1,
                SelectedClass = FlightClass.FirstClass,
                PassengerName = "Robert Johnson",
                BookingDate = DateTime.Parse("2023-03-10T10:15:00", CultureInfo.InvariantCulture)
            }
        };

        // File path to save the CSV file
        string csvFilePath = "bookings.csv";

        // Save the bookings to a CSV file
        SaveBookingsToCsv(bookings, csvFilePath);

        Console.WriteLine($"CSV file '{csvFilePath}' created successfully.");
    }

    public static void SaveBookingsToCsv(List<Booking> bookings, string csvFilePath)
    {
        try
        {
            using (var writer = new StreamWriter(csvFilePath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteRecords(bookings);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while saving bookings to CSV file: {ex.Message}");
        }
    }
}