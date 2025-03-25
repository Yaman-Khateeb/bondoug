using DAL;
using System;
using System.Collections.Generic;

public class EventRides
{
    // Method to create a new EventRide
    public static bool CreateNewEventRide(EventRideDTO eventRide)
    {
        try
        {
            // Call the DAL method to create the event ride
            return EventRidesData.CreateNewEventRide(eventRide);
        }
        catch (Exception ex)
        {
            // Handle exception (you can log it)

            return false;
        }
    }

    // Method to update an existing EventRide
    public static bool UpdateEventRide(EventRideDTO eventRide)
    {
        try
        {
            // Call the DAL method to update the event ride
            return EventRidesData.UpdateEventRide(eventRide);
        }
        catch (Exception ex)
        {
            // Handle exception (you can log it)

            return false;
        }
    }

    // Method to get an EventRide by EventID and DriverID
    public static EventRideDTO GetEventRideByID(int eventRideID)
    {
        try
        {
            // Call the DAL method to get the event ride by EventID and DriverID
            return EventRidesData.GetEventRideByID(eventRideID);
        }
        catch (Exception ex)
        {
            // Handle exception (you can log it)

            return null;
        }
    }

    // Method to get a list of EventRides by EventID and DriverID
    public static List<EventRideDTO> GetEventRidesByEventIDAndDriverID(int eventID, int driverID)
    {
        try
        {
            // Call the DAL method to get the list of event rides
            return EventRidesData.GetEventRidesByEventIDAndDriverID(eventID, driverID);
        }
        catch (Exception ex)
        {
            // Handle exception (you can log it)

            return new List<EventRideDTO>(); // Return an empty list in case of error
        }
    }

    public static List<PassengerDTO> GetAllPassengersForEvent(int eventID)
    {
        return EventRidesData.GetAllPassengersForEvent(eventID);
    }

    public static List<PassengerDTO> GetPassengersForEventWithNoCarAssigned(int eventID)
    {
        return EventRidesData.GetPassengersForEventWithNoCarAssigned(eventID);
    }

    public static List<DriverDTO> GetAllDriversForEvent(int eventID)
    {
        return EventRidesData.GetAllDriversForEvent(eventID);
    }

    public static List<DriverDTO> GetDriversWithAvailableCapacityForEvent(int eventID)
    {
        return EventRidesData.GetDriversWithAvailableCapacityForEvent(eventID);
    }
}
