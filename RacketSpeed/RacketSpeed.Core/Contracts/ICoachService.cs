using RacketSpeed.Core.Models.Coach;

namespace RacketSpeed.Core.Contracts
{
    /// <summary>
    /// Service class for Coach Entity.
    /// </summary>
    public interface ICoachService
    {
        /// <summary>
        /// Async method to retrieve all Coaches.
        /// </summary>
        /// <returns>Collection of all Coaches.</returns>
        Task<ICollection<CoachViewModel>> AllAsync();

        /// <summary>
        /// Async method to retrieve a single Coach with its trainings.
        /// </summary>
        /// <param name="id">Entity Identificator.</param>
        /// <returns>CoachViewModel.</returns>
        Task<CoachTrainingsViewModel> GetByIdAsync(Guid id, bool withTrainings);

        /// <summary>
        /// Async method to add an Entity to the Database.
        /// </summary>
        /// <param name="model">Coach Form Model.</param>
        Task AddAsync(CoachFormModel model);

        /// <summary>
        /// Async method to Edit an Entity.
        /// </summary>
        /// <param name="model">Coach Form Model.</param>
        Task EditAsync(CoachFormModel model);

        /// <summary>
        /// Async method to Delete an Entity.
        /// </summary>
        /// <param name="id">Entity Identificator.</param>
        Task DeleteAsync(Guid id);
    }
}

