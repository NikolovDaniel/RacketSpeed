using RacketSpeed.Core.Models.Player;

namespace RacketSpeed.Core.Contracts
{
    /// <summary>
    /// Service class for Player Entity.
    /// </summary>
    public interface IPlayerService
    {
        /// <summary>
        /// Async method to retrieve all Players.
        /// </summary>
        /// <returns>Collection of all Players.</returns>
        Task<ICollection<PlayerViewModel>> AllAsync();

        /// <summary>
        /// Async method to retrieve a single Player.
        /// </summary>
        /// <param name="id">Entity Identificator.</param>
        /// <returns>PlayerDetailsViewModel.</returns>
        Task<PlayerFormModel> GetByIdAsync(Guid id);

        /// <summary>
        /// Async method to add an Entity to the Database.
        /// </summary>
        /// <param name="model">Player View Model.</param>
        Task AddAsync(PlayerFormModel model);

        /// <summary>
        /// Async method to Edit an Entity.
        /// </summary>
        /// <param name="model">Player View Model.</param>
        Task EditAsync(PlayerFormModel model);

        /// <summary>
        /// Async method to Delete an Entity.
        /// </summary>
        /// <param name="id">Entity Identificator.</param>
        Task DeleteAsync(Guid id);
    }
}

