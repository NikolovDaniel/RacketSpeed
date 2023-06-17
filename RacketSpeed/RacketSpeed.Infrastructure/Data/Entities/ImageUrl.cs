using System.ComponentModel.DataAnnotations;

namespace RacketSpeed.Infrastructure.Data.Entities
{
    /// <summary>
    /// ImageUrl Entity.
    /// </summary>
    public class ImageUrl
    {
        /// <summary>
        /// Identificator.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Url for image resource.
        /// </summary>
        [Required]
        public string Url { get; set; } = null!;
        /// <summary>
        /// Collection of PostImageUrl.
        /// </summary>
        public ICollection<PostImageUrl> PostImageUrls { get; set; } = null!;
    }
}