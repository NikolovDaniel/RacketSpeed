using RacketSpeed.Core.Models.Achievement;

namespace RacketSpeed.Core.Contracts
{
    /// <summary>
    /// Service class for Achievement Entity.
    /// </summary>
    public interface IAchievementService
    {
        /// <summary>
        /// Async method to retrieve all Achievements.
        /// </summary>
        /// <returns>Collection of all Achievements.</returns>
        Task<ICollection<AchievementViewModel>> AllAsync();

        /// <summary>
        /// Async method to retrieve a single Achievement.
        /// </summary>
        /// <param name="id">Entity Identificator.</param>
        /// <returns>AchievementViewModel.</returns>
        Task<AchievementViewModel> GetByIdAsync(Guid id);

        /// <summary>
        /// Async method to add an Entity to the Database.
        /// </summary>
        /// <param name="model">Achievement Form Model.</param>
        Task AddAsync(AchievementFormModel model);

        /// <summary>
        /// Async method to Edit an Entity.
        /// </summary>
        /// <param name="model">Achievement Form Model.</param>
        Task EditAsync(AchievementFormModel model);

        /// <summary>
        /// Async method to Delete an Entity.
        /// </summary>
        /// <param name="id">Entity Identificator.</param>
        Task DeleteAsync(Guid id);
    }
}

