using RacketSpeed.Core.Models.Booking;
using RacketSpeed.Infrastructure.Data.Entities;

namespace RacketSpeed.Core.Contracts
{
    /// <summary>
    /// Service class for Booking Entity.
    /// </summary>
    public interface IBookingService
    {
        /// <summary>
        /// Async method to retrieve all Bookings for specific user.
        /// </summary>
        /// <param name="userId">User identificator..</param>
        /// <returns>Collection of BookingUserViewModel.</returns>
        Task<ICollection<BookingUserViewModel>> UserBookingsAsync(string userId);

        /// <summary>
        /// Async method to retrieve all Bookings.
        /// </summary>
        /// <param name="start">Page number.</param>
        /// <param name="bookingsPerPage">Booking Entities per page.</param>
        /// <returns>Collection of BookingViewModel.</returns>
        Task<ICollection<BookingViewModel>> AllAsync(int start, int bookingsPerPage);

        /// <summary>
        /// Async method to retrieve all Reservations with expression.
        /// </summary>
        /// <param name="start">Page number.</param>
        /// <param name="bookingsPerPage">Reservation Entities per page.</param>
        /// <param name="keyword">Keyword for filtering purpose.</param>
        /// <returns>Collection of BookingViewModel.</returns>
        Task<ICollection<BookingViewModel>> AllAsync(int start, int bookingsPerPage, string keyword);

        /// <summary>
        /// Async method to retrieve all Bookings for specific date.
        /// </summary>
        /// <returns>Collection of BookingViewModel.</returns>
        Task<ICollection<BookingViewModel>> TodayBookingsAsync();

        /// <summary>
        /// Async method to add an Entity to the Database.
        /// </summary>
        /// <param name="model">Booking Form Model.</param>
        Task AddAsync(BookingFormModel model);

        /// <summary>
        /// Async method to change the status of a booking.
        /// </summary>
        /// <param name="bookingId">Booking identificator.</param>
        /// <param name="userId">User identificator.</param>
        /// <param name="status">Status of the booking.</param>
        Task ChangeStatusAsync(Guid bookingId, string userId, string status);

        /// <summary>
        /// Method to get the Count of all Booking Entities and help for the pagination.
        /// </summary>
        /// <param name="bookingsPerPage">Booking count for pagination.</param>
        /// <returns>Integer for Bookings number.</returns>
        public int BookingsPageCount(int bookingsPerPage);

        /// <summary>
        /// Async method to get all available hours on particular date for the court. 
        /// </summary>
        /// <param name="date">Date for the reservation.</param>
        /// <returns>List with available hours.</returns>
        Task<List<Schedule>> GetAvailableHoursAsync(DateTime date, int courtNumber);

        /// <summary>
        /// Async method to get all available courts.
        /// </summary>
        /// <returns>Collection of Court.</returns>
        Task<IEnumerable<Court>> GetAllCourtsAsync();
    }
}

