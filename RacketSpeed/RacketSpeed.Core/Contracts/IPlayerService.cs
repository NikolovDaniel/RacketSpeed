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
        /// <param name="start">Page number.</param>
        /// <param name="playersPerPage">Player Entities per page.</param>
        Task<ICollection<PlayerViewModel>> AllAsync(int start, int playersPerPage);

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

        /// <summary>
        /// Method to get the Count of all Player Entities and help for the pagination.
        /// </summary>
        /// <param name="playersPerPage">Player count for pagination.</param>
        /// <returns>Integer for Page number.</returns>
        public int PlayersPageCount(int playersPerPage);
    }
}

