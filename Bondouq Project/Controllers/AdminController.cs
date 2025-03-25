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

        /// <summary>
        /// Authenticates an admin using their mobile number and password hash.
        /// </summary>
        /// <param name="mobileNumber">Admin's registered mobile number</param>
        /// <param name="passwordHash">Hashed password</param>
        /// <returns>Authenticated AdminBasicDTO if successful</returns>
        [HttpPost("authenticate", Name = "AuthenticateAdmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<AdminBasicDTO> AuthenticateAdmin([FromBody] LoginRequestDTO request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.MobileNumber) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Mobile number and password are required.");
            }

            var hasher = new PasswordHasher<object>();
            


            var admin = Admin.AuthenticateAdmin(request.MobileNumber);

            if (admin == null)
                return Unauthorized("Invalid credentials.");

            bool isPasswordCorrect = hasher.VerifyHashedPassword(null, admin.User.PasswordHash, request.Password) == PasswordVerificationResult.Success;
            if (!isPasswordCorrect)
                return Unauthorized("Invalid credentials.");

            var user = admin.User;
            //Right now all permissions will be set to -1
            AdminBasicDTO adminBasic = new AdminBasicDTO(admin.ID, -1, admin.UserID, new UserBasicDTO(user.ID, user.FirstName,user.LastName, user.MobileNumber, user.Email));

            return Ok(adminBasic);
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
        # region does has permission
        //[HttpGet("HasPermission")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public ActionResult<bool> HasPermission(int AdminID, int RequiredPermission)
        //{
        //    if (AdminID <= 0)
        //        return BadRequest("Invalid Admin ID.");

        //    bool hasPermission = Admin.HasPermission(AdminID, RequiredPermission);
        //    return Ok(hasPermission);
        //}
        #endregion
        public class LoginRequestDTO
        {
            public string MobileNumber { get; set; }
            public string Password { get; set; }
        }

    }
}
