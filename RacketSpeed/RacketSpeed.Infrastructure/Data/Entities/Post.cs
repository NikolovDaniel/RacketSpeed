using System;
using System.ComponentModel.DataAnnotations;
using RacketSpeed.Infrastructure.Utilities;

namespace RacketSpeed.Infrastructure.Data.Entities
{
	/// <summary>
	/// Post.
	/// </summary>
	public class Post
	{
		/// <summary>
		/// Identificator.
		/// </summary>
		[Key]
		public Guid Id { get; set; }

		/// <summary>
		/// Title of the post.
		/// </summary>
		[Required]
		[StringLength(DataConstants.PostTitleMaxLength,
			MinimumLength = DataConstants.PostTitleMinLength,
			ErrorMessage = DataConstants.PostTitleErrorMessage)]
		public string Title { get; set; } = null!;

		/// <summary>
		/// Description of the post.
		/// </summary>
		[Required]
        [StringLength(DataConstants.PostContentMaxLength,
            MinimumLength = DataConstants.PostContentMinLength,
            ErrorMessage = DataConstants.PostContentErrorMessage)]
        public string Content { get; set; } = null!;

		/// <summary>
		/// Delete flag.
		/// </summary>
		public bool IsDeleted { get; set; }
	}
}

