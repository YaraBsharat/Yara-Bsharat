using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;

class Program
{
    static void Main()
    {
        int option;
        // we need to show the user a list to determin which list of actions we should show, as the actions are different from the a regular passenger
        // and a manager, with that being said, itilizing a passcode where the manager enters the passcode then we show the actions related to them, else
        // we should show the passenger what action they shold be able to do

        Console.WriteLine("Welcome to the Flight Booking System!");
        while (true) // Infinite loop to keep the program running
        {
            Console.Write("Enter your role (1.Manager, 2.Passenger), or '3.Exit' to quit: ");
            if (int.TryParse(Console.ReadLine(), out option))
            {

                if (option == 3)
                {
                    break; // Exit the loop and end the program
                }
                else if (option == 2)
                {
                    RunPassengerApp();
                }
                else if (option == 1)
                {
                    RunManagerApp();
                }
                else
                {
                    Console.WriteLine("Invalid role. Please enter 'passenger', 'manager', or 'exit'.");
                }
            }



        }
    }
    static void RunPassengerApp()
    {        
        FlightClass flightClass;
        var manager = new BookingManager();
        Console.WriteLine("Passenger Application");
        manager.BatchFlightUpload("C:/Users/ThinPad/source/repos/Yara-Bsharat/flights.csv");
        int operationOption;

        while (true)
        {
            Console.WriteLine("Please Select An Operation:\n 1.Book a Flight.\n 2.Search for Available Flights\n 3.Cancel a booking\n 4.Modify a booking\n 5.View personal bookings");
            if (int.TryParse(Console.ReadLine(), out operationOption))
            {
                switch (operationOption)
                {
                    case 1:
                        manager.PrintFlightList();
                        Console.WriteLine("Please enter a flight ID (an integer) to book:");
                        if (int.TryParse(Console.ReadLine(), out int flightId))
                        {
                            // Input should be a valid integerg
                            Console.WriteLine("Please enter the flight class (Economy, Business, FirstClass) to book:");
                            if (Enum.TryParse(Console.ReadLine(), true, out flightClass) && Enum.IsDefined(typeof(FlightClass), flightClass))
                            {
                                // Input should be a valid FlightClass enum value
                                Console.WriteLine("Please enter passenger name to book for:");
                                var passengerName = Console.ReadLine();

                                manager.BookFlight(flightId, flightClass, passengerName);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input for flight class. Please enter a valid class.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input for flight ID. Please enter a valid integer for the flight ID.");
                        }
                        break;
                    case 2:
                        Console.WriteLine("Search for Available Flights:");
                        
                            Console.Write("Enter Maximum Price you searching for: ");
                        if (decimal.TryParse(Console.ReadLine(), out var maxPrice)) {
                            Console.Write("Enter Departure Country: ");
                            var departureCountry = Console.ReadLine();
                            Console.Write("Enter Destination Country: ");
                            var destinationCountry = Console.ReadLine();
                            Console.Write("Enter Departure Airport: ");
                            var departureAirport = Console.ReadLine();
                            Console.Write("Enter Arrival Airport: ");
                            var arrivalAirport = Console.ReadLine();

                            Console.WriteLine("Please enter the flight class (Economy, Business, FirstClass) to book:");
                            if (Enum.TryParse(Console.ReadLine(), true, out flightClass) && Enum.IsDefined(typeof(FlightClass), flightClass))
                            {
                                List<Flight> availableFlights = manager.SearchFlights(maxPrice, departureCountry, destinationCountry, null, departureAirport, arrivalAirport, flightClass);

                                if (availableFlights.Count > 0)
                                {
                                    Console.WriteLine("Available Flights:");
                                    foreach (var flight in availableFlights)
                                    {
                                        flight.PrintFlightDetails();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No available flights matching the criteria.");
                                }

                            }
                            else
                            {
                                Console.WriteLine("Invalid input for flight class. Please enter a valid class.");
                            }
                        }                       
                        break;
                    case 3:
                         Console.Write("Enter the Booking ID to cancel: ");

                        if (int.TryParse(Console.ReadLine(), out var bookingId))
                        {
                            manager.CancelBooking(bookingId);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Booking ID. Please enter a valid numeric ID.");
                        }
                        break;
                    case 4:
                        manager.PrintBookingsList();
                        Console.Write("Enter the Booking ID to modify: ");
                   
                        if (int.TryParse(Console.ReadLine(), out var bookingIdToBeModifyed))
                        {
                            Booking existingBooking = manager.GetBookingById(bookingIdToBeModifyed);

                            if (existingBooking != null)
                            {
                                Console.WriteLine($"Current Booking Details:");
                                Console.WriteLine($"Booking ID: {existingBooking.BookingId}");
                                Console.WriteLine($"Passenger Name: {existingBooking.PassengerName}");
                                manager.GetFlightById(existingBooking.FlightId).PrintFlightDetails();
                                
                                Console.WriteLine("Check Out Flights Data To Update Booking If needed");
                                manager.PrintFlightList();
                                Console.WriteLine("Enter new details:");

                                Console.Write("Enter new Flight ID: ");
                                if (int.TryParse(Console.ReadLine(), out int flightIdToBeModified))
                                {
                                    Console.Write("Enter new Flight Class (Economy, Business, First Class): ");
                                    if (Enum.TryParse(Console.ReadLine(), true, out flightClass) && Enum.IsDefined(typeof(FlightClass), flightClass))
                                    {
                                        Console.Write("Enter new Passenger Name: ");
                                        string newPassengerName = Console.ReadLine();

                                        // Create a new Booking object with updated details
                                        Booking updatedBooking = new Booking
                                        {
                                            BookingId = existingBooking.BookingId,
                                            FlightId = flightIdToBeModified,
                                            SelectedClass = flightClass,
                                            PassengerName = newPassengerName,
                                        };

                                        bool isModified = manager.ModifyBooking(bookingIdToBeModifyed, updatedBooking);

                                        if (isModified)
                                        {
                                            Console.WriteLine("Booking has been modified successfully.");
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Booking with ID {bookingIdToBeModifyed} not found or cannot be modified.");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Booking with ID {bookingIdToBeModifyed} not found.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid Booking ID. Please enter a valid numeric ID.");
                                }
                            }
                                    
                        }      
                        break;
                    case 5:
                        Console.Write("Enter the passenger's name to view personal bookings: ");
                        manager.ViewPersonalBookings(Console.ReadLine());
                        break;
                    case 6:
                        Console.WriteLine("Exiting the program. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please choose a valid operation.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a numeric option.");
            }
        }
    }

    static void RunManagerApp()
    {
        var manager = new BookingManager();
        var passcode = "A@0123";
        Console.WriteLine("Manager Application");
        Console.WriteLine("Manager, Please Enter Passcode:\n");
        for (int i = 0; i < 3; i++)
        {
            if (Console.ReadLine() == passcode)
            {
                int operationOption;
                Console.WriteLine("Please Select An Operation:\n 1.Filter Bookings\n 2.Batch Flight Upload:");
                if (int.TryParse(Console.ReadLine(), out operationOption))
                {
                    switch (operationOption)
                    {
                        case 1:
                            int? flightId = GetNullableIntInput("Flight ID: ");
                            decimal? maxPrice = GetNullableDecimalInput("Maximum Price: ");
                            string departureCountry = GetStringInput("Departure Country: ");
                            string destinationCountry = GetStringInput("Destination Country: ");
                            DateTime? departureDate = GetNullableDateTimeInput("Departure Date (yyyy-MM-dd): ");
                            string departureAirport = GetStringInput("Departure Airport: ");
                            string arrivalAirport = GetStringInput("Arrival Airport: ");
                            string passengerName = GetStringInput("Passenger Name: ");
                            FlightClass? flightClass = GetNullableFlightClassInput("Flight Class (Economy/Business/FirstClass): ");
                            var filteredBookings = manager.FilterBookings(flightId, maxPrice, departureCountry, destinationCountry, departureDate, departureAirport, arrivalAirport, passengerName, flightClass);

                            Console.WriteLine("\nFiltered Bookings:");
                            foreach (var booking in filteredBookings)
                            {
                                booking.PrintBookingDetails();
                                
                            }
                            break;
                        case 2:
                            Console.WriteLine("Please enter a valid CSV file path for flights data");
                            manager.BatchFlightUpload(Console.ReadLine());
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a correct choice");
                }

            }
            else
            {
                Console.WriteLine("Incorrect password, try again.");
            }

        }
    }

    static int? GetNullableIntInput(string prompt)
    {
        Console.Write(prompt);
        string input = Console.ReadLine();
        if (int.TryParse(input, out int result))
            return result;
        return null;
    }

    static decimal? GetNullableDecimalInput(string prompt)
    {
        Console.Write(prompt);
        string input = Console.ReadLine();
        if (decimal.TryParse(input, out decimal result))
            return result;
        return null;
    }

    static string GetStringInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }

    static DateTime? GetNullableDateTimeInput(string prompt)
    {
        Console.Write(prompt);
        string input = Console.ReadLine();
        if (DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            return result;
        return null;
    }

    static FlightClass? GetNullableFlightClassInput(string prompt)
    {
        Console.Write(prompt);
        string input = Console.ReadLine();
        if (Enum.TryParse<FlightClass>(input, true, out FlightClass result))
            return result;
        return null;
    }
}

