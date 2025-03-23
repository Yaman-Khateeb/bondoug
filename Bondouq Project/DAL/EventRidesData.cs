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
        public int EventRegistrationID { get; set; }
        public int? AssignedByAdminID { get; set; }                
        public DateTime CreatedAt { get; set; }

        public EventRideDTO(int id,int eventID, int driverID,
                            int passengerID,
                            int eventRegistrationID, int? assignedByAdminID,
                            DateTime createdAt)
        {
            ID = id;
            this.EventID = eventID;
            DriverID = driverID;
            PassengerID = passengerID;
            EventRegistrationID = eventRegistrationID;
            AssignedByAdminID = assignedByAdminID;
            CreatedAt = createdAt;
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
                        command.Parameters.Add(new SqlParameter("@EventRegistrationID", SqlDbType.Int) { Value = eventRide.EventRegistrationID });

                        // Handle optional parameter AssignedByAdminID (nullable)
                        if (eventRide.AssignedByAdminID.HasValue)
                        {
                            command.Parameters.Add(new SqlParameter("@AssignedByAdminID", SqlDbType.Int) { Value = eventRide.AssignedByAdminID.Value });
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@AssignedByAdminID", SqlDbType.Int) { Value = DBNull.Value });
                        }

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
                        command.Parameters.Add(new SqlParameter("@EventRegistrationID", SqlDbType.Int) { Value = eventRide.EventRegistrationID });

                        // Handle optional parameter AssignedByAdminID (nullable)
                        if (eventRide.AssignedByAdminID.HasValue)
                        {
                            command.Parameters.Add(new SqlParameter("@AssignedByAdminID", SqlDbType.Int) { Value = eventRide.AssignedByAdminID.Value });
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@AssignedByAdminID", SqlDbType.Int) { Value = DBNull.Value });
                        }

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
                                    reader.GetInt32(reader.GetOrdinal("EventRegistrationID")),
                                    reader.IsDBNull(reader.GetOrdinal("AssignedByAdminID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("AssignedByAdminID")),
                                    reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
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
                                    reader.GetInt32(reader.GetOrdinal("EventRegistrationID")),
                                    reader.IsDBNull(reader.GetOrdinal("AssignedByAdminID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("AssignedByAdminID")),
                                    reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
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
    }

}

