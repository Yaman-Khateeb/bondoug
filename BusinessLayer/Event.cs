using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    using DAL;
    using System;
    using System.Collections.Generic;

    public class Event
    {
        /// <summary>
        /// Adds a new event.
        /// </summary>
        /// <param name="eventDTO">Event data to insert</param>
        /// <param name="insertedID">Returns the newly inserted event ID</param>
        /// <returns>True if insertion was successful, otherwise false</returns>
        public static int CreateEvent(EventDTO eventDTO)
        {
            
            if (eventDTO == null || string.IsNullOrWhiteSpace(eventDTO.Name))
                return -1;

            if (eventDTO.MosqueID <= 0 || eventDTO.CreatedByAdminID <= 0)
                return -1;

            if (eventDTO.StartDate >= eventDTO.EndDate)
                return -1;

            return EventData.CreateEvent(eventDTO);
        }

        /// <summary>
        /// Updates an existing event.
        /// </summary>
        /// <param name="eventDTO">Updated event data</param>
        /// <returns>True if update was successful, otherwise false</returns>
        public static bool UpdateEvent(EventDTO eventDTO)
        {
            if (eventDTO == null || eventDTO.ID <= 0 || string.IsNullOrWhiteSpace(eventDTO.Name))
                return false;

            if (eventDTO.MosqueID <= 0 || eventDTO.StartDate >= eventDTO.EndDate)
                return false;

            return EventData.UpdateEvent(eventDTO);
        }

        /// <summary>
        /// Deletes an event by ID.
        /// </summary>
        /// <param name="eventID">ID of the event to delete</param>
        /// <returns>True if deletion was successful, otherwise false</returns>
        public static bool DeleteEvent(int eventID)
        {
            if (eventID <= 0)
                return false;

            return EventData.DeleteEvent(eventID);
        }

        /// <summary>
        /// Retrieves an event by ID.
        /// </summary>
        /// <param name="eventID">ID of the event</param>
        /// <returns>EventDTO if found, otherwise null</returns>
        public static EventDTO GetEventByID(int eventID)
        {
            if (eventID <= 0)
                return null;

            return EventData.GetEventByID(eventID);
        }

        /// <summary>
        /// Retrieves all events.
        /// </summary>
        /// <returns>List of EventDTOs</returns>
        public static List<EventDTO> GetAllEvents()
        {
            return EventData.GetAllEvents();
        }
    }

}
