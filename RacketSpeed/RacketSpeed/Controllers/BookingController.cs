using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Booking;
using RacketSpeed.Infrastructure.Data.Entities;


namespace RacketSpeed.Controllers
{
    /// <summary>
    /// Provides functionality to the /Booking/ route.
    /// </summary>
    [Authorize]
    public class BookingController : Controller
    {
        /// <summary>
        /// Booking service.
        /// </summary>
        private readonly IBookingService bookingService;
        UserManager<ApplicationUser> _userManager;


        /// <summary>
        /// DI Booking service.
        /// </summary>
        /// <param name="bookingService">IBookingService.</param>
        public BookingController(IBookingService bookingService, UserManager<ApplicationUser> userManager)
        {
            this.bookingService = bookingService;
            this._userManager = userManager;
        }


        /// <summary>
        /// Displays /Booking/Book page.
        /// </summary>
        /// <returns>/Booking/Book page</returns>
        [HttpGet]
        public async Task<IActionResult> Book()
        {
            var courts = await this.bookingService.GetAllCourtsAsync();

            var model = new BookingFormModel()
            {
                Courts = courts
            };

            return View(model);
        }

        /// <summary>
        /// Gets the data from a HttpPost request and proccesses it.
        /// </summary>
        /// <param name="model">BookingFormModel.</param>
        /// <returns>/Booking/UserBooking Page.</returns>
        [HttpPost]
        public async Task<IActionResult> Book(BookingFormModel model)
        {
            if (!ModelState.IsValid)
            {
                var courts = await this.bookingService.GetAllCourtsAsync();

                model.Courts = courts;

                return View(model);
            }

            var totalPrice = (model.PeopleCount * 5) + (model.RacketsBooked * 2);

            if (model.ReservationTotalSum != totalPrice)
            {
                ModelState.AddModelError("InvalidTotalPrice", "Цената не отговаря на истинската.");
                return View(model);
            }

            var currentHour = DateTime.Now.Hour;
            var currentDate = DateTime.Now.Date;

            if (currentDate == model.Date.Date && model.Hour.Hours <= currentHour)
            {
                ModelState.AddModelError("InvalidHour", "Часа на резервация трябва да бъде най-рано 1 час от текущия.");
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user.Deposit != 30)
            {
                ModelState.AddModelError("NoDeposit", "За да запазите корт Ви е нужен депозит на стойност 30 лева.");
                return View(model);
            }

            user.Deposit = 0;
            await this.bookingService.AddAsync(model);

            return RedirectToAction("UserBookings", "Booking", new { userId = model.UserId });
        }

        /// <summary>
        /// Returns JSON with the available hours for particular court.
        /// </summary>
        /// <param name="date">Date of the reservation.</param>
        /// <param name="courtNumber">Number of the court.</param>
        /// <returns>JSON array with available hours.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAvailableHours(string date, int courtNumber)
        {
            var availableHoursByCourt
                = await this.bookingService.GetAvailableHoursAsync(DateTime.Parse(date), courtNumber);

            var result = availableHoursByCourt
                .Select(s => new { hour = s.Hour.ToString(@"hh\:mm") });

            return Json(result);
        }

        // Action to return bookings for the employee for today.
        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> TodayBookings()
        {
            var bookings = await this.bookingService.TodayBookingsAsync();

            return View(bookings);
        }

        // Action to retrieve the last 5 user bookings for the user.
        public async Task<IActionResult> UserBookings(string userId)
        {
            var userBookings = await this.bookingService.UserBookingsAsync(userId);

            return View(userBookings);
        }

        // Action to get all bookings for the administrator + pagination.
        public IActionResult AllBookings()
        {

            return View();
        }

        // Action to change the status of the booking and return the customer's money if its approved.
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> ChangeBookingStatus(Guid bookingId, string userId, string status)
        {
            await this.bookingService.ChangeStatusAsync(bookingId, userId, status);

            return RedirectToAction("TodayBookings", "Booking");
        }
    }
}

