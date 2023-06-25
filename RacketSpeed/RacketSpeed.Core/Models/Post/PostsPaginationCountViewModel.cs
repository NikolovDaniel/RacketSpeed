namespace RacketSpeed.Core.Models.Post
{
    /// <summary>
    /// Class to hold all Post Entities and the count of pages needed.
    /// </summary>
	public class PostsPaginationCountViewModel
	{
        /// <summary>
        /// Page number.
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// Collection of PostViewModel Entities.
        /// </summary>
        public IEnumerable<PostViewModel> Posts { get; set; } = null!;

    }
}

