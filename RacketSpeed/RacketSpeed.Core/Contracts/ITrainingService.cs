using RacketSpeed.Core.Models.Post;
using RacketSpeed.Core.Models.Training;

namespace RacketSpeed.Core.Contracts
{
    /// <summary>
    /// Service class for Training Entity.
    /// </summary>
    public interface ITrainingService
    {
        /// <summary>
        /// Async method to retrieve all Trainings.
        /// </summary>
        /// <returns>Collection of TrainingViewModel.</returns>
        Task<ICollection<TrainingViewModel>> AllAsync();

        /// <summary>
        /// Async method to retrieve all Trainings filtered by training name.
        /// </summary>
        /// <returns>Collection of TrainingViewModel.</returns>
        Task<ICollection<TrainingViewModel>> AllAsync(string trainingName);

        /// <summary>
        /// Async method to retrieve a single Training.
        /// </summary>
        /// <param name="trainingId">Entity Identificator.</param>
        /// <returns>TrainingViewModel.</returns>
        Task<TrainingFormModel> GetByIdAsync(Guid trainingId);

        /// <summary>
        /// Async method to add an Entity to the Database.
        /// </summary>
        /// <param name="model">Training Form Model.</param>
        Task AddAsync(TrainingFormModel model);

        /// <summary>
        /// Async method to Edit an Entity.
        /// </summary>
        /// <param name="model">Training Form Model.</param>
        Task EditAsync(TrainingFormModel model);

        /// <summary>
        /// Async method to Delete an Entity.
        /// </summary>
        /// <param name="trainingId">Entity Identificator.</param>
        Task DeleteAsync(Guid trainingId);
    }
}

