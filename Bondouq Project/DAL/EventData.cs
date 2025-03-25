using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
namespace DAL
{

    public class EventBasicDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int MosqueID { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //public int CreatedByAdminID { get; set; }

        public EventBasicDTO() { }


        public EventBasicDTO(int id, string name, int mosqueID, DateTime startDate, DateTime endDate)
        {
            ID = id;
            Name = name;
            MosqueID = mosqueID;            
            StartDate = startDate;
            EndDate = endDate;
            //CreatedByAdminID = createdByAdminID;
        }
    }

    public class EventDTO : EventBasicDTO
    {
        //public int ID { get; set; }
        //public string Name { get; set; }
        //public int MosqueID { get; set; }
        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
        ////public int CreatedByAdminID { get; set; }

        public EventDTO() { }

        public MosqueDTO Mosque { get; set; }
        
        public EventDTO(int id, string name, int mosqueID, DateTime startDate, DateTime endDate)
        {
            ID = id;
            Name = name;
            MosqueID = mosqueID;
            Mosque = MosquesData.GetMosqueByID(mosqueID);
            StartDate = startDate;
            EndDate = endDate;
            //CreatedByAdminID = createdByAdminID;
        }
    }
    
public class EventData
    {
        private static readonly string connectionString = DataAccessSettings.ConnectionString;

        /// <summary>
        /// Creates a new event and returns the inserted ID.
        /// </summary>
        /// <param name="eventDTO">Event data to insert</param>
        /// <param name="insertedID">Returns the newly inserted event ID</param>
        /// <returns>True if insertion was successful, otherwise false</returns>
        public static int CreateEvent(EventBasicDTO eventDTO)
        {
            int insertedID;
            insertedID = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_CreateEvent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", eventDTO.Name);
                    cmd.Parameters.AddWithValue("@MosqueID", eventDTO.MosqueID);
                    cmd.Parameters.AddWithValue("@StartDate", eventDTO.StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", eventDTO.EndDate);
                    //cmd.Parameters.AddWithValue("@CreatedByAdminID", eventDTO.CreatedByAdminID);

                    // Output parameter for inserted ID
                    SqlParameter outputParam = new SqlParameter("@InsertedID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    insertedID = Convert.ToInt32(outputParam.Value);
                    return insertedID;
                }
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Updates an existing event.
        /// </summary>
        /// <param name="eventDTO">Event data with updated values</param>
        /// <returns>True if update was successful, otherwise false</returns>
        public static bool UpdateEvent(EventBasicDTO eventDTO)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_UpdateEvent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EventID", eventDTO.ID);
                    cmd.Parameters.AddWithValue("@Name", eventDTO.Name);
                    cmd.Parameters.AddWithValue("@MosqueID", eventDTO.MosqueID);
                    cmd.Parameters.AddWithValue("@StartDate", eventDTO.StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", eventDTO.EndDate);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes an event by ID.
        /// </summary>
        /// <param name="eventID">ID of the event to delete</param>
        /// <returns>True if deletion was successful, otherwise false</returns>
        public static bool DeleteEvent(int eventID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_DeleteEvent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EventID", eventID);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets an event by its ID.
        /// </summary>
        /// <param name="eventID">ID of the event</param>
        /// <returns>EventDTO if found, otherwise null</returns>
        public static EventDTO GetEventByID(int eventID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetEventByID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EventID", eventID);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new EventDTO(
                                Convert.ToInt32(reader["ID"]),
                                reader["Name"].ToString(),
                                Convert.ToInt32(reader["MosqueID"]),
                                Convert.ToDateTime(reader["StartDate"]),
                                Convert.ToDateTime(reader["EndDate"])
                                //Convert.ToInt32(reader["CreatedByAdminID"])
                            );
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        /// <summary>
        /// Retrieves all events.
        /// </summary>
        /// <returns>List of EventDTOs</returns>
        public static List<EventDTO> GetAllEvents()
        {
            List<EventDTO> events = new List<EventDTO>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetAllEvents", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            events.Add(new EventDTO(
                                Convert.ToInt32(reader["ID"]),
                                reader["Name"].ToString(),
                                Convert.ToInt32(reader["MosqueID"]),
                                Convert.ToDateTime(reader["StartDate"]),
                                Convert.ToDateTime(reader["EndDate"])
                                //Convert.ToInt32(reader["CreatedByAdminID"])
                            ));
                        }
                    }
                }
            }
            catch
            {
                return new List<EventDTO>(); // Return empty list if an error occurs
            }
            return events;
        }
    }

}
