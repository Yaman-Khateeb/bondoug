using System;
using System.Collections.Generic;
using System.Data;
using DAL;
using Microsoft.Data.SqlClient;

public class MosqueDTO
{
    public int ID { get; set; }
    public string Name { get; set; }    
    public float Longitude { get; set; }
    public float Latitude { get; set; }
}

public static class MosquesData
{
    private static readonly string connectionString = DataAccessSettings.ConnectionString;

    public static int CreateMosque(MosqueDTO mosque)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_CreateMosque", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", mosque.Name);
                
                cmd.Parameters.AddWithValue("@Longitude", mosque.Longitude);
                cmd.Parameters.AddWithValue("@Latitude", mosque.Latitude);

                // Define the output parameter
                SqlParameter outputIdParam = new SqlParameter("@InsertedID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputIdParam);

                conn.Open();
                cmd.ExecuteNonQuery();

                return Convert.ToInt32(outputIdParam.Value); // Get the inserted ID
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Error creating mosque", ex);
        }
    }

    public static bool UpdateMosque(MosqueDTO mosque)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_UpdateMosque", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MosqueID", mosque.ID);
                cmd.Parameters.AddWithValue("@Name", mosque.Name);
                
                cmd.Parameters.AddWithValue("@Longitude", mosque.Longitude);
                cmd.Parameters.AddWithValue("@Latitude", mosque.Latitude);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating mosque", ex);
        }
    }

    public static bool DeleteMosque(int mosqueID)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_DeleteMosque", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MosqueID", mosqueID);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error deleting mosque", ex);
        }
    }

    public static MosqueDTO GetMosqueByID(int mosqueID)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetMosqueByID", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MosqueID", mosqueID);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new MosqueDTO
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Name = reader["Name"].ToString(),
                            
                            Longitude = Convert.ToSingle(reader["Longitude"]),
                            Latitude = Convert.ToSingle(reader["Latitude"])
                        };
                    }
                }
            }
            return null;
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving mosque by ID", ex);
        }
    }

    public static List<MosqueDTO> GetAllMosques()
    {
        List<MosqueDTO> mosques = new List<MosqueDTO>();
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("select * from Mosques", conn))
            {                
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        mosques.Add(new MosqueDTO
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Name = reader["Name"].ToString(),
                            
                            Longitude = Convert.ToSingle(reader["Longitude"]),
                            Latitude = Convert.ToSingle(reader["Latitude"])
                        });
                    }
                }
            }
            return mosques;
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving all mosques", ex);
        }
    }
}
