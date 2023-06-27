using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RacketSpeed.Infrastructure.Data.Entities
{
    /// <summary>
    /// EventImageUrl Entity.
    /// </summary>
    public class EventImageUrl
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
        /// Foreign key for Event.
        /// </summary>
        [ForeignKey(nameof(Event))]
        public Guid EventId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        public Event Event { get; set; } = null!;
    }
}