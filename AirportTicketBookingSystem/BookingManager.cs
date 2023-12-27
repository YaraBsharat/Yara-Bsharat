using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System.Diagnostics.Metrics;

public class BookingManager
{
    private List<Booking> bookings = new List<Booking>();
    private List<Flight> allFlights = new List<Flight>(); // will be using this global variable to store all flights in the system, also will be using it to store new flights after uploading new files to the system

    public void PrintFlightList()
    {
        Console.WriteLine("Flight List:");
        foreach (var flight in allFlights)
        {
            flight.PrintFlightDetails();
        }
    }

    public void PrintBookingsList()
    {
        Console.WriteLine("Booking List:");
        foreach (var booking in bookings)
        {
            booking.PrintBookingDetails();
        }
    }

    public Booking GetBookingById(int bookingId)
    {
        var csvFilePath = "C:/Users/ThinPad/source/repos/Yara-Bsharat/bookings.csv";
        try
        {
            if (File.Exists(csvFilePath))
            {
                using (var reader = new StreamReader(csvFilePath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    var booking = csv.GetRecords<Booking>().ToList();
                    return booking.FirstOrDefault(b => b.BookingId == bookingId);
                }
            }
            else
            {
                Console.WriteLine($"CSV file '{csvFilePath}' not found. Returning null.");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while loading bookings from CSV file: {ex.Message}");
            return null;
        }
    }

    public Flight GetFlightById(int flightIdToBeRetrived)
    {
        Flight existingFlight = allFlights.Find(b => b.FlightId == flightIdToBeRetrived);
        return existingFlight;
    }

    public void BookFlight(int flightId, FlightClass selectedClass, string passengerName)
    {
        {
                Booking booking = new Booking
                {
                    BookingId = bookings.Count + 1,
                    FlightId = flightId, // Store Flight ID
                    SelectedClass = selectedClass,
                    PassengerName = passengerName,
                    BookingDate = DateTime.Now
                };
                bookings.Add(booking);
                Console.WriteLine($"Booking ID: {booking.BookingId} - Flight booked for {passengerName}");
                SaveBookingsToCsv(booking);
        }
       
    }

    public void CancelBooking(int bookingId)
    {
        try
        {
            LoadBookingsFromCsv(); // this will get the latest list of the bookings and saves it into bookings list that is a general var

            var bookingToDelete = bookings.FirstOrDefault(b => b.BookingId == bookingId);

            if (bookingToDelete != null)
            {
                // Remove the booking from the list
                bookings.Remove(bookingToDelete);

                // Save the updated list of bookings back to the CSV file
                SaveBookingsToCsv(bookings);
                Console.WriteLine($"Booking ID {bookingId} deleted from CSV file successfully.");
            }
            else
            {
                Console.WriteLine($"Booking ID {bookingId} not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while deleting booking from CSV file: {ex.Message}");
        }
    }

    public bool ModifyBooking(int bookingId, Booking updatedBooking)
    {
        Booking existingBooking = GetBookingById(bookingId); // find booking by the id

        if (existingBooking != null)
        {
            existingBooking.FlightId = updatedBooking.FlightId;
            existingBooking.SelectedClass = updatedBooking.SelectedClass;
            existingBooking.PassengerName = updatedBooking.PassengerName;
            existingBooking.BookingDate = DateTime.Now;
            SaveBookingsToCsv(existingBooking);
            return true;
        }

        return false;
    }

    public void ViewPersonalBookings(string passengerName)
    {
        LoadBookingsFromCsv();
        var personalBookings = bookings.FindAll(b => b.PassengerName == passengerName);
        if (personalBookings.Count > 0)
        {
            foreach (var booking in personalBookings)
            {
                Console.WriteLine($"Booking ID: {booking.BookingId}");
                GetFlightById(booking.FlightId).PrintFlightDetails();
            }
        }
        else
        {
            Console.WriteLine("No bookings found for the passenger.");
        }
    }

    public List<Flight> BatchFlightUpload(string csvFilePath)
    {
        try
        {
            var validationErrors = new List<string>();
            var flights = new List<Flight>();

            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                var flightRecords = csv.GetRecords<FlightRecord>().ToList();

                foreach (var record in flightRecords)
                {
                    // Validate each field of the FlightRecord
                    if (string.IsNullOrWhiteSpace(record.DepartureCountry))
                        validationErrors.Add($"Flight ID: {record.FlightId} - Departure Country is required.");

                    if (string.IsNullOrWhiteSpace(record.DestinationCountry))
                        validationErrors.Add($"Flight ID: {record.FlightId} - Destination Country is required.");

                    if (record.DepartureDate <= DateTime.Now)
                        validationErrors.Add($"Flight ID: {record.FlightId} - Departure Date must be in the Present Or Future.");

                    if (string.IsNullOrWhiteSpace(record.DepartureAirport))
                        validationErrors.Add($"Flight ID: {record.FlightId} - Departure Airport is required.");

                    if (string.IsNullOrWhiteSpace(record.ArrivalAirport))
                        validationErrors.Add($"Flight ID: {record.FlightId} - Arrival Airport is required.");

                    if (record.EconomyPrice <= 0 || record.BusinessPrice <= 0 || record.FirstClassPrice <= 0)
                        validationErrors.Add($"Flight ID: {record.FlightId} - Prices must be greater than zero.");
                   

                    if (validationErrors.Count == 0) // when no errors add flight to the system allFlights List
                    {
                        var flight = new Flight
                        {
                            FlightId = record.FlightId,
                            DepartureCountry = record.DepartureCountry,
                            DestinationCountry = record.DestinationCountry,
                            DepartureDate = record.DepartureDate,
                            DepartureAirport = record.DepartureAirport,
                            ArrivalAirport = record.ArrivalAirport,
                            Prices = new Dictionary<FlightClass, decimal>
                            {
                                { FlightClass.Economy, record.EconomyPrice },
                                { FlightClass.Business, record.BusinessPrice },
                                { FlightClass.FirstClass, record.FirstClassPrice }
                            }
                        };
                        allFlights.Add(flight);
                    }
                }
               Console.WriteLine($"{flightRecords.Count} flights imported successfully.");
            }

            return flights;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during batch flight upload: {ex.Message}");
            return null;
        }
    }

    public void SaveBookingsToCsv(object data)
    {
        var csvFilePath = "C:/Users/ThinPad/source/repos/Yara-Bsharat/bookings.csv";
        try
        {
            bool fileExists = File.Exists(csvFilePath);

            using (var writer = new StreamWriter(csvFilePath, true))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                if (data is Booking singleBooking)
                {
                    // Save a single booking
                    csv.WriteRecord(singleBooking);
                }
                else if (data is List<Booking> bookingList)
                {
                    // Save a list of bookings
                    csv.WriteRecords(bookingList);
                }
                else
                {
                    throw new ArgumentException("Invalid data type. Expected Booking or List<Booking>.");
                }
                csv.NextRecord();
            }

            Console.WriteLine($"Booking added to CSV file '{csvFilePath}' successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while saving booking to CSV file: {ex.Message}");
        }
    }

    public void LoadBookingsFromCsv()
    {
        var csvFilePath = "C:/Users/ThinPad/source/repos/Yara-Bsharat/bookings.csv";
        try
        {
            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                var loadedBookings = csv.GetRecords<Booking>().ToList();
                bookings.AddRange(loadedBookings);
            }

            Console.WriteLine($"Bookings loaded from CSV file '{csvFilePath}' successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while loading bookings from CSV file: {ex.Message}");
        }
    }

