using DAL;
using System;
using System.Collections.Generic;

namespace BusinessLayer
{
    public class Admin
    {
        public int ID { get; set; }
        public int Permissions { get; set; }
        public int UserID { get; set; }
        public UserDTO User { get; set; }

        public enum enPermissions { FullAccess = 1, LimitedAccess = 2 };

        public Admin(int id, int permissions, int userId, UserDTO user)
        {
            ID = id;
            Permissions = permissions;
            UserID = userId;
            User = user;
        }

        public static AdminBasicDTO GetAdminByID(int adminID)
        {
            AdminBasicDTO dto = AdminData.GetAdminByID(adminID);
            return dto;
        }

        public static List<Admin> GetAllAdmins()
        {
            List<Admin> admins = new List<Admin>();
            foreach (var dto in AdminData.GetAllAdmins())
            {
                admins.Add(new Admin(dto.ID, dto.Permissions, dto.UserID, dto.User));
            }
            return admins;
        }

        public static bool CreateAdmin(UserDTO user, enPermissions permissions)
        {
            AdminDTO adminDTO = new AdminDTO(0, (int)permissions, user.ID, user);
            return AdminData.AddNewAdmin(adminDTO);
        }

        public static bool UpdateAdmin(AdminBasicDTO admin)
        {
            //AdminDTO dto = new AdminDTO(admin.ID, admin.Permissions, admin.UserID, admin.User);
            return AdminData.UpdateAdmin(admin);
        }

        public static bool DeleteAdminByUserID(int adminID)
        {
            return AdminData.DeleteAdminByUserID(adminID);
        }

        public static bool HasPermission(int AdminID,int requiredPermission)
        {
            var User = AdminData.GetAdminByID(AdminID);
            return (User.Permissions & requiredPermission) == requiredPermission;
        }

    }
}
