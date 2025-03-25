using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DAL
{
    using System;

    public class EventRegistrationBasicDTO
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int EventID { get; set; }
        public int ComingStatus { get; set; }  // 0 = Not Coming, 1 = Coming, 2 = Needs Ride, 3 = Providing Ride
        public int? CarCapacity { get; set; }  // Only for those providing rides

        public double Longitude { get; set; }
        public double Latitude { get; set; }
        //public DateTime RegisteredAt { get; set; }
    }

    public class EventRegistrationDTO : EventRegistrationBasicDTO
        {
        public UserBasicDTO User { get; set; }
        public EventRegistrationDTO(int id, int userID, int eventID, int comingstatus, int? carCapacity, double longitude, double latitude)
        {
            User = UserData.GetUserByID(userID);
            ID = id;
            UserID = userID;
            EventID = eventID;
            ComingStatus = comingstatus;
            CarCapacity = carCapacity;
            Longitude = longitude;
            Latitude = latitude;

        }
        }
    

    public class EventRegistrationsData
    {
        private static readonly string connectionString = DataAccessSettings.ConnectionString;

        /// <summary>
        /// Inserts a new event registration and returns the inserted ID.
        /// </summary>
        public static int CreateEventRegistration(EventRegistrationBasicDTO eventRegDTO)
        {
            int insertedID = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_CreateEventRegistration", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", eventRegDTO.UserID);
                    cmd.Parameters.AddWithValue("@EventID", eventRegDTO.EventID);
                    cmd.Parameters.AddWithValue("@ComingStatus", eventRegDTO.ComingStatus);
                    cmd.Parameters.AddWithValue("@CarCapacity", (object)eventRegDTO.CarCapacity ?? DBNull.Value);
                    
                    cmd.Parameters.AddWithValue("@Longitude", (object)eventRegDTO.Longitude ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Latitude", (object)eventRegDTO.Latitude ?? DBNull.Value);

                    // Output parameter for inserted ID
                    SqlParameter outputParam = new SqlParameter("@InsertedID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    insertedID = Convert.ToInt32(outputParam.Value);
                }
            }
            catch (Exception)
            {
                return -1;
            }

            return insertedID;
        }

        /// <summary>
        /// Updates an existing event registration.
        /// </summary>
        public static bool UpdateEventRegistration(EventRegistrationBasicDTO eventRegDTO)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_UpdateEventRegistration", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RegistrationID", eventRegDTO.ID);
                    cmd.Parameters.AddWithValue("@ComingStatus", eventRegDTO.ComingStatus);
                    cmd.Parameters.AddWithValue("@CarCapacity", (object)eventRegDTO.CarCapacity ?? DBNull.Value);
                    
                    cmd.Parameters.AddWithValue("@Longitude", (object)eventRegDTO.Longitude ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Latitude", (object)eventRegDTO.Latitude ?? DBNull.Value);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes an event registration.
        /// </summary>
        public static bool DeleteEventRegistration(int registrationID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_DeleteEventRegistration", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RegistrationID", registrationID);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Retrieves an event registration by ID.
        /// </summary>
        public static EventRegistrationDTO GetEventRegistrationByID(int registrationID)
        {
            EventRegistrationDTO eventRegDTO = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetEventRegistrationByID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RegistrationID", registrationID);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            eventRegDTO = new EventRegistrationDTO
                            (
                                
                                (int)reader["ID"],
                                (int)reader["UserID"],
                                (int)reader["EventID"],
                                (int)reader["ComingStatus"],
                                reader["CarCapacity"] as int?,                                
                                Convert.ToDouble(reader["Longitude"]),
                                Convert.ToDouble(reader["Latitude"])
                                
                            );
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return eventRegDTO;
        }

        /// <summary>
        /// Retrieves all event registrations for a specific event.
        /// </summary>
        public static List<EventRegistrationDTO> GetEventRegistrationsByEventID(int eventID)
        {
            List<EventRegistrationDTO> eventRegistrations = new List<EventRegistrationDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetEventRegistrationsByEventID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EventID", eventID);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            eventRegistrations.Add(new EventRegistrationDTO
                            (
                                (int)reader["ID"],
                                (int)reader["UserID"],
                                (int)reader["EventID"],
                                (int)reader["ComingStatus"],
                                reader["CarCapacity"] as int?,
                                Convert.ToDouble(reader["Longitude"]),
                                Convert.ToDouble(reader["Latitude"])

                            )
                            )
                            ;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return eventRegistrations;
        }
    }
}
