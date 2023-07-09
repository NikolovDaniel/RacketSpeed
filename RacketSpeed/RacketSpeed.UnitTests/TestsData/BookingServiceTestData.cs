using RacketSpeed.Core.Models.Booking;
using RacketSpeed.Infrastructure.Data.Entities;
using RacketSpeed.Infrastructure.Data.Seed;

namespace RacketSpeed.UnitTests.TestsData
{
    public class BookingServiceTestData
    {
        //public const BookingFormModel validBookingForm = ValidBookingForm();

        public static Reservation ValidReservationForm()
        {
            return new Reservation()
            {
                Id = Guid.NewGuid(),
                UserId = "2a5434f2-4e92-44ee-8b8b-115b943e0ccf",
                CourtId = Guid.Parse("AC0AEA93-5277-4992-9F4B-5C1A8CBB8395"), // court 1
                PeopleCount = 4,
                RacketsBooked = 4,
                CreatedOn = DateTime.Now,
                ReservationTotalSum = 28,
                Date = DateTime.Now,
                Hour = TimeSpan.Parse("15:00"),
                Status = "В разработка",
                Location = "Зала ИЧС",
                PhoneNumber = "+359886075422",
                IsDeleted = false
            };
        }

        public static BookingFormModel ValidBookingForm()
        {
            return new BookingFormModel()
            {
                Id = Guid.NewGuid(),
                CourtNumber = 1,
                UserId = "2a5434f2-4e92-44ee-8b8b-115b943e0ccf",
                PeopleCount = 4,
                RacketsBooked = 4,
                CreatedOn = DateTime.Now,
                ReservationTotalSum = 28,
                Date = DateTime.Now,
                Hour = TimeSpan.Parse("15:00"),
                Status = "В разработка",
                Location = "Зала ИЧС",
                PhoneNumber = "+359886075422",
            };
        }

        public static List<BookingFormModel> BookingFormModels()
        {
            return new List<BookingFormModel>();
        }

    }
}

