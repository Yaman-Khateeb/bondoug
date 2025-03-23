using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer;
using System.Collections.Generic;
using DAL;

namespace APILayer.Controllers
{
    [Route("api/Event")]
    [ApiController]
    public class EventController : ControllerBase
    {
        #region Get All Events
        /// <summary>
        /// Retrieves all events.
        /// </summary>
        /// <returns>List of all events.</returns>
        [HttpGet("All", Name = "GetAllEvents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<EventDTO>> GetAllEvents()
        {
            var events = Event.GetAllEvents();
            if (events.Count == 0)
                return NotFound("No events found.");

            return Ok(events);
        }
        #endregion

        #region Get Event by ID
        /// <summary>
        /// Retrieves an event by its ID.
        /// </summary>
        /// <param name="ID">Event ID</param>
        /// <returns>Event details</returns>
        [HttpGet("{ID}", Name = "GetEventByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<EventDTO> GetEventByID(int ID)
        {
            if (ID <= 0)
                return BadRequest("Invalid event ID.");

            var eventDTO = Event.GetEventByID(ID);
            if (eventDTO == null)
                return NotFound("Event not found.");

            return Ok(eventDTO);
        }
        #endregion

        #region Add New Event
        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <param name="eventDTO">Event data</param>
        /// <returns>Created event details</returns>
        [HttpPost(Name = "AddEvent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<EventDTO> AddEvent([FromBody] EventDTO eventDTO)
        {
            if (eventDTO == null || string.IsNullOrWhiteSpace(eventDTO.Name)
                || eventDTO.MosqueID <= 0 || eventDTO.CreatedByAdminID <= 0
                || eventDTO.StartDate >= eventDTO.EndDate)
                return BadRequest("Invalid event data.");

            int eventID = Event.CreateEvent(eventDTO);
            if (eventID <= 0)
                return BadRequest("Failed to create event.");

            eventDTO.ID = eventID;
            return CreatedAtRoute("GetEventByID", new { ID = eventID }, eventDTO);
        }
        #endregion

        #region Update Event
        /// <summary>
        /// Updates an existing event.
        /// </summary>
        /// <param name="eventDTO">Updated event data</param>
        /// <returns>Update status</returns>
        [HttpPut(Name = "UpdateEvent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdateEvent([FromBody] EventDTO eventDTO)
        {
            if (eventDTO == null || eventDTO.ID <= 0 || string.IsNullOrWhiteSpace(eventDTO.Name)
                || eventDTO.MosqueID <= 0 || eventDTO.StartDate >= eventDTO.EndDate)
                return BadRequest("Invalid event data.");

            bool updated = Event.UpdateEvent(eventDTO);
            if (!updated)
                return BadRequest("Failed to update event.");

            return Ok("Event updated successfully.");
        }
        #endregion

        #region Delete Event
        /// <summary>
        /// Deletes an event by its ID.
        /// </summary>
        /// <param name="ID">Event ID</param>
        /// <returns>Deletion status</returns>
        [HttpDelete("{ID}", Name = "DeleteEvent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteEvent(int ID)
        {
            if (ID <= 0)
                return BadRequest("Invalid event ID.");

            var eventDTO = Event.GetEventByID(ID);
            if (eventDTO == null)
                return NotFound("Event not found.");

            bool isDeleted = Event.DeleteEvent(ID);
            if (!isDeleted)
                return BadRequest("Failed to delete event.");

            return Ok("Event deleted successfully.");
        }
        #endregion
    }
}
