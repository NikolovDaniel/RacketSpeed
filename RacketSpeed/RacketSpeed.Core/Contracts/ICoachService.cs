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
        /// <param name="coachId">Entity Identificator.</param>
        /// <returns>CoachTrainingsViewModel.</returns>
        Task<CoachTrainingsViewModel> GetByIdAsync(Guid coachId, bool withTrainings);

        /// <summary>
        /// Async method to retrieve a single Coach without trainings.
        /// </summary>
        /// <param name="coachId">Entity Identificator.</param>
        /// <returns>CoachViewModel.</returns>
        Task<CoachFormModel> GetByIdAsync(Guid coachId);

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
        /// <param name="coachId">Entity Identificator.</param>
        Task DeleteAsync(Guid coachId);

        /// <summary>
        /// Method to check if there is a duplicate training or training that starts before another ends.
        /// </summary>
        /// <param name="coachId">Coach Entity identificator.</param>
        /// <param name="dayOfWeek">The training's day.</param>
        /// <param name="start">Start hour.</param>
        /// <returns>Boolean whether there is a training at that time or not.</returns>
        bool HasTraining(Guid coachId, string dayOfWeek, DateTime start);

    }
}

