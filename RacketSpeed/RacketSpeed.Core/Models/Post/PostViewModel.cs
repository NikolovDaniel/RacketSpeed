using System;
using RacketSpeed.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations;

namespace RacketSpeed.Core.Models.Post
{
    /// <summary>
    /// Post view model for presenting.
    /// </summary>
    public class PostViewModel
    {
        /// <summary>
        /// Identificator.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Title of the post.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// Content of the post.
        /// </summary>
        public string Content { get; set; } = null!;

        /// <summary>
        /// Collection of ImageUrl.
        /// </summary>
        public string[] ImageUrls { get; set; } = null!;
    }
}
