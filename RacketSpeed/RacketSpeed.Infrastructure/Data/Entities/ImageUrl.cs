using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        /// Foreign key for Post.
        /// </summary>
        [ForeignKey(nameof(Post))]
        public Guid PostId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        public Post Post { get; set; } = null!;
    }
}