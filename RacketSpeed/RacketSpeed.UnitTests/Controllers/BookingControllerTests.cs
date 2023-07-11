using System.Security.Claims;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RacketSpeed.Controllers;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Booking;
using RacketSpeed.Infrastructure.Data.Entities;
using RacketSpeed.UnitTests.TestsData;

namespace RacketSpeed.UnitTests.Controllers
{
    [TestFixture]
    public class BookingControllerTests
    {
        private Mock<IBookingService> _bookingServiceMock;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private BookingController _controller;
        private Fixture fixture = new Fixture();
        private ClaimsPrincipal Administrator = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "Username"),
                new Claim(ClaimTypes.Role, "Administrator")
            }, "mock"));
        private ClaimsPrincipal RegularUser = new ClaimsPrincipal(new ClaimsIdentity(new[]
          {
                new Claim(ClaimTypes.Name, "Username"),
                new Claim(ClaimTypes.Role, "RegularUser")
            }, "mock"));
        private ClaimsPrincipal Employee = new ClaimsPrincipal(new ClaimsIdentity(new[]
          {
                new Claim(ClaimTypes.Name, "Username"),
                new Claim(ClaimTypes.Role, "Employee")
            }, "mock"));

        [SetUp]
        public void Setup()
        {
            _bookingServiceMock = new Mock<IBookingService>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            _controller = new BookingController(_bookingServiceMock.Object, _userManagerMock.Object);
        }

        [Test]
        public async Task ChangeBookingStatus_POST_UnauthorizedAccess_ReturnsUnauthorizedResult()
        {
            // Arrange
            var bookingId = Guid.NewGuid();
            var userId = "user1";
            var status = "Одобрена";

            var user = this.RegularUser;

            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // Act
            var result = await _controller.ChangeBookingStatus(bookingId, userId, status);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Test]
        public async Task ChangeBookingStatus_POST_ValidStatusChangeWithAuthorizedAccess_ReturnsRedirectToActionResult()
        {
            // Arrange
            var bookingId = Guid.NewGuid(); 
            var userId = "user1";
            var status = "В разработка"; 

            _bookingServiceMock.Setup(b => b.ChangeStatusAsync(bookingId, userId, status))
                .Returns(Task.CompletedTask);

            var user = this.Employee;

            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user }; 

            // Act
            var result = await _controller.ChangeBookingStatus(bookingId, userId, status);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result.As<RedirectToActionResult>();
            redirectResult.ActionName.Should().Be("TodayBookings");
            redirectResult.ControllerName.Should().Be("Booking");
        }

        [Test]
        public async Task AllBookings_GET_UnauthorizedAccess_ReturnsUnauthorizedResult()
        {
            // Arrange
            var pageCount = "1";

            var user = this.RegularUser;

            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // Act
            var result = await _controller.AllBookings(pageCount);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Test]
        public async Task AllBookings_GET_WithValidPageNumberAndAuthorizedAccess_ReturnsViewResultWithBookings()
        {
            // Arrange
            var pageCount = "1"; 
            var pageNum = 1;
            var bookingsPerPage = 8;
            var bookings = BookingControllerTestData.ListWithBookingViewModel();
            var pagesCount = 3; 

            _bookingServiceMock.Setup(b => b.AllAsync(pageNum, bookingsPerPage))
                .ReturnsAsync(bookings);
            _bookingServiceMock.Setup(b => b.BookingsPageCount(bookingsPerPage))
                .Returns(pagesCount);

            var user = this.Administrator;

            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // Act
            var result = await _controller.AllBookings(pageCount);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.Model.Should().BeOfType<BookingsPaginationCountViewModel>();

            var viewModel = viewResult.Model.As<BookingsPaginationCountViewModel>();
            viewModel.Bookings.Should().BeEquivalentTo(bookings);
            viewModel.PageCount.Should().Be(pagesCount);

            viewResult.ViewName.Should().BeNull();
            viewResult.ViewData["pageNum"].Should().Be(pageNum);
        }

        [Test]
        public async Task BookingsByKeyword_GET_UnauthorizedAccess_ReturnsUnauthorizedResult()
        {
            // Arrange
            var phoneNumber = "+359883";
            var pageCount = "1";

            var user = this.RegularUser;

            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // Act
            var result = await _controller.BookingsByKeyword(phoneNumber, pageCount);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Test]
        public async Task BookingsByKeyword_GET_WithInvalidPhoneNumber_ReturnsViewResultWithModelError()
        {
            // Arrange
            var phoneNumber = "";
            var pageCount = "1";

            var user = this.Administrator;

            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            _controller.ModelState.AddModelError("KeywordError", "Полето трябва да съдържа поне 1 символ.");

            // Act
            var result = await _controller.BookingsByKeyword(phoneNumber, pageCount);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.Model.Should().BeNull();
            viewResult.ViewName.Should().BeNull();

            _controller.ModelState.IsValid.Should().BeFalse();
            _controller.ModelState.Keys.Should().Contain("KeywordError");
        }

        [Test]
        [TestCase("8830", "1")]
        [TestCase("89", "1")]
        [TestCase("359", "1")]
        [TestCase("00", "1")]
        public async Task BookingsByKeyword_GET_WithValidPhoneNumberAndAuthorizedAccess_ReturnsViewResultWithBookings(string phoneNumber, string pageCount)
        {
            // Arrange
            var bookings = BookingControllerTestData.ListWithBookingViewModel();
            int bookingsPerPage = 8;
            int returnCount = 3;
            int start = 1;
            _bookingServiceMock.Setup(b => b.AllAsync(start, bookingsPerPage, phoneNumber))
                .ReturnsAsync(bookings);
            _bookingServiceMock.Setup(b => b.BookingsPageCount(bookingsPerPage))
                .Returns(returnCount);

            var user = this.Administrator;

            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // Act
            var result = await _controller.BookingsByKeyword(phoneNumber, pageCount);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.Model.Should().BeOfType<BookingsPaginationCountViewModel>();

            int expectedCount = 3;
            var viewModel = viewResult.Model.As<BookingsPaginationCountViewModel>();
            viewModel.Bookings.Should().BeEquivalentTo(bookings);
            viewModel.PageCount.Should().Be(expectedCount);

            viewResult.ViewName.Should().BeNull();
        }

        [Test]
        public async Task UserBookings_GET_WithInvalidUserId_ReturnsNotFoundResult()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString(); // Example user ID

            _bookingServiceMock.Setup(b => b.UserBookingsAsync(userId))
                .ReturnsAsync((List<BookingUserViewModel>)null); // Return null to simulate invalid user ID

            // Act
            var result = await _controller.UserBookings(userId);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.Model.Should().BeNull();
            viewResult.ViewName.Should().BeNull();
        }

        [Test]
        public async Task UserBookings_GET_WithValidUserId_ReturnsViewResultWithUserBookings()
        {
            // Arrange
            int createCount = 4;
            var userId = Guid.NewGuid().ToString(); // Example user ID
            var userBookings = fixture.Build<BookingUserViewModel>()
                .With(b => b.CourtNumber, 1)
                .With(b => b.UserBookingName, "Даниел Николов")
                .With(b => b.PeopleCount, 4)
                .With(b => b.RacketsBooked, 2)
                .With(b => b.Date, DateTime.Now)
                .With(b => b.Hour, TimeSpan.FromHours(DateTime.Now.Hour + 2))
                .With(b => b.Location, "Зала ИЧС")
                .With(b => b.PhoneNumber, "+359883003002")
                .With(b => b.Status, "Одобрена")
                .With(b => b.ReservationTotalSum, 24.0M)
                .CreateMany(createCount);

            _bookingServiceMock.Setup(b => b.UserBookingsAsync(userId))
                .ReturnsAsync(userBookings.ToList());

            // Act
            var result = await _controller.UserBookings(userId);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.Model.Should().BeOfType<List<BookingUserViewModel>>();
            viewResult.Model.As<List<BookingUserViewModel>>().Should().BeEquivalentTo(userBookings);
            viewResult.ViewName.Should().BeNull();
        }

        [Test]
        public async Task TodayBookings_GET_WithoutEmployeeRole_ReturnsUnauthorizedResult()
        {
            // Arrange
            var user = this.RegularUser;

            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            // Act
            var result = await _controller.TodayBookings();

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Test]
        public async Task TodayBookings_GET_WithEmployeeRole_ReturnsViewResultWithBookings()
        {
            // Arrange
            var bookings = new List<BookingViewModel>
            {
              new BookingViewModel { Id = Guid.NewGuid(), UserBookingName = "Name 1" },
              new BookingViewModel { Id = Guid.NewGuid(), UserBookingName = "Name 2" },
              new BookingViewModel { Id = Guid.NewGuid(), UserBookingName = "Name 3" }
            };

            _bookingServiceMock.Setup(s => s.TodayBookingsAsync())
                .ReturnsAsync(bookings);

            var user = this.Employee;

            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // Act
            var result = await _controller.TodayBookings();

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.Model.Should().BeEquivalentTo(bookings);
            viewResult.ViewName.Should().BeNull();
        }

        [Test]
        public async Task GetAvailableHours_GET_ReturnsJsonArrayWithAvailableHours()
        {
            // Arrange
            var date = "2023-07-06";
            var courtNumber = 1;
            var expectedHours = new List<string> { "08:00", "09:00", "10:00", "11:00" };
            _bookingServiceMock.Setup(s => s.GetAvailableHoursAsync(DateTime.Parse(date), courtNumber))
                .Returns(Task.FromResult(expectedHours.Select(hour => new Schedule { Hour = TimeSpan.Parse(hour) }).ToList()));

            // Act
            var result = await _controller.GetAvailableHours(date, courtNumber);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result.As<JsonResult>();
            jsonResult.Value.Should().BeEquivalentTo(expectedHours.Select(hour => new { hour }));
        }

        [Test]
        public async Task Book_POST_WithValidBooking_TakesDepositAndRedirectsToUserBookings()
        {
            // Arrange
            var model = new BookingFormModel
            {
                UserId = "user123",
                PeopleCount = 4,
                RacketsBooked = 2,
                ReservationTotalSum = 24,
                Date = DateTime.Now,
                Hour = TimeSpan.FromHours(DateTime.Now.Hour + 2)
            };

            var user = new ApplicationUser
            {
                Id = "user123",
                Deposit = 30
            };

            _userManagerMock.Setup(m => m.FindByIdAsync(model.UserId)).ReturnsAsync(user);

            // Act
            var result = await _controller.Book(model);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result.As<RedirectToActionResult>();
            redirectResult.ActionName.Should().Be("UserBookings");
            redirectResult.ControllerName.Should().Be("Booking");

            user.Deposit.Should().Be(0);
        }

        [Test]
        public async Task Book_POST_UserWithNoDeposit_DoesNotTakeDepositAndReturnsViewWithModelError()
        {
            // Arrange
            var model = new BookingFormModel
            {
                UserId = "user123",
                PeopleCount = 2,
                RacketsBooked = 1,
                ReservationTotalSum = 12,
                Date = DateTime.Now,
                Hour = TimeSpan.FromHours(DateTime.Now.Hour + 2)
            };

            var user = new ApplicationUser
            {
                Id = "user123",
                Deposit = 0
            };

            _userManagerMock.Setup(m => m.FindByIdAsync(model.UserId)).ReturnsAsync(user);

            // Act
            var result = await _controller.Book(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewData.ModelState["NoDeposit"].Errors
            .Should()
            .Contain(e => e.ErrorMessage == "За да запазите корт Ви е нужен депозит на стойност 30 лева.");
            user.Deposit.Should().Be(0);
        }

        [Test]
        public async Task Book_POST_WithInvalidDate_ReturnsViewWithBookingFormModel()
        {
            // Arrange
            var model = new BookingFormModel
            {
                PeopleCount = 2,
                RacketsBooked = 1,
                ReservationTotalSum = 12,
                Date = DateTime.Parse("07-05-2022")
            };
            _controller.ModelState.AddModelError("InvalidModel", "Invalid model error");

            // Act
            var result = await _controller.Book(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.Model.Should().Be(model);
        }

        [Test]
        public async Task Book_POST_WithTheSameDateAndHour_ReturnsViewWithBookingFormModel()
        {
            // Arrange
            var model = new BookingFormModel
            {
                PeopleCount = 2,
                RacketsBooked = 1,
                ReservationTotalSum = 12,
                Date = DateTime.Now,
                Hour = TimeSpan.FromHours(DateTime.Now.Hour)
            };

            // Act
            var result = await _controller.Book(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewData.ModelState["InvalidHour"].Errors
                .Should()
                .Contain(e => e.ErrorMessage == "Часа на резервация трябва да бъде най-рано 1 час от текущия.");
            viewResult.Model.Should().Be(model);
        }

        [Test]
        public async Task Book_POST_WithInvalidModelState_ReturnsViewWithBookingFormModel()
        {
            // Arrange
            var model = new BookingFormModel();
            _controller.ModelState.AddModelError("InvalidModel", "Invalid model error");

            // Act
            var result = await _controller.Book(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.Model.Should().Be(model);
        }

        [Test]
        [TestCase(30.0)]
        [TestCase(11.0)]
        [TestCase(19.0)]
        public async Task Book_POST_WithInvalidTotalPrice_ReturnsViewWithBookingFormModel(decimal reservationSum)
        {
            // Arrange
            var model = new BookingFormModel
            {
                PeopleCount = 2,
                RacketsBooked = 1,
                ReservationTotalSum = reservationSum,
                Date = DateTime.Now
            };

            // Act
            var result = await _controller.Book(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewData.ModelState["InvalidTotalPrice"].Errors
                .Should()
                .Contain(e => e.ErrorMessage == "Цената не отговаря на истинската.");
            viewResult.Model.Should().Be(model);
        }

        [Test]
        public async Task Book_GET_ReturnsViewWithBookingFormModel()
        {
            // Arrange
            int createCount = 4;
            var courts = fixture.Build<Court>()
                .With(c => c.Id, Guid.NewGuid())
                .With(c => c.Number, 1)
                .With(c => c.IsDeleted, false)
                .CreateMany(createCount);
            _bookingServiceMock.Setup(b => b.GetAllCourtsAsync()).ReturnsAsync(courts);

            // Act
            var result = await _controller.Book();

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.Model.Should().BeOfType<BookingFormModel>();
            var model = viewResult.Model.As<BookingFormModel>();
            model.Courts.Should().BeEquivalentTo(courts);
        }
    }
}

