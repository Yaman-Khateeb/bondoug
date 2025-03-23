using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer;
using DAL;
using System.Collections.Generic;

namespace APILayer.Controllers
{
    [Route("api/Mosques")]
    [ApiController]
    public class MosquesController : ControllerBase
    {
        #region GetAll
        /// <summary>
        /// Get all mosques
        /// </summary>
        /// <returns>List of all mosques</returns>
        [HttpGet("All", Name = "GetAllMosques")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<MosqueDTO>> GetAll()
        {
            var mosques = Mosque.GetAllMosques();
            if (mosques.Count == 0)
                return NotFound("No mosques found!");
            return Ok(mosques);
        }
        #endregion

        #region Get By ID
        /// <summary>
        /// Get a mosque by ID
        /// </summary>
        /// <param name="ID">Mosque ID</param>
        /// <returns>Mosque details</returns>
        [HttpGet("{ID}", Name = "GetMosqueByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<MosqueDTO> GetMosqueByID(int ID)
        {
            if (ID <= 0)
                return BadRequest("Invalid mosque ID.");

            var mosque = Mosque.GetMosqueByID(ID);
            if (mosque == null)
                return NotFound("Mosque not found!");
            return Ok(mosque);
        }
        #endregion

        #region Add new
        /// <summary>
        /// Add a new mosque
        /// </summary>
        /// <param name="mosqueDTO">Mosque data</param>
        /// <returns>Created mosque details</returns>
        [HttpPost(Name = "AddMosque")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<MosqueDTO> AddMosque([FromBody] MosqueDTO mosqueDTO)
        {
            if (mosqueDTO == null
                || string.IsNullOrWhiteSpace(mosqueDTO.Name) || mosqueDTO.Name.ToLower() == "string"
                || string.IsNullOrWhiteSpace(mosqueDTO.Address) || mosqueDTO.Address.ToLower() == "string"
                || mosqueDTO.Longitude < -180 || mosqueDTO.Longitude > 180
                || mosqueDTO.Latitude < -90 || mosqueDTO.Latitude > 90)
                return BadRequest("Invalid mosque data.");

            int mosqueID = Mosque.CreateMosque(mosqueDTO);
            if (mosqueID <= 0)
                return BadRequest("Failed to create mosque.");

            mosqueDTO.ID = mosqueID;
            return CreatedAtRoute("GetMosqueByID", new { ID = mosqueID }, mosqueDTO);
        }

        #endregion

        #region Update
        /// <summary>
        /// Update an existing mosque
        /// </summary>
        /// <param name="mosqueDTO">Updated mosque data</param>
        /// <returns>Update status</returns>
        [HttpPut(Name = "UpdateMosque")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdateMosque([FromBody] MosqueDTO mosqueDTO)
        {
            if (mosqueDTO == null || mosqueDTO.ID <= 0
                || string.IsNullOrWhiteSpace(mosqueDTO.Name) || mosqueDTO.Name.ToLower() == "string"
                || string.IsNullOrWhiteSpace(mosqueDTO.Address) || mosqueDTO.Address.ToLower() == "string"
                || mosqueDTO.Longitude < -180 || mosqueDTO.Longitude > 180
                || mosqueDTO.Latitude < -90 || mosqueDTO.Latitude > 90)
                return BadRequest("Invalid mosque data.");
                

            
            bool updated = Mosque.UpdateMosque(mosqueDTO);
            if (updated)
                return Ok("Mosque updated successfully.");
            return BadRequest("Failed to update mosque.");
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete a mosque by ID
        /// </summary>
        /// <param name="ID">Mosque ID</param>
        /// <returns>Delete status</returns>
        [HttpDelete("{ID}", Name = "DeleteMosque")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteMosque(int ID)
        {
            if (ID <= 0)
                return BadRequest("Invalid mosque ID.");

            var mosque = Mosque.GetMosqueByID(ID);
            if (mosque == null)
                return NotFound("Mosque not found!");

            bool deleted = Mosque.DeleteMosque(ID);
            if (deleted)
                return Ok("Mosque deleted successfully.");
            return BadRequest("Failed to delete mosque.");
        }
        #endregion
    }
}
