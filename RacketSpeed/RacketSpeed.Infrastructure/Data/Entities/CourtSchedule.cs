using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RacketSpeed.Infrastructure.Data.Entities
{
    /// <summary>
    /// Mapping table for Court and Schedule Entities.
    /// </summary>
    public class CourtSchedule
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
        /// Navigation property for Court entity.
        /// </summary>
        public Court Court { get; set; } = null!;

        /// <summary>
        /// Foreign key for Schedule Entity.
        /// </summary>
        [ForeignKey(nameof(Schedule))]
        public Guid ScheduleId { get; set; }

        /// <summary>
        /// Navigation property for Schedule entity.
        /// </summary>
        public Schedule Schedule { get; set; } = null!;
    }
}

