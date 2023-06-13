using System;
using RacketSpeed.Core.Models.Post;
namespace RacketSpeed.Core.Contracts
{
    /// <summary>
    /// Service class for Post Entity.
    /// </summary>
    public interface IPostService
    {
        /// <summary>
        /// Async method to retrieve all Posts.
        /// </summary>
        /// <returns>Collection of all Posts.</returns>
        Task<ICollection<PostViewModel>> AllAsync();

        /// <summary>
        /// Async method to retrieve a single Post.
        /// </summary>
        /// <param name="id">Entity Identificator.</param>
        /// <returns>PostViewModel.</returns>
        Task<PostViewModel> GetByIdAsync(Guid id);

        /// <summary>
        /// Async method to add an Entity to the Database.
        /// </summary>
        /// <param name="model">Post View Model.</param>
        Task AddAsync(PostViewModel model);

        /// <summary>
        /// Async method to Edit an Entity.
        /// </summary>
        /// <param name="model">Post View Model.</param>
        Task EditAsync(PostViewModel model);

        /// <summary>
        /// Async method to Delete an Entity.
        /// </summary>
        /// <param name="id">Entity Identificator.</param>
        Task DeleteAsync(Guid id);
    }
}

