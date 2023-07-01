using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RacketSpeed.Infrastructure.Utilities;

namespace RacketSpeed.Infrastructure.Data.Entities
{
	/// <summary>
	/// Reservation Entity.
	/// </summary>
	public class Reservation
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
		[Required]
		public Guid CourtId { get; set; }

		/// <summary>
		/// Navigation property for Court Entity.
		/// </summary>
		public Court Court { get; set; } = null!;

		/// <summary>
		/// Foreign key for ApplicationUser Entity.
		/// </summary>
		[ForeignKey(nameof(User))]
		[Required]
		public string UserId { get; set; } = null!;

        /// <summary>
        /// Navigation property for ApplicationUser Entity.
        /// </summary>
        public ApplicationUser User { get; set; } = null!;

		/// <summary>
		/// Count of people in the game session.
		/// </summary>
		[Required]
		[Range(DataConstants.ReservationPeopleCountMinValue, DataConstants.ReservationPeopleCountMaxValue,
			ErrorMessage = DataConstants.ReservationPeopleCountErrorMessage)]
		public int PeopleCount { get; set; }

		/// <summary>
		/// Count of rackets booked for the session.
		/// </summary>
		[Required]
		[Range(DataConstants.ReservationRacketsBookedMinValue, DataConstants.ReservationRacketsBookedMaxValue,
			ErrorMessage = DataConstants.ReservationRacketsBookedErrorMessage)]
		public int RacketsBooked { get; set; }

		/// <summary>
		/// When the reservation is created.
		/// </summary>
	    public DateTime CreatedOn { get; set; }

		/// <summary>
		/// The reservation date.
		/// </summary>
		[Required]
		public DateTime Date { get; set; }

		/// <summary>
		/// The hour of the reservation.
		/// </summary>
		[Required]
		public TimeSpan Hour { get; set; }

		/// <summary>
		/// Location of where the training will be held.
		/// </summary>
		[Required]
		[StringLength(DataConstants.ReservationLocationMaxValue,
			MinimumLength = DataConstants.ReservationLocationMinValue,
			ErrorMessage = DataConstants.ReservationLocationErrorMessage)]
		public string Location { get; set; } = null!;

		/// <summary>
		/// Status of the reservation. Might be better if it is enum.
		/// </summary>
		[Required]
		public string Status { get; set; } = null!;

        /// <summary>
        /// Phone number of the user.
        /// </summary>
        [Required(ErrorMessage = DataConstants.RequiredFieldErrorMessage)]
        [RegularExpression("^\\+?[1-9][0-9]{7,14}$",
            ErrorMessage = DataConstants.SignKidPhoneNumberErrorMessage)]
        public string PhoneNumber { get; set; } = null!;

        /// <summary>
        /// The price of the reservation.
        /// </summary>
        [Required]
		public decimal ReservationTotalSum { get; set; }

		/// <summary>
		/// Delete flag.
		/// </summary>
		public bool IsDeleted { get; set; }
	}
}

