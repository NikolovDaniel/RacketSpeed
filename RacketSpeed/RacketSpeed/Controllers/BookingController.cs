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
        private readonly UserManager<ApplicationUser> _userManager;


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
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Book(BookingFormModel model)
        {
            // later on, validation for location might be needed
            
            if (!ModelState.IsValid || model.Date.Date < DateTime.Now.Date)
            {
                var courts = await this.bookingService.GetAllCourtsAsync();

                model.Courts = courts;

                return View(model);
            }

            var totalPrice = (model.PeopleCount * 5) + (model.RacketsBooked * 2);

            if (model.ReservationTotalSum != totalPrice)
            {
                ModelState.AddModelError("InvalidTotalPrice", "Цената не отговаря на истинската.");
                model.ReservationTotalSum = totalPrice;
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

            if (user.Deposit < 30)
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

        /// <summary>
        /// Display a /Booking/TodayBookings page for the employee.
        /// </summary>
        /// <returns>/Booking/TodayBookings page.</returns>
        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> TodayBookings()
        {
            if(!User.IsInRole("Employee"))
            {
                return Unauthorized();
            }

            var bookings = await this.bookingService.TodayBookingsAsync();

            return View(bookings);
        }

        /// <summary>
        /// Display a /Booking/UserBookings page with the current user's reservations.
        /// </summary>
        /// <param name="userId">User identificator.</param>
        /// <returns>/Booking/UserBookings page.</returns>
        [HttpGet]
        public async Task<IActionResult> UserBookings(string userId)
        {
            var userBookings = await this.bookingService.UserBookingsAsync(userId);

            return View(userBookings);
        }

        /// <summary>
        /// Displays a /Booking/BookingsByKeyword page with 8 reservations by phone number.
        /// </summary>
        /// <returns>/Booking/BookingsByKeyword/{keyword} page.</returns>
        /// <param name="phoneNumber">Phone number used to filter Reservation Entities.</param>
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> BookingsByKeyword(string phoneNumber, string pageCount = "1")
        {
            if (!User.IsInRole("Administrator"))
            {
                return Unauthorized();
            }

            if (string.IsNullOrEmpty(phoneNumber))
            {
                ModelState.AddModelError("KeywordError", "Полето трябва да съдържа поне 1 символ.");
                return View();
            }

            int pageNum = int.Parse(pageCount);

            int bookingsPerPage = 8;

            var pagesCount = this.bookingService.BookingsPageCount(bookingsPerPage);

            pageNum = CalculateValidPageNum(pageNum, pagesCount);

            var bookings = await bookingService.AllAsync(pageNum, bookingsPerPage, phoneNumber);

            ViewData["phoneNumber"] = phoneNumber;
            ViewData["pageNum"] = pageNum;

            return View(new BookingsPaginationCountViewModel()
            {
                Bookings = bookings,
                PageCount = pagesCount
            });
        }

        /// <summary>
        /// Displays a /News/All page with three posts.
        /// </summary>
        /// <returns>/News/All page.</returns>
        /// <param name="pageCount">Int for page index.</param>
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AllBookings(string pageCount = "1")
        {
            if (!User.IsInRole("Administrator"))
            {
                return Unauthorized();
            }

            int pageNum = int.Parse(pageCount);

            int bookingsPerPage = 8;

            var pagesCount = this.bookingService.BookingsPageCount(bookingsPerPage);

            pageNum = CalculateValidPageNum(pageNum, pagesCount);
            ViewData["pageNum"] = pageNum;

            var bookings = await bookingService.AllAsync(pageNum, bookingsPerPage);

            return View(new BookingsPaginationCountViewModel()
            {
                Bookings = bookings,
                PageCount = pagesCount
            });
        }

        /// <summary>
        /// Changes the Booking status.
        /// </summary>
        /// <param name="bookingId">Booking identificator.</param>
        /// <param name="userId">User identificator.</param>
        /// <param name="status">Status of the booking.</param>
        /// <returns>/Booking/TodayBookings page.</returns>
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> ChangeBookingStatus(Guid bookingId, string userId, string status)
        {
            if (!User.IsInRole("Employee"))
            {
                return Unauthorized();
            }

            await this.bookingService.ChangeStatusAsync(bookingId, userId, status);

            return RedirectToAction("TodayBookings", "Booking");
        }

        /// <summary>
        /// Method to calculate the right page number.
        /// </summary>
        /// <param name="pageNum">Page number that the user wants.</param>
        /// <param name="pagesCount">All pages available.</param>
        /// <returns>Integer which is the correct page number.</returns>
        private int CalculateValidPageNum(int pageNum, int pagesCount)
        {
            if (pageNum <= 0)
            {
                return 1;
            }

            if (pageNum > pagesCount)
            {
                return pagesCount;
            }

            return pageNum;
        }
    }
}

