using RacketSpeed.Core.Models.Event;

namespace RacketSpeed.Core.Contracts
{
    /// <summary>
    /// Service class for Event Entity.
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Async method to retrieve all Events.
        /// </summary>
        /// <param name="category">Category for fitlering purpose.</param>
        /// <param name="isAdministrator">Boolean for taking certain amount of when the user is administrator events.</param>
        /// <returns>Collection of EventViewModel.</returns>
        Task<ICollection<EventViewModel>> AllAsync(string category, bool isAdministrator);

        /// <summary>
        /// Async method to retrieve a single Event.
        /// </summary>
        /// <param name="eventId">Entity Identificator.</param>
        /// <returns>EventDetailsViewModel.</returns>
        Task<EventDetailsViewModel> GetByIdAsync(Guid eventId);

        /// <summary>
        /// Async method to add an Entity to the Database.
        /// </summary>
        /// <param name="model">Event Form Model.</param>
        Task AddAsync(EventFormModel model);

        /// <summary>
        /// Async method to Edit an Entity.
        /// </summary>
        /// <param name="model">Event Form Model.</param>
        Task EditAsync(EventFormModel model);

        /// <summary>
        /// Async method to Delete an Entity.
        /// </summary>
        /// <param name="eventId">Entity Identificator.</param>
        Task DeleteAsync(Guid eventId);
    }
}

