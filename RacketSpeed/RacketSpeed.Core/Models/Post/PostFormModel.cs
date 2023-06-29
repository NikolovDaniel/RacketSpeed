using System.ComponentModel.DataAnnotations;
using RacketSpeed.Infrastructure.Utilities;

namespace RacketSpeed.Core.Models.Post
{
    /// <summary>
    /// Post form model, holds validation for CRUD operations.
    /// </summary>
    public class PostFormModel
	{
        /// <summary>
        /// Identificator.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Title of the post.
        /// </summary>
        [Required(ErrorMessage = DataConstants.RequiredFieldErrorMessage)]
        [StringLength(DataConstants.PostTitleMaxLength,
            MinimumLength = DataConstants.PostTitleMinLength,
            ErrorMessage = DataConstants.PostTitleErrorMessage)]
        public string Title { get; set; } = null!;

        /// <summary>
        /// Description of the post.
        /// </summary>
        [Required(ErrorMessage = DataConstants.RequiredFieldErrorMessage)]
        [StringLength(DataConstants.PostContentMaxLength,
            MinimumLength = DataConstants.PostContentMinLength,
            ErrorMessage = DataConstants.PostContentErrorMessage)]
        public string Content { get; set; } = null!;

        /// <summary>
        /// Collection of image urls.
        /// </summary>
        public string[] ImageUrls { get; set; }
    }
}

