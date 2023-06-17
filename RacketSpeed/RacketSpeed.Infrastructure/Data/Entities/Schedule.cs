using System;
using System.ComponentModel.DataAnnotations;

namespace RacketSpeed.Infrastructure.Data.Entities
{
	/// <summary>
	/// Schedule Entity.
	/// </summary>
	public class Schedule
	{
		/// <summary>
		/// Identificator.
		/// </summary>
		[Key]
		public Guid Id { get; set; }

		/// <summary>
		/// Date and hour for the booking.
		/// </summary>
		[Required]
		public DateTime Date { get; set; }

		/// <summary>
		/// Delete flag.
		/// </summary>
		public bool IsDeleted { get; set; }

        /// <summary>
        /// Mapping property.
        /// </summary>
        public IEnumerable<CourtSchedule> CourtSchedules { get; set; } = null!;
    }
}

