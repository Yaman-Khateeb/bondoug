using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer;
using DAL;
using Microsoft.AspNetCore.Identity;
namespace APILayer.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet("All",Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        
        public ActionResult<IEnumerable<UserDTO>> GetAll()
        {
            var users = Users.GetAllUsers();
            if (users.Count == 0)
                return NotFound("No users found!");
            else            
                return Ok(users);            
        }
        [HttpGet("{ID}", Name = "GetUserByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserDTO> GetUserByID(int ID)
        {
            if (ID < 0)
            {
                return BadRequest("Unvalid user ID");
            }
            var user = Users.GetUserByID(ID);
            if(user == null)
                return NotFound("User was not found!");
            else
                return Ok(user);
        }

        [HttpPost(Name = "AddUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserDTO> AddUser(UserRegisterDTO newUserDTO)
        {
            // Validate the data
            if (newUserDTO == null ||
                string.IsNullOrEmpty(newUserDTO.FirstName) || (newUserDTO.FirstName).ToLower() == "string" ||
                string.IsNullOrEmpty(newUserDTO.LastName) ||
                string.IsNullOrEmpty(newUserDTO.MobileNumber) || (newUserDTO.LastName).ToLower() == "string" || 
                string.IsNullOrEmpty(newUserDTO.Password) || (newUserDTO.Password).ToLower() == "string")
            {
                return BadRequest("Invalid user data.");
            }

            // Hash the password using PasswordHasher
            var hasher = new PasswordHasher<object>();
            string hashedPassword = hasher.HashPassword(null, newUserDTO.Password);

            // Create UserDTO with the hashed password
            UserDTO userDto = new UserDTO(-1, newUserDTO.FirstName, newUserDTO.LastName, newUserDTO.MobileNumber, hashedPassword);

            // Create the user in the database (returns true or false)
            bool result = Users.CreateUser(userDto);

            if (result)
            {
                // Assign the ID from the database (after successful creation)
                newUserDTO.ID = userDto.ID;

                // Return the created user with status 201 (Created)
                return CreatedAtRoute("GetUserById", new { id = newUserDTO.ID }, newUserDTO);
            }
            else
            {
                // User creation failed
                return BadRequest("Failed to create user.");
            }
        }

        [HttpPut(Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]        
        public ActionResult<UserDTO> UpdateBasicInfo(int id, UserBasicDTO newUserDTO)
        {
            // Validate the input data
            if (newUserDTO == null ||
                string.IsNullOrEmpty(newUserDTO.FirstName) || (newUserDTO.FirstName).ToLower() == "string" ||
                string.IsNullOrEmpty(newUserDTO.LastName) || (newUserDTO.LastName).ToLower() == "string" ||
                string.IsNullOrEmpty(newUserDTO.MobileNumber) || (newUserDTO.MobileNumber).ToLower() == "string")
            {
                return BadRequest("Invalid user data.");
            }

            // Fetch user from database
            var user = Users.GetUserByID(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Update the user's basic information
            user.FirstName = newUserDTO.FirstName;
            user.LastName = newUserDTO.LastName;
            user.MobileNumber = newUserDTO.MobileNumber;
            user.Email = newUserDTO.Email;

            // Save the updated information to the database
            bool result = Users.UpdateUser(user);
            if (result)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest("Failed to update user.");
            }
        }


        [HttpPut("update-password", Name = "UpdatePassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdatePassword(int id, string oldPassword,string newPassword)
        {
            // Validate the input data
            if (id <= 0 || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(oldPassword))
            {
                return BadRequest("Invalid user data.");
            }

            // Fetch user from database
            var user = Users.GetUserByID(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }


            var hasher = new PasswordHasher<object>();
            
            var currentHashPasword = Users.GetUserHashPassword(id);
            var VerifyPassword = hasher.VerifyHashedPassword(null, currentHashPasword, oldPassword);
            if (VerifyPassword == PasswordVerificationResult.Failed)
            {
                return BadRequest("Password does not match.");
            }
            // Hash the password using PasswordHasher
            string hashedPassword = hasher.HashPassword(null, newPassword);


            // Save the updated information to the database
            bool result = Users.UpdateUserPassword(id,hashedPassword);
            if (result)
            {
                return Ok("Password was updated successfully");
            }
            else
            {
                return BadRequest("Failed to update user password.");
            }
        }
        [HttpDelete("{id}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)] 
        public ActionResult DeleteUser(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var user = Users.GetUserByID(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            bool isDeleted = Users.DeleteUser(id);
            if (isDeleted)
            {
                return Ok("User deleted successfully.");
            }
            else
            {
                return BadRequest("Failed to delete user.");
            }
        }

    }
}
