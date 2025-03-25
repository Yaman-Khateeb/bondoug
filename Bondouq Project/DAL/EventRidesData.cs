using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Microsoft.Data.SqlClient;
namespace DAL
{

    public class EventRideDTO
    {
        public int ID { get; set; }
        public int EventID { get; set; }
        public int DriverID { get; set; }               
        public int PassengerID { get; set; }                
        public int DriverRegistrationID { get; set; }
        public int PassengerRegistrationID { get; set; }
        //public int? AssignedByAdminID { get; set; }                
        //public DateTime CreatedAt { get; set; }

        public EventRideDTO(int id,int eventID, int driverID,
                            int passengerID,
                            int driverRegistrationID, int passengerRegistrationID
                            /*DateTime createdAt*/)
        {
            ID = id;
            this.EventID = eventID;
            DriverID = driverID;
            PassengerID = passengerID;
            DriverRegistrationID = driverRegistrationID;
            PassengerRegistrationID = passengerRegistrationID;
            //AssignedByAdminID = assignedByAdminID;
            //CreatedAt = createdAt;
        }
    }
    public class PassengerDTO
    {
        public int PassengerID { get; set; }
        public int EventRegistrationID { get; set; }
        public int EventID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsAssignedToCar { get; set; }

        public PassengerDTO(int passengerID, int eventRegistrationID, int eventID, string firstName, string lastName, string mobileNumber, double latitude, double longitude, bool isAssignedToCar)
        {
            PassengerID = passengerID;
            EventRegistrationID = eventRegistrationID;
            EventID = eventID;
            FirstName = firstName;
            LastName = lastName;
            MobileNumber = mobileNumber;
            Latitude = latitude;
            Longitude = longitude;
            IsAssignedToCar = isAssignedToCar;
        }
    }

    public class DriverDTO
    {
        public int DriverID { get; set; }
        public int EventRegistrationID { get; set; }
        public int EventID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int CarCapacity { get; set; }
        public int AvailableCapacity { get; set; }

        public DriverDTO(int driverID, int eventRegistrationID, int eventID, string firstName, string lastName,
                         string mobileNumber, double latitude, double longitude, int carCapacity, int availableCapacity)
        {
            DriverID = driverID;
            EventRegistrationID = eventRegistrationID;
            EventID = eventID;
            FirstName = firstName;
            LastName = lastName;
            MobileNumber = mobileNumber;
            Latitude = latitude;
            Longitude = longitude;
            CarCapacity = carCapacity;
            AvailableCapacity = availableCapacity;
        }
    }


    public class EventRidesData
    {
        private static string connectionString  = DataAccessSettings.ConnectionString; // Your connection string
        public static bool CreateNewEventRide(EventRideDTO eventRide)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_CreateEventRides", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure
                        command.Parameters.Add(new SqlParameter("@DriverID", SqlDbType.Int) { Value = eventRide.DriverID });
                        command.Parameters.Add(new SqlParameter("@PassengerID", SqlDbType.Int) { Value = eventRide.PassengerID });
                        command.Parameters.Add(new SqlParameter("@DriverRegistrationID", SqlDbType.Int) { Value = eventRide.DriverRegistrationID });
                        command.Parameters.Add(new SqlParameter("@PassengerRegistrationID", SqlDbType.Int) { Value = eventRide.PassengerRegistrationID });

                        //// Handle optional parameter AssignedByAdminID (nullable)
                        //if (eventRide.AssignedByAdminID.HasValue)
                        //{
                        //    command.Parameters.Add(new SqlParameter("@AssignedByAdminID", SqlDbType.Int) { Value = eventRide.AssignedByAdminID.Value });
                        //}
                        //else
                        //{
                        //    command.Parameters.Add(new SqlParameter("@AssignedByAdminID", SqlDbType.Int) { Value = DBNull.Value });
                        //}

