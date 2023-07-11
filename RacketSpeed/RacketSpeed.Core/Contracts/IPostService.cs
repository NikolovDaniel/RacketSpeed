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
        /// <param name="keyword">Keyword for filtering purpose.</param>
        /// <returns>Collection of PostViewModel.</returns>
        Task<ICollection<PostViewModel>> AllAsync(int start, int postsPerPage, string keyword);

        /// <summary>
        /// Async method to retrieve a single Post.
        /// </summary>
        /// <param name="postId">Entity Identificator.</param>
        /// <returns>PostViewModel.</returns>
        Task<PostViewModel> GetByIdAsync(Guid postId);

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
        /// <param name="postId">Entity Identificator.</param>
        Task DeleteAsync(Guid postId);

        /// <summary>
        /// Method to get the Count of all Post Entities with certain keyword and help for the pagination.
        /// </summary>
        /// <param name="postsPerPage">Post count for pagination.</param>
        /// <param name="keyword">Keyword used to filter the posts.</param>
        /// <returns>Integer for Page number.</returns>
        public int PostsPageCount(int postsPerPage, string keyword);

        /// <summary>
        /// Method to get the Count of all Post Entities and help for the pagination.
        /// </summary>
        /// <param name="postsPerPage">Post count for pagination.</param>
        /// <returns>Integer for Page number.</returns>
        public int PostsPageCount(int postsPerPage);
    }
}

