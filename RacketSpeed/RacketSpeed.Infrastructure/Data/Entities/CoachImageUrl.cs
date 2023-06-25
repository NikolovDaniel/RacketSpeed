using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RacketSpeed.Infrastructure.Data.Entities
{
    /// <summary>
    /// CoachImageUrl Entity.
    /// </summary>
    public class CoachImageUrl
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
        /// Foreign key for Coach.
        /// </summary>
        [ForeignKey(nameof(Coach))]
        public Guid CoachId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        public Coach Coach { get; set; } = null!;
    }
}

