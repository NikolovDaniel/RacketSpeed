using RacketSpeed.Core.Models.Booking;

namespace RacketSpeed.UnitTests.TestsData
{
    public class BookingControllerTestData
    {
        public static List<BookingViewModel> ListWithBookingViewModel()
        {
            return new List<BookingViewModel>
            {
                new BookingViewModel()
                {
                    Id = Guid.NewGuid(),
                    CourtNumber = 1,
                    UserBookingName = $"Даниел Николов",
                    PeopleCount = 2,
                    RacketsBooked = 2,
                    UserId = Guid.NewGuid().ToString(),
                    CreatedOn = DateTime.Now,
                    Date = DateTime.Now,
                    Hour = TimeSpan.FromHours(DateTime.Now.Hour + 2),
                    Status = "В разработка",
                    PhoneNumber = "+359883005002",
                    Location = "Зала ИЧС",
                    ReservationTotalSum = 14.0M
                },
                new BookingViewModel()
                {
                    Id = Guid.NewGuid(),
                    CourtNumber = 2,
                    UserBookingName = $"Даниел Николов",
                    PeopleCount = 2,
                    RacketsBooked = 2,
                    UserId = Guid.NewGuid().ToString(),
                    CreatedOn = DateTime.Now,
                    Date = DateTime.Now,
                    Hour = TimeSpan.FromHours(DateTime.Now.Hour + 2),
                    Status = "В разработка",
                    PhoneNumber = "+359893005002",
                    Location = "Зала ИЧС",
                    ReservationTotalSum = 14.0M
                },
                new BookingViewModel()
                {
                    Id = Guid.NewGuid(),
                    CourtNumber = 3,
                    UserBookingName = $"Даниел Николов",
                    PeopleCount = 2,
                    RacketsBooked = 2,
                    UserId = Guid.NewGuid().ToString(),
                    CreatedOn = DateTime.Now,
                    Date = DateTime.Now,
                    Hour = TimeSpan.FromHours(DateTime.Now.Hour + 2),
                    Status = "В разработка",
                    PhoneNumber = "+359886008343",
                    Location = "Зала ИЧС",
                    ReservationTotalSum = 14.0M
                }
            };
        }
    }
}

