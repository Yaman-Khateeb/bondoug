using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class UserDTO
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }

        // Parameterized constructor
        public UserDTO(int id, string firstName, string lastName, string mobileNumber, string passwordHash = null, string email = null, DateTime? createdAt = null, DateTime? lastLogin = null)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            MobileNumber = mobileNumber;
            PasswordHash = passwordHash;
            Email = email;
            CreatedAt = createdAt.HasValue ? createdAt.Value : null;
            LastLogin = lastLogin.HasValue ? lastLogin.Value : null;
        }
    }

    public class UserBasicDTO
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }

        // Parameterized constructor
        public UserBasicDTO(int id, string firstName, string lastName, string mobileNumber, string email = null)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            MobileNumber = mobileNumber;
            Email = email;
        }
    }

    public class UserData
    {
        private static readonly string _connectionString = DataAccessSettings.ConnectionString2;

        public static UserBasicDTO GetUserByID(int userID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_GetUserByID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new UserBasicDTO(
                                    (int)reader["ID"],
                                    reader["FirstName"].ToString(),
                                    reader["LastName"].ToString(),
                                    reader["MobileNumber"].ToString(),
                                    reader["Email"].ToString()
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                Console.WriteLine("Error in GetUserByID: " + ex.Message);
            }
            return null;
        }

        public static List<UserBasicDTO> GetAllUsers()
        {
            List<UserBasicDTO> users = new List<UserBasicDTO>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "Select * from users";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            UserBasicDTO user = new UserBasicDTO(
                                (int)reader["ID"],
                                reader["FirstName"].ToString(),
                                reader["LastName"].ToString(),
                                reader["MobileNumber"].ToString(),
                                reader["Email"] == DBNull.Value ? null : reader["Email"].ToString()
                            );

                            users.Add(user);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                Console.WriteLine("Error in GetAllUsers: " + ex.Message);
            }

            return users;
        }

        public static bool CreateUser(UserDTO user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_CreateUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", user.LastName);
                        cmd.Parameters.AddWithValue("@MobileNumber", user.MobileNumber);
                        cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                        cmd.Parameters.AddWithValue("@Email", user.Email);

                        SqlParameter insertedIDParam = new SqlParameter("@InsertedID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(insertedIDParam);

                        cmd.ExecuteNonQuery();

                        // Check if InsertedID was assigned
                        if (insertedIDParam.Value != DBNull.Value)
                        {
                            user.ID = (int)insertedIDParam.Value;
                            return true; // Return true if user was successfully created
                        }
                        else
                        {
                            return false; // Return false if ID was not assigned
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                Console.WriteLine("Error in CreateUser: " + ex.Message);
                return false;
            }
        }

        public static bool UpdateUser(UserBasicDTO user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_UpdateUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", user.ID);
                        cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", user.LastName);
                        cmd.Parameters.AddWithValue("@MobileNumber", user.MobileNumber);
                        cmd.Parameters.AddWithValue("@Email", user.Email);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                Console.WriteLine("Error in UpdateUser: " + ex.Message);
                return false;
            }
        }

        public static bool UpdateUserPassword(int ID, string newPasswordHash)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("[sp_UpdateUserPassword]", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", ID);
                        cmd.Parameters.AddWithValue("@NewPasswordHash", newPasswordHash);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                Console.WriteLine("Error in UpdateUserPassword: " + ex.Message);
                return false;
            }
        }

        public static string GetUserHashPassword(int userID)
        {
            string hashedPassword = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT PasswordHash FROM Users WHERE ID = @ID", conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", userID);

                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            hashedPassword = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                Console.WriteLine("Error fetching password hash: " + ex.Message);
            }

            return hashedPassword;
        }

        public static bool DeleteUser(int userID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_DeleteUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                Console.WriteLine("Error in DeleteUser: " + ex.Message);
                return false;
            }
        }
    }
}
