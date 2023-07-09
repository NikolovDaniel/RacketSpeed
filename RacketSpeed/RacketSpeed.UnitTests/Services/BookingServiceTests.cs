using System.Collections;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Booking;
using RacketSpeed.Core.Services;
using RacketSpeed.Infrastructure.Data;
using RacketSpeed.Infrastructure.Data.Entities;
using RacketSpeed.Infrastructure.Data.Repositories;
using RacketSpeed.Infrastructure.Data.Seed;
using RacketSpeed.UnitTests.TestsData;

namespace RacketSpeed.UnitTests.Services
{
    public class BookingServiceTests
    {
        private DbContextOptions<ApplicationDbContext> options
            = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase("racketspeedtests")
           .Options;

        private Mock<Repository> repository;
        private IBookingService bookingService;
        private ApplicationDbContext context;

        [SetUp]
        public async Task Setup()
        {
            context = new ApplicationDbContext(options);

            await context.Database.EnsureCreatedAsync();

            repository = new Mock<Repository>(context);

            bookingService = new BookingService(repository.Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await context.Database.EnsureDeletedAsync();
        }

        [Test]
        public async Task GetAllCourtsAsync_ReturnAllCourtsCorrectly()
        {
            // Arrange
            var expectedResult = await this.context.Courts.ToListAsync();

            // Act
            var result = await this.bookingService.GetAllCourtsAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        [TestCase(4)]
        public async Task GetAvailableHoursAsync_WithNoBookingsOnThatCourt_ReturnsCorrectHours(int courtNumber)
        {
            // Arrange
            DateTime date = DateTime.Now;
            var expectedResult = await this.context.Schedule.ToListAsync();

            // Act
            var result = await this.bookingService.GetAvailableHoursAsync(date, courtNumber);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        [TestCase(1, "15:00")]
        [TestCase(1, "16:00")]
        [TestCase(1, "17:00")]
        [TestCase(1, "18:00")]
        public async Task GetAvailableHoursAsync_ReturnsCorrectHours(int courtNumber, TimeSpan hour)
        {
            // Arrange
            DateTime date = DateTime.Now;
            var reservation = BookingServiceTestData.ValidReservationForm();
            reservation.Hour = hour;
            await this.context.Reservations.AddAsync(reservation);
            await this.context.SaveChangesAsync();

            // Act
            var result = await this.bookingService.GetAvailableHoursAsync(date, courtNumber);

            // Assert
            result.Any(r => r.Hour == hour).Should().BeFalse();
        }

        [Test]
        public async Task TodayBookingsAsync_WithNoBookingsToday_ReturnsNull()
        {
            // Act
            var result = await this.bookingService.TodayBookingsAsync();

            // Assert
            result.Should().BeNullOrEmpty();
        }

        [Test]
        public async Task TodayBookingsAsync_ReturnTodaysBookingsCorrectly()
        {
            // Arrange
            int expectedResult = 1;
            await this.context.Reservations.AddAsync(BookingServiceTestData.ValidReservationForm());
            await this.context.SaveChangesAsync();
            // Act
            var result = await this.bookingService.TodayBookingsAsync();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Count().Should().Be(expectedResult);
        }

        [Test]
        [TestCase("2a5224f2-4e92-44ee-8b8b-115b943e0ccf", "2a5434f2-4e92-44ee-8b8b-115b943e0ccf", "Одобрена")]
        [TestCase("aa5354f2-9e12-44ee-8b8b-115b943e0ccf", "2a5434f2-4e92-44ee-8b8b-115b943e0ccf", "В разработка")]
        [TestCase("bb5354f2-9e12-44ee-8b8b-115b943e0ccf", "2a5434f2-4e92-44ee-8b8b-115b943e0ccf", "Отказана")]
        public async Task ChangeStatusAsync_WithInvalidReservationId_DoesNotChangeTheStatus(Guid bookingId, string userId, string status)
        {
            // Arrange
            var expectedResult = await this.context.ApplicationUsers.FindAsync(userId);

            // Act
            await this.bookingService.ChangeStatusAsync(bookingId, userId, status);
            var result = await this.context.ApplicationUsers.FindAsync(userId);

            // Assert
            result.Should().NotBeNull().And.Match<ApplicationUser>(r => r.Deposit == expectedResult.Deposit, "we forwarded invalid userId");
        }


        [Test]
        [TestCase(SeedPropertyConstants.ReservationId1, "2a5224f2-4e92-44ee-8b8b-115b943e0ccf", "Одобрена")]
        [TestCase(SeedPropertyConstants.ReservationId1, "dsa234f2-4e92-44ee-8b8b-115b943e0ccf", "В разработка")]
        public async Task ChangeStatusAsync_WithInvalidUserId_DoesNotChangeTheStatus(Guid bookingId, string userId, string status)
        {
            // Arrange
            var expectedResult = await this.context.Reservations.FindAsync(bookingId);

            // Act
            await this.bookingService.ChangeStatusAsync(bookingId, userId, status);
            var result = await this.context.Reservations.FindAsync(bookingId);

            // Assert
            result.Should().NotBeNull().And.Match<Reservation>(r => r.Status == expectedResult.Status, "we forwarded invalid userId");
        }


        [Test]
        [TestCase(SeedPropertyConstants.ReservationId1, "2a5434f2-4e92-44ee-8b8b-115b943e0ccf", "Одобрена", 30.0)]
        [TestCase(SeedPropertyConstants.ReservationId2, "2a5434f2-4e92-44ee-8b8b-115b943e0ccf", "Отказана", 0.0)]
        [TestCase(SeedPropertyConstants.ReservationId3, "2a5434f2-4e92-44ee-8b8b-115b943e0ccf", "В разработка", 0.0)]
        public async Task ChangeStatusAsync_ReturnsTheDepositSumCorrectly(Guid bookingId, string userId, string status, decimal expectedResult)
        {
            // Arrange
            var user = await this.context.ApplicationUsers.FindAsync(userId);
            user.Deposit = 0;
            await this.context.SaveChangesAsync();

            // Act
            await this.bookingService.ChangeStatusAsync(bookingId, userId, status);
            var result = user.Deposit;

            // Assert
            user.Should().NotBeNull();
            result.Should().Be(expectedResult, "we approved the reservation");
        }

        [Test]
        [TestCase(SeedPropertyConstants.ReservationId1, "2a5434f2-4e92-44ee-8b8b-115b943e0ccf", "Одобрена")]
        [TestCase(SeedPropertyConstants.ReservationId2, "2a5434f2-4e92-44ee-8b8b-115b943e0ccf", "Отказана")]
        [TestCase(SeedPropertyConstants.ReservationId3, "2a5434f2-4e92-44ee-8b8b-115b943e0ccf", "В разработка")]
        public async Task ChangeStatusAsync_ChangesTheStatusCorrectly(Guid bookingId, string userId, string status)
        {
            // Arrange
            var reservation = await context.Reservations.FindAsync(bookingId);

            // Act
            await bookingService.ChangeStatusAsync(reservation.Id, userId, status);
            var result = await context.Reservations.FindAsync(reservation.Id);

            // Assert
            result.Should().NotBeNull().And.Match<Reservation>(r => r.Status == status);
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public async Task BookingsPageCount_ReturnsCorrectResult(int bookingsPerPage)
        {
            // Arrange
            int allBookingsCount = await this.context.Reservations
                .Where(r => r.IsDeleted == false)
                .CountAsync();
            int expectedResult = (int)Math.Ceiling((allBookingsCount / (double)bookingsPerPage));

            // Act
            int result = this.bookingService.BookingsPageCount(bookingsPerPage);

            // Assert
            result.Should().Be(expectedResult, $"we thought we have {expectedResult} reservations in the collection");
        }

        [Test]
        [TestCase("2a5434f2-4e92-44ee-8b8b-115b943e0ccf")]
        [TestCase("343434f2-4e92-44ee-8b8b-505b943e0ccf")]
        public async Task UserBookingsAsync_WithValidUserId_ReturnResultsCorrectly(string userId)
        {
            // Arrange
            int bookingsCount = 5;
            var expectedResult = await this.context.Reservations
            .Where(r => r.UserId == userId)
            .OrderByDescending(ub => ub.Date)
            .ThenBy(ub => ub.Hour)
            .Take(bookingsCount)
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

            // Act
            var result = await this.bookingService.UserBookingsAsync(userId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult, $"we select the reservations by the same userId");
        }

        [Test]
        [TestCase(0, 3, "359883024522")]
        [TestCase(1, 2, "359883")]
        [TestCase(2, 1, "2452")]
        public async Task AllAsync_WithPhoneNumber_ReturnResultsCorrectly(int start, int bookingsPerPage, string phoneNumber)
        {
            // Arrange
            int calcCurrPage = start;
            int calcCurrStart = (calcCurrPage - 1) * bookingsPerPage;
            var expectedResult = await this.context.Reservations
                .AsNoTracking()
                .Where(r => r.IsDeleted == false && r.PhoneNumber.Contains(phoneNumber))
                .OrderByDescending(r => r.Date.Year)
                .ThenBy(r => r.Date.Date)
                .ThenBy(r => r.Hour)
                .Skip(calcCurrStart)
                .Take(bookingsPerPage)
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
               .ToListAsync();

            // Act
            var bookings = await bookingService.AllAsync(start, bookingsPerPage, phoneNumber);

            // Assert
            bookings.Should().BeEquivalentTo(expectedResult, $"we ordered them by the same criteria");
        }

        [Test]
        [TestCase(0, 3)]
        [TestCase(1, 2)]
        [TestCase(1, 3)]
        public async Task AllAsync_ReturnsResultsInCorrectOrder(int start, int bookingsPerPage)
        {
            // Arrange
            int calcCurrPage = start;
            int calcCurrStart = (calcCurrPage - 1) * bookingsPerPage;
            var expectedResult = await this.context.Reservations
                .AsNoTracking()
                .OrderByDescending(r => r.Date.Year)
                .ThenBy(r => r.Date.Date)
                .ThenBy(r => r.Hour)
                .Skip(calcCurrStart)
                .Take(bookingsPerPage)
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
               .ToListAsync();

            // Act
            var bookings = await bookingService.AllAsync(start, bookingsPerPage);

            // Assert
            bookings.Should().BeEquivalentTo(expectedResult, $"we ordered them by the same criteria");
        }

        [Test]
        [TestCase(2, 5)]
        [TestCase(3, 15)]
        [TestCase(5, 8)]
        public async Task AllAsync_WithInvalidPagination_ReturnsEntitiesCorrectly(int start, int bookingsPerPage)
        {
            // Arrange
            int expectedCount = 0;

            // Act
            var bookings = await bookingService.AllAsync(start, bookingsPerPage);

            // Assert
            bookings.Should().HaveCount(expectedCount, $"we wanted to start from invalid page {start} and get {bookingsPerPage} reservations");
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(1, 2)]
        [TestCase(1, 3)]
        public async Task AllAsync_WithValidPagination_ReturnsEntitiesCorrectly(int start, int bookingsPerPage)
        {
            // Arrange
            int expectedCount = bookingsPerPage;

            // Act
            var bookings = await bookingService.AllAsync(start, bookingsPerPage);

            // Assert
            bookings.Should().HaveCount(expectedCount, $"we wanted to start from page {start} and get {bookingsPerPage} reservations");
        }

        [Test]
        public async Task AllAsync_ReturnsAllEntitiesCorrectly()
        {
            // Arrange
            int initialCount = await context.Reservations.CountAsync();
            int start = 0;
            int bookingsPerPage = 3;

            // Act
            var bookings = await bookingService.AllAsync(start, bookingsPerPage);

            // Assert
            bookings.Should().HaveCount(initialCount, $"we have {initialCount} entities in the database!");
        }

        [Test]
        public async Task AddAsync_WithValidModel_AddsCorrectly()
        {
            // Arrange
            var reservation = BookingServiceTestData.ValidBookingForm();
            int initialCount = await context.Reservations.CountAsync();

            // Act
            await bookingService.AddAsync(reservation);
            int finalCount = await context.Reservations.CountAsync();

            // Assert
            finalCount.Should().Be(initialCount + 1, "we added an entity");
        }
    }
}