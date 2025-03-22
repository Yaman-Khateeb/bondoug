using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class AdminDTO
    {
        public int ID { get; set; }
        public int Permissions { get; set; }
        public int UserID { get; set; }
        // Inner User object to store the related User data
        public UserDTO User { get; set; }
        // Constructor to initialize the AdminDTO with a UserDTO
        public AdminDTO(int id, int permissions, int userId, UserDTO user)
        {
            ID = id;
            Permissions = permissions;
            UserID = userId;
            User = user;
        }
    }

    public class AdminBasicDTO
    {
        public int ID { get; set; }
        public int Permissions { get; set; }
        public int UserID { get; set; }
        public UserBasicDTO User { get; set; }
        public AdminBasicDTO(int iD, int permissions, int userID, UserBasicDTO user)
        {
            ID = iD;
            Permissions = permissions;
            UserID = userID;
            User = user;
        }
    }

    public class AdminData
    {
        private static String _connectionString = DataAccessSettings.ConnectionString2;

        public static bool AddNewAdmin(AdminDTO adminDTO)
        {
            bool result = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_CreateUserAndAdmin", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Input Parameters (User Info)
                        cmd.Parameters.AddWithValue("@FirstName", adminDTO.User.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", adminDTO.User.LastName);
                        cmd.Parameters.AddWithValue("@MobileNumber", adminDTO.User.MobileNumber);
                        cmd.Parameters.AddWithValue("@PasswordHash", adminDTO.User.PasswordHash);  // 🔹 Added this
                        // Input Parameter (Admin Info)
                        cmd.Parameters.AddWithValue("@Permissions", adminDTO.Permissions);
                        // Output Parameters (IDs)
                        SqlParameter userIdParam = new SqlParameter("@InsertedUserID", SqlDbType.Int);
                        userIdParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(userIdParam);
                        SqlParameter adminIdParam = new SqlParameter("@InsertedAdminID", SqlDbType.Int);
                        adminIdParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(adminIdParam);
                        // Execute
                        conn.Open();
                        result = cmd.ExecuteNonQuery() > 0;
                        // Retrieve Output Values
                        int insertedUserId = (int)cmd.Parameters["@InsertedUserID"].Value;
                        int insertedAdminId = (int)cmd.Parameters["@InsertedAdminID"].Value;

                        // Optionally, store the IDs in the DTO
                        adminDTO.User.ID = insertedUserId;
                        adminDTO.ID = insertedAdminId;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (implement your logging mechanism here)
                Console.WriteLine("Error in AddNewAdmin: " + ex.Message);
                result = false;
            }
            return result;
        }

        public static bool UpdateAdmin(AdminBasicDTO adminDTO)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_UpdateUserAndAdmin", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Admin parameters
                        cmd.Parameters.AddWithValue("@UserID", adminDTO.UserID);
                        cmd.Parameters.AddWithValue("@Permissions", adminDTO.Permissions);

                        // User parameters
                        cmd.Parameters.AddWithValue("@FirstName", adminDTO.User.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", adminDTO.User.LastName);
                        cmd.Parameters.AddWithValue("@MobileNumber", adminDTO.User.MobileNumber);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (implement your logging mechanism here)
                Console.WriteLine("Error in UpdateAdmin: " + ex.Message);
                return false;
            }
        }

        public static bool DeleteAdminByUserID(int userID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_DeleteUserAndAdmin", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (implement your logging mechanism here)
                Console.WriteLine("Error in DeleteAdminByUserID: " + ex.Message);
                return false;
            }
        }

        public static AdminBasicDTO GetAdminByID(int adminID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_GetAdminByID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AdminID", adminID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                UserBasicDTO user = new UserBasicDTO
                                (
                                    (int)reader["UserID"],
                                    reader["FirstName"].ToString(),
                                    reader["LastName"].ToString(),
                                    reader["MobileNumber"].ToString(),
                                    reader["Email"] == DBNull.Value ? null : reader["Email"].ToString()
                               );

                                return new AdminBasicDTO(
                                    (int)reader["ID"],
                                    (int)reader["Permissions"],
                                    (int)reader["UserID"],
                                    user
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (implement your logging mechanism here)
                Console.WriteLine("Error in GetAdminByID: " + ex.Message);
            }
            return null;
        }

        public static AdminDTO GetAdminByUserID(int userID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_GetAdminByUserID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                UserDTO user = new UserDTO
                                (
                                    (int)reader["UserID"],
                                    reader["FirstName"].ToString(),
                                    reader["LastName"].ToString(),
                                    reader["MobileNumber"].ToString(),
                                    reader["Email"] == DBNull.Value ? null : reader["Email"].ToString()
                                );

                                return new AdminDTO(
                                    (int)reader["ID"],
                                    (int)reader["Permissions"],
                                    (int)reader["UserID"],
                                    user
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (implement your logging mechanism here)
                Console.WriteLine("Error in GetAdminByUserID: " + ex.Message);
            }
            return null;
        }

        public static AdminDTO GetAdminByNumberAndPassword(string mobileNumber, string passwordHash)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_GetAdminByNumberAndPassword", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MobileNumber", mobileNumber);
                        cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                UserDTO user = new UserDTO(
                                (int)reader["UserID"],
                                reader["FirstName"].ToString(),
                                reader["LastName"].ToString(),
                                reader["MobileNumber"].ToString(),
                                reader["Email"] == DBNull.Value ? null : reader["Email"].ToString()
                                );

                                return new AdminDTO(
                                    (int)reader["ID"],
                                    (int)reader["Permissions"],
                                    (int)reader["UserID"],
                                    user
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (implement your logging mechanism here)
                Console.WriteLine("Error in GetAdminByNumberAndPassword: " + ex.Message);
            }
            return null;
        }

        public static List<AdminDTO> GetAllAdmins()
        {
            List<AdminDTO> admins = new List<AdminDTO>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM vAdmins", conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create UserDTO using the parameterized constructor
                                UserDTO user = new UserDTO(
                                    (int)reader["UserID"],
                                    reader["FirstName"].ToString(),
                                    reader["LastName"].ToString(),
                                    reader["MobileNumber"].ToString(),
                                    null,
                                    reader["Email"] == DBNull.Value ? null : reader["Email"].ToString(),
                                    reader["CreatedAt"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["CreatedAt"],
                                    reader["LastLogin"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["LastLogin"]
                                );

                                // Create AdminDTO with UserDTO
                                AdminDTO admin = new AdminDTO(
                                    (int)reader["ID"],
                                    (int)reader["Permissions"],
                                    (int)reader["UserID"],
                                    user
                                );

                                admins.Add(admin);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (implement your logging mechanism here)
                Console.WriteLine("Error in GetAllAdmins: " + ex.Message);
            }

            return admins;
        }
    }
}
