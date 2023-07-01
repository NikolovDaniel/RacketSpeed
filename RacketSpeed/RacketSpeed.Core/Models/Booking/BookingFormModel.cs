using RacketSpeed.Infrastructure.Data.Entities;
using RacketSpeed.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations;

namespace RacketSpeed.Core.Models.Booking
{
    /// <summary>
    /// Booking form model, holds validation for CRUD operations.
    /// </summary>
    public class BookingFormModel
    {
        /// <summary>
        /// Identificator.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Number of the court.
        /// </summary>
        [Required]
        [Range(DataConstants.ReservationCourtNumberMinValue, DataConstants.ReservationCourtNumberMaxValue,
            ErrorMessage = DataConstants.ReservationCourtNumberErrorMessage)]
        public int CourtNumber { get; set; }

        /// <summary>
        /// Foreign key for ApplicationUser Entity.
        /// </summary>
        [Required(ErrorMessage = DataConstants.RequiredFieldErrorMessage)]
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Phone number of the user.
        /// </summary>
        [Required(ErrorMessage = DataConstants.RequiredFieldErrorMessage)]
        [RegularExpression("^\\+?[1-9][0-9]{7,14}$",
            ErrorMessage = DataConstants.SignKidPhoneNumberErrorMessage)]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = DataConstants.RequiredFieldErrorMessage)]
        [StringLength(DataConstants.SignKidFullNameMaxLength,
            MinimumLength = DataConstants.SignKidFullNameMinLength,
            ErrorMessage = DataConstants.SignKidFullNameErrorMessage)]
        public string FullName { get; set; } = null!;

        /// <summary>
        /// Count of people in the game session.
        /// </summary>
        [Required(ErrorMessage = DataConstants.RequiredFieldErrorMessage)]
        [Range(DataConstants.ReservationPeopleCountMinValue, DataConstants.ReservationPeopleCountMaxValue,
            ErrorMessage = DataConstants.ReservationPeopleCountErrorMessage)]
        public int PeopleCount { get; set; }

        /// <summary>
        /// Count of rackets booked for the session.
        /// </summary>
        [Required(ErrorMessage = DataConstants.RequiredFieldErrorMessage)]
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
        [Required(ErrorMessage = DataConstants.RequiredFieldErrorMessage)]
        public DateTime Date { get; set; }

        /// <summary>
        /// The hour of the reservation.
        /// </summary>
        [Required(ErrorMessage = DataConstants.RequiredFieldErrorMessage)]
        public TimeSpan Hour { get; set; }

        /// <summary>
        /// Status of the reservation. Might be better if it is enum.
        /// </summary>
        public string? Status { get; set; } = null!;

        /// <summary>
        /// Location of where the training will be held.
        /// </summary>
        public string Location { get; set; } = null!;

        /// <summary>
        /// The price of the reservation.
        /// </summary>
        [Required(ErrorMessage = DataConstants.RequiredFieldErrorMessage)]
        public decimal ReservationTotalSum { get; set; }

        /// <summary>
        /// Collection to hold the available courts.
        /// </summary>
        public IEnumerable<Court> Courts { get; set; } = new List<Court>();
    }
}