                        // Output parameter for the InsertedID
                        SqlParameter insertedIdParam = new SqlParameter("@InsertedID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(insertedIdParam);

                        // Execute the command
                        command.ExecuteNonQuery();

                        // Retrieve the inserted ID from the output parameter
                        eventRide.ID = (int)insertedIdParam.Value;

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (you can log it or rethrow it)
                
                return false;
            }
        }
        public static bool UpdateEventRide(EventRideDTO eventRide)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_UpdateEventRides", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure
                        command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = eventRide.ID });
                        command.Parameters.Add(new SqlParameter("@DriverID", SqlDbType.Int) { Value = eventRide.DriverID });
                        command.Parameters.Add(new SqlParameter("@PassengerID", SqlDbType.Int) { Value = eventRide.PassengerID });
                        command.Parameters.Add(new SqlParameter("@DriverRegistrationID", SqlDbType.Int) { Value = eventRide.DriverRegistrationID });
                        command.Parameters.Add(new SqlParameter("@PassengerRegistrationID", SqlDbType.Int) { Value = eventRide.PassengerRegistrationID });


                        // Handle optional parameter AssignedByAdminID (nullable)
                        //if (eventRide.AssignedByAdminID.HasValue)
                        //{
                        //    command.Parameters.Add(new SqlParameter("@AssignedByAdminID", SqlDbType.Int) { Value = eventRide.AssignedByAdminID.Value });
                        //}
                        //else
                        //{
                        //    command.Parameters.Add(new SqlParameter("@AssignedByAdminID", SqlDbType.Int) { Value = DBNull.Value });
                        //}

                        // Execute the command
                        int rowsAffected = command.ExecuteNonQuery();

                        // Return true if at least one row was updated, false otherwise
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (you can log it or rethrow it)
                
                return false;
            }
        }
        public static EventRideDTO GetEventRideByID(int eventRideID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_GetEventRideByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure
                        command.Parameters.Add(new SqlParameter("@RideID", SqlDbType.Int) { Value = eventRideID });
                        

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // Check if a result is returned
                            {
                                // Map the result to the EventRideDTO
                                return new EventRideDTO(
                                    reader.GetInt32(reader.GetOrdinal("ID")),
                                    reader.GetInt32(reader.GetOrdinal("EventID")),
                                    reader.GetInt32(reader.GetOrdinal("DriverID")),
                                    reader.GetInt32(reader.GetOrdinal("PassengerID")),
                                    reader.GetInt32(reader.GetOrdinal("PassengerRegistrationID")),
                                    reader.GetInt32(reader.GetOrdinal("DriverRegistrationID"))
                                    //reader.IsDBNull(reader.GetOrdinal("AssignedByAdminID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("AssignedByAdminID")),

                                );
                            }
                            else
                            {
                                return null; // No record found
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (you can log it or rethrow it)
                Console.WriteLine(ex.Message);
                return null;
            }
        }          
        public static List<EventRideDTO> GetEventRidesByEventIDAndDriverID(int eventID, int driverID)
        {
            List<EventRideDTO> eventRidesList = new List<EventRideDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_GetEventRidesByEventIDAndDriverID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure
                        command.Parameters.Add(new SqlParameter("@EventID", SqlDbType.Int) { Value = eventID });
                        command.Parameters.Add(new SqlParameter("@DriverID", SqlDbType.Int) { Value = driverID });

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) // Iterate over all results
                            {
                                // Map each row to EventRideDTO and add it to the list
                                eventRidesList.Add(new EventRideDTO(
                                    reader.GetInt32(reader.GetOrdinal("ID")),
                                    reader.GetInt32(reader.GetOrdinal("EventID")),
                                    reader.GetInt32(reader.GetOrdinal("DriverID")),
                                    reader.GetInt32(reader.GetOrdinal("PassengerID")),                                    
                                    //reader.IsDBNull(reader.GetOrdinal("AssignedByAdminID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("AssignedByAdminID")),
                                    reader.GetInt32(reader.GetOrdinal("PassengerRegistrationID")),
                                    reader.GetInt32(reader.GetOrdinal("DriverRegistrationID"))
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (you can log it or rethrow it)
                
                return null;
            }

            return eventRidesList;
        }

        public static List<EventRideDTO> GetAllEventRideByEventID(int eventID)
        {
            List<EventRideDTO> eventRidesList = new List<EventRideDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_GetEventRidesByEventID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure
                        command.Parameters.Add(new SqlParameter("@EventID", SqlDbType.Int) { Value = eventID });
                        

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) // Iterate over all results
                            {
                                // Map each row to EventRideDTO and add it to the list
                                eventRidesList.Add(new EventRideDTO(
                                    reader.GetInt32(reader.GetOrdinal("ID")),
                                    reader.GetInt32(reader.GetOrdinal("EventID")),
                                    reader.GetInt32(reader.GetOrdinal("DriverID")),
                                    reader.GetInt32(reader.GetOrdinal("PassengerID")),                                    
                                    //reader.IsDBNull(reader.GetOrdinal("AssignedByAdminID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("AssignedByAdminID")),
                                    reader.GetInt32(reader.GetOrdinal("PassengerRegistrationID")),
                                    reader.GetInt32(reader.GetOrdinal("DriverRegistrationID"))
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (you can log it or rethrow it)

                return null;
            }

            return eventRidesList;
        }

        public static List<PassengerDTO> GetAllPassengersForEvent(int eventID)
        {
            List<PassengerDTO> passengers = new List<PassengerDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetAllPassengersForEvent", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EventID", eventID);

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                passengers.Add(new PassengerDTO(
                                    Convert.ToInt32(reader["PassengerID"]),       // PassengerID
                                    Convert.ToInt32(reader["EventRegesterID"]),   // EventRegistrationID
                                    Convert.ToInt32(reader["EventID"]),           // EventID
                                    Convert.ToString(reader["FirstName"]),        // FirstName
                                    Convert.ToString(reader["LastName"]),         // LastName
                                    Convert.ToString(reader["MobileNumber"]),     // MobileNumber
                                    Convert.ToDouble(reader["Latitude"]),         // Latitude
                                    Convert.ToDouble(reader["Longitude"]),        // Longitude

                                    Convert.ToBoolean(reader["IsAssignedToCar"])  // IsAssignedToCar
                                ));
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                // Log error (you may use a logger instead of Console)
                Console.WriteLine($"Error: {ex.Message}");
            }

            return passengers;
        }

        public static List<PassengerDTO> GetPassengersForEventWithNoCarAssigned(int eventID)
        {
            List<PassengerDTO> passengers = new List<PassengerDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetPassengersForEventWithNoCarAssigned", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EventID", eventID);

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                passengers.Add(new PassengerDTO(
                                    Convert.ToInt32(reader["PassengerID"]),       // PassengerID
                                    Convert.ToInt32(reader["EventRegesterID"]),   // EventRegistrationID
                                    Convert.ToInt32(reader["EventID"]),           // EventID
                                    Convert.ToString(reader["FirstName"]),        // FirstName
                                    Convert.ToString(reader["LastName"]),         // LastName
                                    Convert.ToString(reader["MobileNumber"]),     // MobileNumber
                                    Convert.ToDouble(reader["Latitude"]),         // Latitude
                                    Convert.ToDouble(reader["Longitude"]),        // Longitude
                                    false // IsAssignedToCar is always false
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error: {ex.Message}");
            }

            return passengers;
        }
        public static List<DriverDTO> GetAllDriversForEvent(int eventID)
        {
            List<DriverDTO> drivers = new List<DriverDTO>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllDriversForEvent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EventID", eventID);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            drivers.Add(new DriverDTO(
                                Convert.ToInt32(reader["DriverID"]),
                                Convert.ToInt32(reader["EventRegistrationID"]),
                                Convert.ToInt32(reader["EventID"]),
                                Convert.ToString(reader["FirstName"]),
                                Convert.ToString(reader["LastName"]),
                                Convert.ToString(reader["MobileNumber"]),
                                Convert.ToDouble(reader["Latitude"]),
                                Convert.ToDouble(reader["Longitude"]),
                                Convert.ToInt32(reader["CarCapacity"]),
                                Convert.ToInt32(reader["AvailableCapacity"])
                            ));
                        }
                    }
                }
            }
            return drivers;
        }

        public static List<DriverDTO> GetDriversWithAvailableCapacityForEvent(int eventID)
        {
            List<DriverDTO> drivers = new List<DriverDTO>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetDriversWithAvailableCapacityForEvent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EventID", eventID);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            drivers.Add(new DriverDTO(
                                Convert.ToInt32(reader["DriverID"]),
                                Convert.ToInt32(reader["EventRegistrationID"]),
                                Convert.ToInt32(reader["EventID"]),
                                Convert.ToString(reader["FirstName"]),
                                Convert.ToString(reader["LastName"]),
                                Convert.ToString(reader["MobileNumber"]),
                                Convert.ToDouble(reader["Latitude"]),
                                Convert.ToDouble(reader["Longitude"]),
                                Convert.ToInt32(reader["CarCapacity"]),
                                Convert.ToInt32(reader["AvailableCapacity"])
                            ));
                        }
                    }
                }
            }
            return drivers;
        }


    }

}

