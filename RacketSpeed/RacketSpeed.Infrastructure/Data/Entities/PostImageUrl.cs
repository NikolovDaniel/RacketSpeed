using System;
namespace RacketSpeed.Infrastructure.Data.Entities
{
	/// <summary>
	/// Mapping class for Post and ImageUrl.
	/// </summary>
	public class PostImageUrl
	{
		/// <summary>
		/// Identificator for Post.
		/// </summary>
		public Guid PostId { get; set; }

		/// <summary>
		/// Navigation property for Post.
		/// </summary>
		public Post Post { get; set; } = null!;

		/// <summary>
		/// Identificator for ImageUrl.
		/// </summary>
		public Guid ImageUrlId { get; set; }

		/// <summary>
		/// Navigation property for ImageUrl.
		/// </summary>
		public ImageUrl ImageUrl { get; set; } = null!;
	}
}

