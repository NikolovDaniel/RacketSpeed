using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RacketSpeed.Infrastructure.Data.Entities
{
    /// <summary>
    /// Availability Entity used to retrieve the free courts.
    /// </summary>
    public class Availability
    {
        /// <summary>
        /// Identificator.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Foreign key for Court Entity.
        /// </summary>
        [ForeignKey(nameof(Court))]
        public Guid CourtId { get; set; }

        /// <summary>
        /// Navigation property.
        /// </summary>
        public Court Court { get; set; } = null!;

        /// <summary>
        /// Date to check if the court is free.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Start hour to check if the court is free for that hour.
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// End hour for the training session.
        /// </summary>
        public TimeSpan EndTime { get; set; }
    }
}