    public List<Flight> SearchFlights(
        decimal maxPrice,
        string departureCountry = null,
        string destinationCountry = null,
        DateTime? departureDate = null,
        string departureAirport = null,
        string arrivalAirport = null,
        FlightClass? flightClass = null)
    {
        var availableFlights = new List<Flight>();

        foreach (var flight in allFlights)
        {
            if (flight.Prices[FlightClass.Economy] <= maxPrice)
            {
                if (departureCountry != null && flight.DepartureCountry != departureCountry)
                    continue;

                if (destinationCountry != null && flight.DestinationCountry != destinationCountry)
                    continue;

                if (departureDate != null && flight.DepartureDate.Date != departureDate.Value.Date)
                    continue;

                if (departureAirport != null && flight.DepartureAirport != departureAirport)
                    continue;

                if (arrivalAirport != null && flight.ArrivalAirport != arrivalAirport)
                    continue;

                if (flightClass != null && flight.Prices.ContainsKey(flightClass.Value))
                    availableFlights.Add(flight);
            }
        }

        return availableFlights;
    }

    public List<Booking> FilterBookings(
    int? flightId = null,
    decimal? maxPrice = null,
    string departureCountry = null,
    string destinationCountry = null,
    DateTime? departureDate = null,
    string departureAirport = null,
    string arrivalAirport = null,
    string passengerName = null,
    FlightClass? flightClass = null)
    {
        LoadBookingsFromCsv();
        var filteredBookings = bookings;

        if (flightId != null)
        {
            filteredBookings = filteredBookings.Where(b => b.FlightId == flightId).ToList();
        }

        if (maxPrice != null)
        {
            filteredBookings = (List<Booking>)filteredBookings.Where(b =>
                allFlights.Any(f => f.FlightId == b.FlightId && f.Prices[b.SelectedClass] <= maxPrice.Value)).ToList();
        }

        if (!string.IsNullOrEmpty(departureCountry))
        {
            filteredBookings = (List<Booking>)filteredBookings.Where(b =>
            allFlights.Any(f => f.FlightId == b.FlightId && f.DepartureCountry == departureCountry)).ToList();
        }

        if (!string.IsNullOrEmpty(destinationCountry))
        {
            filteredBookings = (List<Booking>)filteredBookings.Where(b =>
            allFlights.Any(f => f.FlightId == b.FlightId && f.DestinationCountry == destinationCountry)).ToList();
        }

        if (departureDate != null)
        {
            filteredBookings = (List<Booking>)filteredBookings.Where(b =>
           allFlights.Any(f => f.FlightId == b.FlightId && f.DepartureDate.Date == departureDate.Value.Date)).ToList();
        }

        if (!string.IsNullOrEmpty(departureAirport))
        {
            filteredBookings = (List<Booking>)filteredBookings.Where(b =>
            allFlights.Any(f => f.FlightId == b.FlightId && f.DepartureAirport == departureAirport)).ToList();
        }

        if (!string.IsNullOrEmpty(arrivalAirport))
        {
            filteredBookings = (List<Booking>)filteredBookings.Where(b => allFlights.Any(f => f.FlightId == b.FlightId && f.ArrivalAirport == arrivalAirport)).ToList();
        }

        if (!string.IsNullOrEmpty(passengerName))
        {
            filteredBookings = filteredBookings.Where(b => b.PassengerName == passengerName).ToList();
        }

        if (flightClass != null)
        {
            filteredBookings = filteredBookings.Where(b => b.SelectedClass == flightClass).ToList();
        }

        return filteredBookings;
    }

}