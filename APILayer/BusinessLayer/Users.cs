using DAL;
using System;
using System.Collections.Generic;

namespace BusinessLayer
{
    public class UserRegisterDTO
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }  // Plain password (Only used for input)
    }
  
    public class Users
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }

        

        

        public UserDTO UDTO
        {
            get { return new UserDTO(ID, FirstName, LastName, MobileNumber, null, Email, CreatedAt, LastLogin); }
        }

        public static UserBasicDTO GetUserByID(int userID)
        {
            return UserData.GetUserByID(userID);
        }

        public static String GetUserHashPassword(int userID)
        {
            return UserData.GetUserHashPassword(userID);
        }

        public static List<UserBasicDTO> GetAllUsers()
        {
            return UserData.GetAllUsers();
        }

        public static bool CreateUser(UserDTO user)
        {
            return UserData.CreateUser(user);
        }

        public static bool UpdateUser(UserBasicDTO user)
        {
            return UserData.UpdateUser(user);
        }

        public static bool UpdateUserPassword(int ID,String password)
        {
            return UserData.UpdateUserPassword(ID,password);
        }



        public static bool DeleteUser(int userID)
        {
               return UserData.DeleteUser(userID);
        }
    }
}
