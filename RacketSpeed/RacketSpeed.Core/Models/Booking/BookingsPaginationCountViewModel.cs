namespace RacketSpeed.Core.Models.Booking
{
    /// <summary>
    /// Class to hold all Reservation Entities and the count of pages needed.
    /// </summary>
    public class BookingsPaginationCountViewModel
    {
        /// <summary>
        /// Page number.
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// Collection of BookingViewModel Entities.
        /// </summary>
        public IEnumerable<BookingViewModel> Bookings { get; set; } = null!;
    }
}

