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
        /// <param name="start">Page number.</param>
        /// <param name="postsPerPage">Post Entities per page.</param>
        /// <returns>Collection of PostViewModel.</returns>
        Task<ICollection<PostViewModel>> AllAsync(int start, int postsPerPage);

        /// <summary>
        /// Async method to retrieve all Posts with expression.
        /// </summary>
        /// <param name="start">Page number.</param>
        /// <param name="postsPerPage">Post Entities per page.</param>
        /// <returns>Collection of PostViewModel.</returns>
        Task<ICollection<PostViewModel>> AllAsync(int start, int postsPerPage, string keyword);

        /// <summary>
        /// Async method to retrieve a single Post.
        /// </summary>
        /// <param name="id">Entity Identificator.</param>
        /// <returns>PostViewModel.</returns>
        Task<PostViewModel> GetByIdAsync(Guid id);

        /// <summary>
        /// Async method to add an Entity to the Database.
        /// </summary>
        /// <param name="model">Post Form Model.</param>
        Task AddAsync(PostFormModel model);

        /// <summary>
        /// Async method to Edit an Entity.
        /// </summary>
        /// <param name="model">Post Form Model.</param>
        Task EditAsync(PostFormModel model);

        /// <summary>
        /// Async method to Delete an Entity.
        /// </summary>
        /// <param name="id">Entity Identificator.</param>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Method to get the Count of all Post Entities and help for the pagination.
        /// </summary>
        /// <param name="postsPerPage">Post count for pagination.</param>
        /// <returns>Integer for Page number.</returns>
        public int PostsPageCount(int postsPerPage);
    }
}

