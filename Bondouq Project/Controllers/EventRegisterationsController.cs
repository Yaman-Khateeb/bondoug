using BL;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("api/EventRegistrations")]
    [ApiController]
    public class EventRegistrationsController : ControllerBase
    {
        /// <summary>
        /// Registers a user for an event.
        /// </summary>
        /// <param name="eventRegistrationDTO">Registration details.</param>
        /// <returns>HTTP response with inserted ID or error.</returns>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult RegisterForEvent([FromBody] EventRegistrationDTO eventRegistrationDTO)
        {

            if (eventRegistrationDTO == null)
            {
                return BadRequest("Invalid registration data.");
            }

            if (eventRegistrationDTO.Longitude.HasValue &&
                (eventRegistrationDTO.Longitude < -180 || eventRegistrationDTO.Longitude > 180))
            {
                return BadRequest("Invalid longitude value. It must be between -180 and 180.");
            }

            if (eventRegistrationDTO.Latitude.HasValue &&
                (eventRegistrationDTO.Latitude < -90 || eventRegistrationDTO.Latitude > 90))
            {
                return BadRequest("Invalid latitude value. It must be between -90 and 90.");
            }


            int insertedID = EventRegistration.RegisterForEvent(eventRegistrationDTO);

            if (insertedID > 0)
                return CreatedAtAction(nameof(GetEventRegistrationByID), new { registrationID = insertedID }, new { RegistrationID = insertedID });

            return BadRequest("Failed to register for the event.");
        }

        /// <summary>
        /// Updates an existing event registration.
        /// </summary>
        /// <param name="eventRegistrationDTO">Updated registration details.</param>
        /// <returns>HTTP response indicating success or failure.</returns>
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateEventRegistration([FromBody] EventRegistrationDTO eventRegistrationDTO)
        {
            if (eventRegistrationDTO == null || eventRegistrationDTO.ID <= 0)
                return BadRequest("Invalid registration ID or data.");

            if (eventRegistrationDTO.Longitude.HasValue &&
                (eventRegistrationDTO.Longitude < -180 || eventRegistrationDTO.Longitude > 180))
            {
                return BadRequest("Invalid longitude value. It must be between -180 and 180.");
            }

            if (eventRegistrationDTO.Latitude.HasValue &&
                (eventRegistrationDTO.Latitude < -90 || eventRegistrationDTO.Latitude > 90))
            {
                return BadRequest("Invalid latitude value. It must be between -90 and 90.");
            }



            bool isUpdated = EventRegistration.UpdateEventRegistration(eventRegistrationDTO);

            if (isUpdated)
                return Ok("Event registration updated successfully.");

            return BadRequest("Failed to update event registration.");
        }

        /// <summary>
        /// Deletes an event registration.
        /// </summary>
        /// <param name="registrationID">The ID of the registration to delete.</param>
        /// <returns>HTTP response indicating success or failure.</returns>
        [HttpDelete("delete/{registrationID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteEventRegistration(int registrationID)
        {
            if (registrationID <= 0)
                return BadRequest("Invalid registration ID.");

            bool isDeleted = EventRegistration.DeleteEventRegistration(registrationID);

            if (isDeleted)
                return Ok("Event registration deleted successfully.");

            return NotFound("Event registration not found.");
        }

        /// <summary>
        /// Retrieves a specific event registration by ID.
        /// </summary>
        /// <param name="registrationID">The ID of the registration.</param>
        /// <returns>Event registration details.</returns>
        [HttpGet("get/{registrationID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEventRegistrationByID(int registrationID)
        {
            if (registrationID <= 0)
                return BadRequest("Invalid registration ID.");

            var registration = EventRegistration.GetEventRegistrationByID(registrationID);

            if (registration != null)
                return Ok(registration);

            return NotFound("Event registration not found.");
        }

        /// <summary>
        /// Retrieves all event registrations for a specific event.
        /// </summary>
        /// <param name="eventID">The event ID.</param>
        /// <returns>List of event registrations.</returns>
        [HttpGet("get-by-event/{eventID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEventRegistrationsByEventID(int eventID)
        {
            if (eventID <= 0)
                return BadRequest("Invalid event ID.");

            List<EventRegistrationDTO> registrations = EventRegistration.GetEventRegistrationsByEventID(eventID);

            if (registrations.Count > 0)
                return Ok(registrations);

            return NotFound("No registrations found for this event.");
        }

        ///// <summary>
        ///// Retrieves all event registrations for a specific mosque.
        ///// </summary>
        ///// <param name="mosqueID">The mosque ID.</param>
        ///// <returns>List of event registrations.</returns>
        //[HttpGet("get-by-mosque/{mosqueID}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public IActionResult GetAllEventRegistrationsByMosque(int mosqueID)
        //{
        //    if (mosqueID <= 0)
        //        return BadRequest("Invalid mosque ID.");

        //    List<EventRegistrationDTO> registrations = EventRegistration.GetAllEventRegistrationsByMosque(mosqueID);

        //    if (registrations.Count > 0)
        //        return Ok(registrations);

        //    return NotFound("No registrations found for this mosque.");
        //}
    }
}
