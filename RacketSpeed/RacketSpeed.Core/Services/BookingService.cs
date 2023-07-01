using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Booking;
using RacketSpeed.Infrastructure.Data.Entities;
using RacketSpeed.Infrastructure.Data.Repositories;

namespace RacketSpeed.Core.Services
{
    /// <summary>
    /// Service class for Booking Entity.
    /// </summary>
    public class BookingService : IBookingService
    {
        /// <summary>
        /// Repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// DI repository.
        /// </summary>
        public BookingService(IRepository repository)
        {
            this.repository = repository;
        }


        public async Task AddAsync(BookingFormModel model)
        {
            Expression<Func<Court, bool>> expression
              = c => c.Number == model.CourtNumber;

            var court = await this.repository.All<Court>(expression).FirstAsync();

            var reservation = new Reservation()
            {
                CourtId = court.Id,
                UserId = model.UserId,
                PeopleCount = model.PeopleCount,
                RacketsBooked = model.RacketsBooked,
                ReservationTotalSum = model.ReservationTotalSum,
                Date = model.Date,
                Hour = model.Hour,
                PhoneNumber = model.PhoneNumber,
                Location = model.Location,
                CreatedOn = DateTime.Now,
                Status = "В разработка",
                IsDeleted = false
            };

            await this.repository.AddAsync<Reservation>(reservation);
            await this.repository.SaveChangesAsync();
        }

        public async Task<ICollection<BookingViewModel>> AllAsync(int start, int bookingsPerPage)
        {
            int currPage = start;
            start = (currPage - 1) * bookingsPerPage;

            var allBookings = this.repository
                .AllReadonly<Reservation>()
                .Where(p => p.IsDeleted == false)
                .Skip(start)
                .Take(bookingsPerPage);

            return await allBookings
                .Select(b => new BookingViewModel()
                {
                    Id = b.Id,
                    CourtNumber = b.Court.Number,
                    UserBookingName = $"{b.User.FirstName} {b.User.LastName}",
                    PeopleCount = b.PeopleCount,
                    RacketsBooked = b.RacketsBooked,
                    UserId = b.UserId,
                    CreatedOn = b.CreatedOn,
                    Date = b.Date,
                    Hour = b.Hour,
                    Status = b.Status,
                    PhoneNumber = b.PhoneNumber,
                    Location = b.Location,
                    ReservationTotalSum = b.ReservationTotalSum
                })
                .OrderBy(r => r.Date)
                .ThenBy(r => r.Hour)
                .ToListAsync();
        }

        public async Task<ICollection<BookingViewModel>> AllAsync(int start, int bookingsPerPage, string phoneNumber)
        {
            int currPage = start;
            start = (currPage - 1) * bookingsPerPage;

            Expression<Func<Reservation, bool>> expression
             = r => r.IsDeleted == false && r.PhoneNumber.Contains(phoneNumber);

            var allBookings = this.repository.All<Reservation>(expression);

            return await allBookings
               .Select(b => new BookingViewModel()
               {
                   Id = b.Id,
                   CourtNumber = b.Court.Number,
                   UserBookingName = $"{b.User.FirstName} {b.User.LastName}",
                   PeopleCount = b.PeopleCount,
                   RacketsBooked = b.RacketsBooked,
                   UserId = b.UserId,
                   CreatedOn = b.CreatedOn,
                   Date = b.Date,
                   Hour = b.Hour,
                   Status = b.Status,
                   PhoneNumber = b.PhoneNumber,
                   Location = b.Location,
                   ReservationTotalSum = b.ReservationTotalSum
               })
               .OrderBy(r => r.Date)
               .ThenBy(r => r.Hour)
               .ToListAsync();
        }

        public async Task<ICollection<BookingUserViewModel>> UserBookingsAsync(string userId)
        {
            Expression<Func<Reservation, bool>> expression
                = r => r.UserId == userId;

            int bookingsCount = 5;

            var userBookings = this.repository.AllReadonly<Reservation>(expression)
                .OrderBy(ub => ub.Date)
                .ThenBy(ub => ub.Hour)
                .Take(bookingsCount);

            return await userBookings
               .Select(b => new BookingUserViewModel()
               {
                   CourtNumber = b.Court.Number,
                   UserBookingName = $"{b.User.FirstName} {b.User.LastName}",
                   PeopleCount = b.PeopleCount,
                   RacketsBooked = b.RacketsBooked,
                   Date = b.Date,
                   Hour = b.Hour,
                   Location = b.Location,
                   PhoneNumber = b.PhoneNumber,
                   Status = b.Status,
                   ReservationTotalSum = b.ReservationTotalSum
               })
               .ToListAsync();
        }

        public int BookingsPageCount(int bookingsPerPage)
        {
            int allBookingsCount = this.repository
               .AllReadonly<Reservation>()
               .Where(r => r.IsDeleted == false)
               .Count();

            int pageCount = (int)Math.Ceiling((allBookingsCount / (double)bookingsPerPage));

            return pageCount == 0 ? 1 : pageCount;
        }

        public async Task ChangeStatusAsync(Guid bookingId, string userId, string status)
        {
            var reservation = await this.repository.GetByIdAsync<Reservation>(bookingId);

            if (reservation == null)
            {
                return;
            }

            if (status.ToUpper() == "ОДОБРЕНА")
            {
                var user = await this.repository.GetByIdAsync<ApplicationUser>(userId);

                user.Deposit = 30;
            }

            reservation.Status = status;


            await this.repository.SaveChangesAsync();
        }

        public async Task<List<Schedule>> GetAvailableHoursAsync(DateTime date, int courtNumber)
        {
            Expression<Func<Reservation, bool>> expression
                = r => r.Date.Date == date.Date && r.Court.Number == courtNumber;

            var reservations = this.repository
                .All<Reservation>(expression);


            if (reservations.Count() <= 0 || reservations == null)
            {
                return await this.repository.AllReadonly<Schedule>().ToListAsync();
            }

            return await this.repository.AllReadonly<Schedule>()
                .Where(s => !reservations.Any(r => r.Hour == s.Hour))
                .ToListAsync();
        }

        public async Task<ICollection<BookingViewModel>> TodayBookingsAsync()
        {
            Expression<Func<Reservation, bool>> expression
                = r => r.Date.Date == DateTime.Now.Date;

            var todayBookings = this.repository.All<Reservation>(expression);

            return await todayBookings
                .Select(b => new BookingViewModel()
                {
                    Id = b.Id,
                    CourtNumber = b.Court.Number,
                    UserBookingName = $"{b.User.FirstName} {b.User.LastName}",
                    PeopleCount = b.PeopleCount,
                    RacketsBooked = b.RacketsBooked,
                    UserId = b.UserId,
                    CreatedOn = b.CreatedOn,
                    Date = b.Date,
                    Hour = b.Hour,
                    Status = b.Status,
                    PhoneNumber = b.PhoneNumber,
                    Location = b.Location,
                    ReservationTotalSum = b.ReservationTotalSum
                })
                .OrderBy(b => b.Hour)
                .ToListAsync();
        }

        public async Task<IEnumerable<Court>> GetAllCourtsAsync()
        {
            return await this.repository.All<Court>().OrderBy(c => c.Number).ToListAsync();
        }
    }
}

