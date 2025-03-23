using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer;
using DAL;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace APILayer.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllAdmins")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Admin>> GetAll()
        {
            var admins = Admin.GetAllAdmins();
            if (admins.Count == 0)
                return NotFound("No admins found!");
            return Ok(admins);
        }

        [HttpGet("{ID}", Name = "GetAdminByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Admin> GetAdminByID(int ID)
        {
            if (ID < 0)
                return BadRequest("Invalid admin ID");

            var admin = Admin.GetAdminByID(ID);
            if (admin == null)
                return NotFound("Admin not found!");
            return Ok(admin);
        }

        [HttpPost(Name = "AddAdmin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<AdminBasicDTO> AddAdmin(UserRegisterDTO newUserDTO, Admin.enPermissions permissions)
        {
            if (newUserDTO == null
    || string.IsNullOrWhiteSpace(newUserDTO.FirstName) || newUserDTO.FirstName.ToLower() == "string"
    || string.IsNullOrWhiteSpace(newUserDTO.LastName) || newUserDTO.LastName.ToLower() == "string"
    || string.IsNullOrWhiteSpace(newUserDTO.MobileNumber) || newUserDTO.MobileNumber.ToLower() == "string"
    || string.IsNullOrWhiteSpace(newUserDTO.Password) || newUserDTO.Password.ToLower() == "string")
                return BadRequest("Invalid user data.");

            var hasher = new PasswordHasher<object>();
            string hashedPassword = hasher.HashPassword(null, newUserDTO.Password);

            // Create UserDTO
            UserDTO userDto = new UserDTO(-1, newUserDTO.FirstName, newUserDTO.LastName, newUserDTO.MobileNumber, hashedPassword);

            // Create the admin
            bool adminCreated = Admin.CreateAdmin(userDto, permissions);
            if (!adminCreated)
                return BadRequest("Failed to create admin.");

            // Create AdminBasicDTO
            AdminBasicDTO adminBasicDTO = new AdminBasicDTO(
                userDto.ID,  // Admin ID
                (int)permissions,  // Admin's permissions
                userDto.ID,  // User ID
                new UserBasicDTO(userDto.ID, userDto.FirstName, userDto.LastName, userDto.MobileNumber, userDto.Email)  // User Basic DTO
            );

            // Return the newly created admin details
            return CreatedAtRoute("GetAdminByID", new { ID = adminBasicDTO.ID }, adminBasicDTO);
        }


        [HttpPut(Name = "UpdateAdmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdateAdmin([FromBody] AdminBasicDTO adminDTO)
        {
            if (adminDTO == null || adminDTO.User == null)
                return BadRequest("Invalid admin data.");

            // Update the user information with UserBasicDTO
            bool userUpdateResult = Users.UpdateUser(adminDTO.User);
            // Update the admin information
            bool adminUpdateResult = Admin.UpdateAdmin(adminDTO);

            if (userUpdateResult && adminUpdateResult)
                return Ok("Admin updated successfully.");
            return BadRequest("Failed to update admin.");
        }


        [HttpDelete("{ID}", Name = "DeleteAdminByUserID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteAdmin(int ID)
        {
            if (ID <= 0)
                return BadRequest("Invalid admin ID.");

            var admin = Admin.GetAdminByID(ID);
            if (admin == null)
                return NotFound("Admin not found.");

            bool isDeleted = Admin.DeleteAdminByUserID(admin.UserID);
            if (isDeleted)
                return Ok("Admin deleted successfully.");
            return BadRequest("Failed to delete admin.");
        }

        [HttpGet("HasPermission")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> HasPermission(int AdminID, int RequiredPermission)
        {
            if (AdminID <= 0)
                return BadRequest("Invalid Admin ID.");

            bool hasPermission = Admin.HasPermission(AdminID, RequiredPermission);
            return Ok(hasPermission);
        }
    }
}
