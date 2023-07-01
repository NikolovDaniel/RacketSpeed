namespace RacketSpeed.Core.Models.Booking
{
    public class BookingUserViewModel
    {
        /// <summary>
        /// Number of the court.
        /// </summary>
        public int CourtNumber { get; set; }

        /// <summary>
        /// The name of the user who made the reservation.
        public string UserBookingName { get; set; } = null!;

        /// <summary>
        /// Count of people in the game session.
        /// </summary>
        public int PeopleCount { get; set; }

        /// <summary>
        /// Count of rackets booked for the session.
        /// </summary>
        public int RacketsBooked { get; set; }

        /// <summary>
        /// When the reservation is created.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// The reservation date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The hour of the reservation.
        /// </summary>
        public TimeSpan Hour { get; set; }

        /// <summary>
        /// Status of the reservation. Might be better if it is enum.
        /// </summary>
        public string Status { get; set; } = null!;

        /// <summary>
        /// Phone number of the user.
        /// </summary>
        public string PhoneNumber { get; set; } = null!;

        /// <summary>
        /// Location of where the training will be held.
        /// </summary>
        public string Location { get; set; } = null!;

        /// <summary>
        /// The price of the reservation.
        /// </summary>
        public decimal ReservationTotalSum { get; set; }
    }
}

