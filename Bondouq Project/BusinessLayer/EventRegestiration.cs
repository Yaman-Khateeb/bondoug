using DAL;
using System;
using System.Collections.Generic;

namespace BL
{
    public class EventRegistration
    {
        /// <summary>
        /// Registers a user for an event.
        /// </summary>
        /// <param name="eventRegistrationDTO">Registration details.</param>
        /// <returns>Inserted registration ID, or -1 if failed.</returns>
        public static int RegisterForEvent(EventRegistrationDTO eventRegistrationDTO)
        {
            if (eventRegistrationDTO == null || eventRegistrationDTO.UserID <= 0 || eventRegistrationDTO.EventID <= 0)
                return -1;

            if (eventRegistrationDTO.ComingStatus < 0 || eventRegistrationDTO.ComingStatus > 3)
                return -1;

            return EventRegistrationsData.CreateEventRegistration(eventRegistrationDTO);
        }

        /// <summary>
        /// Updates an existing event registration.
        /// </summary>
        /// <param name="eventRegistrationDTO">Updated registration details.</param>
        /// <returns>True if updated successfully, otherwise false.</returns>
        public static bool UpdateEventRegistration(EventRegistrationDTO eventRegistrationDTO)
        {
            if (eventRegistrationDTO == null || eventRegistrationDTO.ID <= 0)
                return false;

            return EventRegistrationsData.UpdateEventRegistration(eventRegistrationDTO);
        }

        /// <summary>
        /// Deletes an event registration.
        /// </summary>
        /// <param name="registrationID">The ID of the registration to delete.</param>
        /// <returns>True if deleted successfully, otherwise false.</returns>
        public static bool DeleteEventRegistration(int registrationID)
        {
            if (registrationID <= 0)
                return false;

            return EventRegistrationsData.DeleteEventRegistration(registrationID);
        }

        /// <summary>
        /// Retrieves a specific event registration by ID.
        /// </summary>
        /// <param name="registrationID">The ID of the registration.</param>
        /// <returns>Event registration details, or null if not found.</returns>
        public static EventRegistrationDTO GetEventRegistrationByID(int registrationID)
        {
            if (registrationID <= 0)
                return null;

            return EventRegistrationsData.GetEventRegistrationByID(registrationID);
        }

        /// <summary>
        /// Retrieves all event registrations for a specific event.
        /// </summary>
        /// <param name="eventID">The event ID.</param>
        /// <returns>List of event registrations.</returns>
        public static List<EventRegistrationDTO> GetEventRegistrationsByEventID(int eventID)
        {
            if (eventID <= 0)
                return new List<EventRegistrationDTO>();

            return EventRegistrationsData.GetEventRegistrationsByEventID(eventID);
        }

        ///// <summary>
        ///// Retrieves all event registrations for a specific mosque.
        ///// </summary>
        ///// <param name="mosqueID">The mosque ID.</param>
        ///// <returns>List of event registrations.</returns>
        //public static List<EventRegistrationDTO> GetAllEventRegistrationsByMosque(int mosqueID)
        //{
        //    if (mosqueID <= 0)
        //        return new List<EventRegistrationDTO>();

        //    return EventRegistrationsData.GetAllEventRegistrationsByMosque(mosqueID);
        //}
    }
}
