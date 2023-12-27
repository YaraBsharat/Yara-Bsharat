using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

public class FlightRecord
{
    public int FlightId { get; set; }
    public string DepartureCountry { get; set; }
    public string DestinationCountry { get; set; }
    public DateTime DepartureDate { get; set; }
    public string DepartureAirport { get; set; }
    public string ArrivalAirport { get; set; }
    public decimal EconomyPrice { get; set; }
    public decimal BusinessPrice { get; set; }
    public decimal FirstClassPrice { get; set; }
}

class Program
{
    static void Main()
    {
        // Create a list of FlightRecord objects (sample data)
        var flightRecords = new List<FlightRecord>
        {
            new FlightRecord
            {
                FlightId = 1,
                DepartureCountry = "USA",
                DestinationCountry = "Canada",
                DepartureDate = new DateTime(2023, 12, 31),
                DepartureAirport = "JFK",
                ArrivalAirport = "YYZ",
                EconomyPrice = 200,
                BusinessPrice = 400,
                FirstClassPrice = 600
            },
            new FlightRecord
            {
                FlightId = 2,
                DepartureCountry = "Canada",
                DestinationCountry = "Mexico",
                DepartureDate = new DateTime(2023, 12, 31),
                DepartureAirport = "YYZ",
                ArrivalAirport = "CUN",
                EconomyPrice = 250,
                BusinessPrice = 500,
                FirstClassPrice = 750
            }
            ,
            new FlightRecord
            {
                FlightId = 3,
                DepartureCountry = "China",
                DestinationCountry = "Jaban",
                DepartureDate = new DateTime(2023, 12, 31),
                DepartureAirport = "PEK",
                ArrivalAirport = "TNH",
                EconomyPrice = 250,
                BusinessPrice = 500,
                FirstClassPrice = 750
            },
            new FlightRecord
            {
                FlightId = 4,
                DepartureCountry = "USA",
                DestinationCountry = "Jordan",
                DepartureDate = new DateTime(2023, 12, 31),
                DepartureAirport = "JFK",
                ArrivalAirport = "QAIA",
                EconomyPrice = 300,
                BusinessPrice = 550,
                FirstClassPrice = 800
            }
        };

        // Specify the path for the CSV file
        var csvFilePath = "flights.csv";

        // Serialize the FlightRecord objects to a CSV file
        using (var writer = new StreamWriter(csvFilePath))
        using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            csv.WriteRecords(flightRecords);
        }

        Console.WriteLine($"CSV file '{csvFilePath}' created successfully.");
    }
}
