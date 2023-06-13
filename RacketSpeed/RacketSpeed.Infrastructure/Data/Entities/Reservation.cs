using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RacketSpeed.Infrastructure.Data.Entities
{
	/// <summary>
	/// Reservation.
	/// </summary>
	public class Reservation
	{
		/// <summary>
		/// Identificator.
		/// </summary>
		[Key]
		public Guid Id { get; set; }

		/// <summary>
		/// Foreign key for CourtSchedule Entity.
		/// </summary>
		[ForeignKey(nameof(CourtSchedule))]
		public Guid CourtScheduleId { get; set; }

		/// <summary>
		/// Navigation property for CourtSchedule Entity.
		/// </summary>
		public CourtSchedule CourtSchedule { get; set; } = null!;

		/// <summary>
		/// Foreign key for ApplicationUser Entity.
		/// </summary>
		public string UserId { get; set; } = null!;

        /// <summary>
        /// Navigation property for ApplicationUser Entity.
        /// </summary>
        public ApplicationUser User { get; set; } = null!;

		/// <summary>
		/// Count of people in the game session.
		/// </summary>
		public int PeopleCount { get; set; }

		/// <summary>
		/// Count of rackets booked for the session.
		/// </summary>
		public int RacketsBooked { get; set; }

		/// <summary>
		/// Reservation date.
		/// </summary>
	    public DateTime CreatedOn { get; set; }

		/// <summary>
		/// The price of the reservation.
		/// </summary>
		public decimal ReservationTotalSum { get; set; }

		/// <summary>
		/// Delete flag.
		/// </summary>
		public bool IsDeleted { get; set; }
	}
}

