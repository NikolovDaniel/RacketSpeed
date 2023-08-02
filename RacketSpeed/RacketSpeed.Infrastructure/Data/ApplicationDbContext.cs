using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RacketSpeed.Infrastructure.Data.Entities;
using RacketSpeed.Infrastructure.Data.Seed;

namespace RacketSpeed.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
        public DbSet<Coach> Coaches { get; set; } = null!;
        public DbSet<Court> Courts { get; set; } = null!;
        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;
        public DbSet<Training> Trainings { get; set; } = null!;
        public DbSet<PostImageUrl> PostImageUrls { get; set; } = null!;
        public DbSet<PlayerImageUrl> PlayerImageUrls { get; set; } = null!;
        public DbSet<CoachImageUrl> CoachImageUrls { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<EventImageUrl> EventImageUrls { get; set; } = null!;
        public DbSet<SignKid> SignedKids { get; set; } = null!;
        public DbSet<Schedule> Schedule { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseInMemoryDatabase("racketspeedtests");

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // This might be a problem in the future if any of the entities come with
            // large amount of images.
            // Fluent API to configure a Coach to always come with its ImageUrls.
            builder.Entity<Coach>().Navigation(img => img.CoachImageUrl).AutoInclude();
            // Fluent API to configure a Player to always come with its ImageUrls.
            builder.Entity<Player>().Navigation(img => img.PlayerImageUrl).AutoInclude();
            // Fluent API to configure a Post to always come with its ImageUrls.
            builder.Entity<Post>().Navigation(img => img.PostImageUrls).AutoInclude();
            // Fluent API to configure an Event to always come with its ImageUrls.
            builder.Entity<Event>().Navigation(img => img.EventImageUrls).AutoInclude();

            // Fluent API to configure an Reservation to always come with its User and Court.
            builder.Entity<Reservation>().Navigation(r => r.Court).AutoInclude();
            builder.Entity<Reservation>().Navigation(r => r.User).AutoInclude();

            // Fluent API to configure ApplicationUser and Reservation Entities for One-To-Many.
            builder.Entity<ApplicationUser>()
                .HasMany(au => au.Reservations)
                .WithOne(r => r.User)
                .OnDelete(DeleteBehavior.Restrict);

            //if (Database.IsInMemory())
            //{
            //    SeedUsers(builder);

            //    SeedPostsAndImages(builder);

            //    SeedCoachAndImages(builder);

            //    SeedCoachTrainings(builder);

            //    SeedEventsAndImages(builder);

            //    SeedCourts(builder);

            //    SeedPlayersAndImages(builder);

            //    SeedSchedule(builder);

            //    SeedReservations(builder);

            //    SeedSignForms(builder);
            //}
            SeedUsers(builder);

            SeedPostsAndImages(builder);

            SeedCoachAndImages(builder);

            SeedCoachTrainings(builder);

            SeedEventsAndImages(builder);

            SeedCourts(builder);

            SeedPlayersAndImages(builder);

            SeedSchedule(builder);

            SeedReservations(builder);

            SeedSignForms(builder);

            base.OnModelCreating(builder);
        }

        private void SeedSignForms(ModelBuilder builder)
        {
            builder.Entity<SignKid>()
                .HasData(
                new SignKid()
                {
                    Id = Guid.Parse(SeedPropertyConstants.SignFormId1),
                    FullName = SeedPropertyConstants.SignFormFullName1,
                    PhoneNumber = SeedPropertyConstants.SignFormPhoneNumber1,
                    EmailAddress = SeedPropertyConstants.SignFormEmailAddress1,
                    SignOn = DateTime.Parse(SeedPropertyConstants.SignFormSignOn1),
                    TrainingType = SeedPropertyConstants.SignFormTrainingType1,
                    Location = SeedPropertyConstants.SignFormLocation1,
                    Message = SeedPropertyConstants.SignFormMessage1,
                    PrivacyPolicyIsAccepted = true,
                    IsDeleted = false
                },
                new SignKid()
                {
                    Id = Guid.Parse(SeedPropertyConstants.SignFormId2),
                    FullName = SeedPropertyConstants.SignFormFullName2,
                    PhoneNumber = SeedPropertyConstants.SignFormPhoneNumber2,
                    EmailAddress = SeedPropertyConstants.SignFormEmailAddress2,
                    SignOn = DateTime.Parse(SeedPropertyConstants.SignFormSignOn2),
                    TrainingType = SeedPropertyConstants.SignFormTrainingType2,
                    Location = SeedPropertyConstants.SignFormLocation2,
                    Message = SeedPropertyConstants.SignFormMessage2,
                    PrivacyPolicyIsAccepted = true,
                    IsDeleted = false
                },
                new SignKid()
                {
                    Id = Guid.Parse(SeedPropertyConstants.SignFormId3),
                    FullName = SeedPropertyConstants.SignFormFullName3,
                    PhoneNumber = SeedPropertyConstants.SignFormPhoneNumber3,
                    EmailAddress = SeedPropertyConstants.SignFormEmailAddress3,
                    SignOn = DateTime.Parse(SeedPropertyConstants.SignFormSignOn3),
                    TrainingType = SeedPropertyConstants.SignFormTrainingType3,
                    Location = SeedPropertyConstants.SignFormLocation3,
                    Message = SeedPropertyConstants.SignFormMessage3,
                    PrivacyPolicyIsAccepted = true,
                    IsDeleted = false
                });
        }

        private void SeedReservations(ModelBuilder builder)
        {
            builder.Entity<Reservation>()
                .HasData(
                new Reservation()
                {
                    Id = Guid.Parse(SeedPropertyConstants.ReservationId1),
                    CourtId = Guid.Parse("AC0AEA93-5277-4992-9F4B-5C1A8CBB8395"),
                    UserId = "2a5434f2-4e92-44ee-8b8b-115b943e0ccf",
                    PeopleCount = SeedPropertyConstants.ReservationPeopleCount1,
                    RacketsBooked = SeedPropertyConstants.ReservationRacketsBooked1,
                    CreatedOn = DateTime.Parse(SeedPropertyConstants.ReservationCreatedOn1),
                    ReservationTotalSum = SeedPropertyConstants.ReservationTotalSum1,
                    Date = DateTime.Parse(SeedPropertyConstants.ReservationDate1),
                    Hour = TimeSpan.Parse(SeedPropertyConstants.ReservationHour1),
                    Status = SeedPropertyConstants.ReservationStatus1,
                    Location = SeedPropertyConstants.ReservationLocation1,
                    PhoneNumber = SeedPropertyConstants.ReservationPhoneNumber1,
                    IsDeleted = false
                },
                new Reservation()
                {
                    Id = Guid.Parse(SeedPropertyConstants.ReservationId2),
                    CourtId = Guid.Parse("81CFCBBD-6411-4E43-974F-625C2DB5B9AC"),
                    UserId = "2a5434f2-4e92-44ee-8b8b-115b943e0ccf",
                    PeopleCount = SeedPropertyConstants.ReservationPeopleCount2,
                    RacketsBooked = SeedPropertyConstants.ReservationRacketsBooked2,
                    CreatedOn = DateTime.Parse(SeedPropertyConstants.ReservationCreatedOn2),
                    ReservationTotalSum = SeedPropertyConstants.ReservationTotalSum2,
                    Date = DateTime.Parse(SeedPropertyConstants.ReservationDate2),
                    Hour = TimeSpan.Parse(SeedPropertyConstants.ReservationHour2),
                    Status = SeedPropertyConstants.ReservationStatus2,
                    Location = SeedPropertyConstants.ReservationLocation2,
                    PhoneNumber = SeedPropertyConstants.ReservationPhoneNumber2,
                    IsDeleted = false
                },
                new Reservation()
                {
                    Id = Guid.Parse(SeedPropertyConstants.ReservationId3),
                    CourtId = Guid.Parse("D4A6A62B-6695-486A-B81E-C60DA83BA6F2"),
                    UserId = "2a5434f2-4e92-44ee-8b8b-115b943e0ccf",
                    PeopleCount = SeedPropertyConstants.ReservationPeopleCount3,
                    RacketsBooked = SeedPropertyConstants.ReservationRacketsBooked3,
                    CreatedOn = DateTime.Parse(SeedPropertyConstants.ReservationCreatedOn3),
                    ReservationTotalSum = SeedPropertyConstants.ReservationTotalSum3,
                    Date = DateTime.Parse(SeedPropertyConstants.ReservationDate3),
                    Hour = TimeSpan.Parse(SeedPropertyConstants.ReservationHour3),
                    Status = SeedPropertyConstants.ReservationStatus3,
                    Location = SeedPropertyConstants.ReservationLocation3,
                    PhoneNumber = SeedPropertyConstants.ReservationPhoneNumber3,
                    IsDeleted = false
                });
        }

        private void SeedSchedule(ModelBuilder builder)
        {
            // Define the start and end times for the availability slots
            TimeSpan startTime = TimeSpan.FromHours(9); // Start time: 09:00 AM
            TimeSpan endTime = TimeSpan.FromHours(21); // End time: 21:00 PM

            // Define the duration of each availability slot
            TimeSpan timeSlotDuration = TimeSpan.FromHours(1); // 1-hour time slots

            while (startTime <= endTime)
            {
                startTime = startTime.Add(timeSlotDuration);

                builder.Entity<Schedule>()
                .HasData(
                    new Schedule()
                    {
                        Id = Guid.NewGuid(),
                        Hour = startTime
                    });
            }
        }

        private void SeedPlayersAndImages(ModelBuilder builder)
        {
            builder.Entity<Player>()
                .HasData(
                new Player()
                {
                    Id = Guid.Parse(SeedPropertyConstants.PlayerId1),
                    FirstName = SeedPropertyConstants.PlayerFirstName1,
                    LastName = SeedPropertyConstants.PlayerLastName1,
                    WorldRanking = SeedPropertyConstants.PlayerWorldRanking1,
                    Biography = SeedPropertyConstants.PlayerBiography1,
                    BirthDate = DateTime.Parse(SeedPropertyConstants.PlayerBirthDate1),
                    CreatedOn = DateTime.Parse(SeedPropertyConstants.PlayerCreatedOn1),
                    NationalRanking = SeedPropertyConstants.PlayerNationalRanking1,
                    BirthPlace = SeedPropertyConstants.PlayerBirthPlace1,
                    Height = SeedPropertyConstants.PlayerHeight1,
                    PlayingHand = SeedPropertyConstants.PlayerPlayingHand1,
                    IsDeleted = false
                },
                new Player()
                {
                    Id = Guid.Parse(SeedPropertyConstants.PlayerId2),
                    FirstName = SeedPropertyConstants.PlayerFirstName2,
                    LastName = SeedPropertyConstants.PlayerLastName2,
                    WorldRanking = SeedPropertyConstants.PlayerWorldRanking2,
                    Biography = SeedPropertyConstants.PlayerBiography2,
                    BirthDate = DateTime.Parse(SeedPropertyConstants.PlayerBirthDate2),
                    CreatedOn = DateTime.Parse(SeedPropertyConstants.PlayerCreatedOn2),
                    NationalRanking = SeedPropertyConstants.PlayerNationalRanking2,
                    BirthPlace = SeedPropertyConstants.PlayerBirthPlace2,
                    Height = SeedPropertyConstants.PlayerHeight2,
                    PlayingHand = SeedPropertyConstants.PlayerPlayingHand2,
                    IsDeleted = false
                },
                new Player()
                {
                    Id = Guid.Parse(SeedPropertyConstants.PlayerId3),
                    FirstName = SeedPropertyConstants.PlayerFirstName3,
                    LastName = SeedPropertyConstants.PlayerLastName3,
                    WorldRanking = SeedPropertyConstants.PlayerWorldRanking3,
                    Biography = SeedPropertyConstants.PlayerBiography3,
                    BirthDate = DateTime.Parse(SeedPropertyConstants.PlayerBirthDate3),
                    CreatedOn = DateTime.Parse(SeedPropertyConstants.PlayerCreatedOn3),
                    NationalRanking = SeedPropertyConstants.PlayerNationalRanking3,
                    BirthPlace = SeedPropertyConstants.PlayerBirthPlace3,
                    Height = SeedPropertyConstants.PlayerHeight3,
                    PlayingHand = SeedPropertyConstants.PlayerPlayingHand3,
                    IsDeleted = false
                });

            builder.Entity<PlayerImageUrl>()
                .HasData(
                new PlayerImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Player1Img1Id),
                    PlayerId = Guid.Parse(SeedPropertyConstants.PlayerId1),
                    Url = SeedPropertyConstants.Player1Img1Url
                },
                new PlayerImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Player2Img1Id),
                    PlayerId = Guid.Parse(SeedPropertyConstants.PlayerId2),
                    Url = SeedPropertyConstants.Player2Img1Url
                },
                new PlayerImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Player3Img1Id),
                    PlayerId = Guid.Parse(SeedPropertyConstants.PlayerId3),
                    Url = SeedPropertyConstants.Player3Img1Url
                });
        }

        private void SeedCourts(ModelBuilder builder)
        {
            builder.Entity<Court>()
                .HasData(

                new Court()
                {
                    Id = Guid.Parse("AC0AEA93-5277-4992-9F4B-5C1A8CBB8395"),
                    Number = 1,
                    IsDeleted = false
                },
                new Court()
                {
                    Id = Guid.Parse("81CFCBBD-6411-4E43-974F-625C2DB5B9AC"),
                    Number = 2,
                    IsDeleted = false
                },
                new Court()
                {
                    Id = Guid.Parse("D4A6A62B-6695-486A-B81E-C60DA83BA6F2"),
                    Number = 3,
                    IsDeleted = false
                },
                new Court()
                {
                    Id = Guid.Parse("6E00F7D0-9549-4404-9385-1213D5764C91"),
                    Number = 4,
                    IsDeleted = false
                });
        }

        private void SeedEventsAndImages(ModelBuilder builder)
        {
            builder.Entity<Event>()
                    .HasData(
                    // Kid events
                    new Event()
                    {
                        Id = Guid.Parse(SeedPropertyConstants.EventKidsId1),
                        Start = DateTime.Parse(SeedPropertyConstants.EventKidsStart1),
                        End = DateTime.Parse(SeedPropertyConstants.EventKidsEnd1),
                        Category = SeedPropertyConstants.EventKidsCategory1,
                        Title = SeedPropertyConstants.EventKidsTitle1,
                        Content = SeedPropertyConstants.EventKidsContent1,
                        Location = SeedPropertyConstants.EventKidsLocation1,
                        IsDeleted = false
                    },
                    new Event()
                    {
                        Id = Guid.Parse(SeedPropertyConstants.EventKidsId2),
                        Start = DateTime.Parse(SeedPropertyConstants.EventKidsStart2),
                        End = DateTime.Parse(SeedPropertyConstants.EventKidsEnd2),
                        Category = SeedPropertyConstants.EventKidsCategory2,
                        Title = SeedPropertyConstants.EventKidsTitle2,
                        Content = SeedPropertyConstants.EventKidsContent2,
                        Location = SeedPropertyConstants.EventKidsLocation2,
                        IsDeleted = false
                    },
                    new Event()
                    {
                        Id = Guid.Parse(SeedPropertyConstants.EventKidsId3),
                        Start = DateTime.Parse(SeedPropertyConstants.EventKidsStart3),
                        End = DateTime.Parse(SeedPropertyConstants.EventKidsEnd3),
                        Category = SeedPropertyConstants.EventKidsCategory3,
                        Title = SeedPropertyConstants.EventKidsTitle3,
                        Content = SeedPropertyConstants.EventKidsContent3,
                        Location = SeedPropertyConstants.EventKidsLocation3,
                        IsDeleted = false
                    },
                    // Adult events
                    new Event()
                    {
                        Id = Guid.Parse(SeedPropertyConstants.EventAdultsId1),
                        Start = DateTime.Parse(SeedPropertyConstants.EventAdultsStart1),
                        End = DateTime.Parse(SeedPropertyConstants.EventAdultsEnd1),
                        Category = SeedPropertyConstants.EventAdultsCategory1,
                        Title = SeedPropertyConstants.EventAdultsTitle1,
                        Content = SeedPropertyConstants.EventAdultsContent1,
                        Location = SeedPropertyConstants.EventAdultsLocation1,
                        IsDeleted = false
                    },
                    // For fun events
                    new Event()
                    {
                        Id = Guid.Parse(SeedPropertyConstants.EventForFunId1),
                        Start = DateTime.Parse(SeedPropertyConstants.EventForFunStart1),
                        End = DateTime.Parse(SeedPropertyConstants.EventForFunEnd1),
                        Category = SeedPropertyConstants.EventForFunCategory1,
                        Title = SeedPropertyConstants.EventForFunTitle1,
                        Content = SeedPropertyConstants.EventForFunContent1,
                        Location = SeedPropertyConstants.EventForFunLocation1,
                        IsDeleted = false
                    },
                    // Camp events
                    new Event()
                    {
                        Id = Guid.Parse(SeedPropertyConstants.EventCampId1),
                        Start = DateTime.Parse(SeedPropertyConstants.EventCampStart1),
                        End = DateTime.Parse(SeedPropertyConstants.EventCampEnd1),
                        Category = SeedPropertyConstants.EventCampCategory1,
                        Title = SeedPropertyConstants.EventCampTitle1,
                        Content = SeedPropertyConstants.EventCampContent1,
                        Location = SeedPropertyConstants.EventCampLocation1,
                        IsDeleted = false
                    },
                    new Event()
                    {
                        Id = Guid.Parse(SeedPropertyConstants.EventCampId2),
                        Start = DateTime.Parse(SeedPropertyConstants.EventCampStart2),
                        End = DateTime.Parse(SeedPropertyConstants.EventCampEnd2),
                        Category = SeedPropertyConstants.EventCampCategory2,
                        Title = SeedPropertyConstants.EventCampTitle2,
                        Content = SeedPropertyConstants.EventCampContent2,
                        Location = SeedPropertyConstants.EventCampLocation2,
                        IsDeleted = false
                    });

            builder.Entity<EventImageUrl>()
                .HasData(
                // Kid events images
                new EventImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Event1Img1Id),
                    EventId = Guid.Parse(SeedPropertyConstants.EventKidsId1),
                    Url = SeedPropertyConstants.Event1Img1Url
                },
                new EventImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Event1Img2Id),
                    EventId = Guid.Parse(SeedPropertyConstants.EventKidsId1),
                    Url = SeedPropertyConstants.Event1Img2Url
                },
                new EventImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Event1Img3Id),
                    EventId = Guid.Parse(SeedPropertyConstants.EventKidsId1),
                    Url = SeedPropertyConstants.Event1Img3Url
                },
                new EventImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Event2Img1Id),
                    EventId = Guid.Parse(SeedPropertyConstants.EventKidsId2),
                    Url = SeedPropertyConstants.Event2Img1Url
                },
                new EventImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Event2Img2Id),
                    EventId = Guid.Parse(SeedPropertyConstants.EventKidsId2),
                    Url = SeedPropertyConstants.Event2Img2Url
                },
                new EventImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Event2Img3Id),
                    EventId = Guid.Parse(SeedPropertyConstants.EventKidsId2),
                    Url = SeedPropertyConstants.Event2Img3Url
                },
                new EventImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Event3Img1Id),
                    EventId = Guid.Parse(SeedPropertyConstants.EventKidsId3),
                    Url = SeedPropertyConstants.Event3Img1Url
                },
                new EventImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Event3Img2Id),
                    EventId = Guid.Parse(SeedPropertyConstants.EventKidsId3),
                    Url = SeedPropertyConstants.Event3Img2Url
                },
                new EventImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Event3Img3Id),
                    EventId = Guid.Parse(SeedPropertyConstants.EventKidsId3),
                    Url = SeedPropertyConstants.Event3Img3Url
                },
                // Adult events images
                new EventImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Event4Img1Id),
                    EventId = Guid.Parse(SeedPropertyConstants.EventAdultsId1),
                    Url = SeedPropertyConstants.Event4Img1Url
                },
                new EventImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Event4Img2Id),
                    EventId = Guid.Parse(SeedPropertyConstants.EventAdultsId1),
                    Url = SeedPropertyConstants.Event4Img2Url
                },
                new EventImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Event4Img3Id),
                    EventId = Guid.Parse(SeedPropertyConstants.EventAdultsId1),
                    Url = SeedPropertyConstants.Event4Img3Url
                },
                // For fun events images
                new EventImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Event5Img1Id),
                    EventId = Guid.Parse(SeedPropertyConstants.EventForFunId1),
                    Url = SeedPropertyConstants.Event5Img1Url
                },
                new EventImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Event5Img2Id),
                    EventId = Guid.Parse(SeedPropertyConstants.EventForFunId1),
                    Url = SeedPropertyConstants.Event5Img2Url
                },
                new EventImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Event5Img3Id),
                    EventId = Guid.Parse(SeedPropertyConstants.EventForFunId1),
                    Url = SeedPropertyConstants.Event5Img3Url
                },
                // Camp events images
                new EventImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Event6Img1Id),
                    EventId = Guid.Parse(SeedPropertyConstants.EventCampId1),
                    Url = SeedPropertyConstants.Event6Img1Url
                },
                new EventImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Event6Img2Id),
                    EventId = Guid.Parse(SeedPropertyConstants.EventCampId1),
                    Url = SeedPropertyConstants.Event6Img2Url
                },
                new EventImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Event6Img3Id),
                    EventId = Guid.Parse(SeedPropertyConstants.EventCampId1),
                    Url = SeedPropertyConstants.Event6Img3Url
                },
                new EventImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Event7Img1Id),
                    EventId = Guid.Parse(SeedPropertyConstants.EventCampId2),
                    Url = SeedPropertyConstants.Event7Img1Url
                },
                new EventImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Event7Img2Id),
                    EventId = Guid.Parse(SeedPropertyConstants.EventCampId2),
                    Url = SeedPropertyConstants.Event7Img2Url
                },
                new EventImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Event7Img3Id),
                    EventId = Guid.Parse(SeedPropertyConstants.EventCampId2),
                    Url = SeedPropertyConstants.Event7Img3Url
                });
        }

        private void SeedCoachTrainings(ModelBuilder builder)
        {
            builder.Entity<Training>()
                .HasData(
                // coach 1 trainings
                new Training()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Coach1Training1Id),
                    Name = SeedPropertyConstants.Coach1Training1Name,
                    Start = DateTime.Parse(SeedPropertyConstants.Coach1Training1Start),
                    End = DateTime.Parse(SeedPropertyConstants.Coach1Training1End),
                    DayOfWeek = SeedPropertyConstants.Coach1Training1DayOfWeek,
                    CoachId = Guid.Parse(SeedPropertyConstants.CoachId1),
                    IsDeleted = false
                },
               new Training()
               {
                   Id = Guid.Parse(SeedPropertyConstants.Coach1Training2Id),
                   Name = SeedPropertyConstants.Coach1Training2Name,
                   Start = DateTime.Parse(SeedPropertyConstants.Coach1Training2Start),
                   End = DateTime.Parse(SeedPropertyConstants.Coach1Training2End),
                   DayOfWeek = SeedPropertyConstants.Coach1Training2DayOfWeek,
                   CoachId = Guid.Parse(SeedPropertyConstants.CoachId1),
                   IsDeleted = false
               },
               new Training()
               {
                   Id = Guid.Parse(SeedPropertyConstants.Coach1Training3Id),
                   Name = SeedPropertyConstants.Coach1Training3Name,
                   Start = DateTime.Parse(SeedPropertyConstants.Coach1Training3Start),
                   End = DateTime.Parse(SeedPropertyConstants.Coach1Training3End),
                   DayOfWeek = SeedPropertyConstants.Coach1Training3DayOfWeek,
                   CoachId = Guid.Parse(SeedPropertyConstants.CoachId1),
                   IsDeleted = false
               },
                 new Training()
                 {
                     Id = Guid.Parse(SeedPropertyConstants.Coach1Training4Id),
                     Name = SeedPropertyConstants.Coach1Training4Name,
                     Start = DateTime.Parse(SeedPropertyConstants.Coach1Training4Start),
                     End = DateTime.Parse(SeedPropertyConstants.Coach1Training4End),
                     DayOfWeek = SeedPropertyConstants.Coach1Training4DayOfWeek,
                     CoachId = Guid.Parse(SeedPropertyConstants.CoachId1),
                     IsDeleted = false
                 },
               new Training()
               {
                   Id = Guid.Parse(SeedPropertyConstants.Coach1Training5Id),
                   Name = SeedPropertyConstants.Coach1Training5Name,
                   Start = DateTime.Parse(SeedPropertyConstants.Coach1Training5Start),
                   End = DateTime.Parse(SeedPropertyConstants.Coach1Training5End),
                   DayOfWeek = SeedPropertyConstants.Coach1Training5DayOfWeek,
                   CoachId = Guid.Parse(SeedPropertyConstants.CoachId1),
                   IsDeleted = false
               },
               new Training()
               {
                   Id = Guid.Parse(SeedPropertyConstants.Coach1Training6Id),
                   Name = SeedPropertyConstants.Coach1Training6Name,
                   Start = DateTime.Parse(SeedPropertyConstants.Coach1Training6Start),
                   End = DateTime.Parse(SeedPropertyConstants.Coach1Training6End),
                   DayOfWeek = SeedPropertyConstants.Coach1Training6DayOfWeek,
                   CoachId = Guid.Parse(SeedPropertyConstants.CoachId1),
                   IsDeleted = false
               },
               // coach 2 trainings
               new Training()
               {
                   Id = Guid.Parse(SeedPropertyConstants.Coach2Training1Id),
                   Name = SeedPropertyConstants.Coach2Training1Name,
                   Start = DateTime.Parse(SeedPropertyConstants.Coach2Training1Start),
                   End = DateTime.Parse(SeedPropertyConstants.Coach2Training1End),
                   DayOfWeek = SeedPropertyConstants.Coach2Training1DayOfWeek,
                   CoachId = Guid.Parse(SeedPropertyConstants.CoachId2),
                   IsDeleted = false
               },
               new Training()
               {
                   Id = Guid.Parse(SeedPropertyConstants.Coach2Training2Id),
                   Name = SeedPropertyConstants.Coach2Training2Name,
                   Start = DateTime.Parse(SeedPropertyConstants.Coach2Training2Start),
                   End = DateTime.Parse(SeedPropertyConstants.Coach2Training2End),
                   DayOfWeek = SeedPropertyConstants.Coach2Training2DayOfWeek,
                   CoachId = Guid.Parse(SeedPropertyConstants.CoachId2),
                   IsDeleted = false
               },
               new Training()
               {
                   Id = Guid.Parse(SeedPropertyConstants.Coach2Training3Id),
                   Name = SeedPropertyConstants.Coach2Training3Name,
                   Start = DateTime.Parse(SeedPropertyConstants.Coach2Training3Start),
                   End = DateTime.Parse(SeedPropertyConstants.Coach2Training3End),
                   DayOfWeek = SeedPropertyConstants.Coach2Training3DayOfWeek,
                   CoachId = Guid.Parse(SeedPropertyConstants.CoachId2),
                   IsDeleted = false
               },
                 new Training()
                 {
                     Id = Guid.Parse(SeedPropertyConstants.Coach2Training4Id),
                     Name = SeedPropertyConstants.Coach2Training4Name,
                     Start = DateTime.Parse(SeedPropertyConstants.Coach2Training4Start),
                     End = DateTime.Parse(SeedPropertyConstants.Coach2Training4End),
                     DayOfWeek = SeedPropertyConstants.Coach2Training4DayOfWeek,
                     CoachId = Guid.Parse(SeedPropertyConstants.CoachId2),
                     IsDeleted = false
                 },
               new Training()
               {
                   Id = Guid.Parse(SeedPropertyConstants.Coach2Training5Id),
                   Name = SeedPropertyConstants.Coach2Training5Name,
                   Start = DateTime.Parse(SeedPropertyConstants.Coach2Training5Start),
                   End = DateTime.Parse(SeedPropertyConstants.Coach2Training5End),
                   DayOfWeek = SeedPropertyConstants.Coach2Training5DayOfWeek,
                   CoachId = Guid.Parse(SeedPropertyConstants.CoachId2),
                   IsDeleted = false
               },
               new Training()
               {
                   Id = Guid.Parse(SeedPropertyConstants.Coach2Training6Id),
                   Name = SeedPropertyConstants.Coach2Training6Name,
                   Start = DateTime.Parse(SeedPropertyConstants.Coach2Training6Start),
                   End = DateTime.Parse(SeedPropertyConstants.Coach2Training6End),
                   DayOfWeek = SeedPropertyConstants.Coach2Training6DayOfWeek,
                   CoachId = Guid.Parse(SeedPropertyConstants.CoachId2),
                   IsDeleted = false
               },
                // coach 3 trainings
                new Training()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Coach3Training1Id),
                    Name = SeedPropertyConstants.Coach3Training1Name,
                    Start = DateTime.Parse(SeedPropertyConstants.Coach3Training1Start),
                    End = DateTime.Parse(SeedPropertyConstants.Coach3Training1End),
                    DayOfWeek = SeedPropertyConstants.Coach3Training1DayOfWeek,
                    CoachId = Guid.Parse(SeedPropertyConstants.CoachId3),
                    IsDeleted = false
                },
               new Training()
               {
                   Id = Guid.Parse(SeedPropertyConstants.Coach3Training2Id),
                   Name = SeedPropertyConstants.Coach3Training2Name,
                   Start = DateTime.Parse(SeedPropertyConstants.Coach3Training2Start),
                   End = DateTime.Parse(SeedPropertyConstants.Coach3Training2End),
                   DayOfWeek = SeedPropertyConstants.Coach3Training2DayOfWeek,
                   CoachId = Guid.Parse(SeedPropertyConstants.CoachId3),
                   IsDeleted = false
               },
               new Training()
               {
                   Id = Guid.Parse(SeedPropertyConstants.Coach3Training3Id),
                   Name = SeedPropertyConstants.Coach3Training3Name,
                   Start = DateTime.Parse(SeedPropertyConstants.Coach3Training3Start),
                   End = DateTime.Parse(SeedPropertyConstants.Coach3Training3End),
                   DayOfWeek = SeedPropertyConstants.Coach3Training3DayOfWeek,
                   CoachId = Guid.Parse(SeedPropertyConstants.CoachId3),
                   IsDeleted = false
               },
                 new Training()
                 {
                     Id = Guid.Parse(SeedPropertyConstants.Coach3Training4Id),
                     Name = SeedPropertyConstants.Coach3Training4Name,
                     Start = DateTime.Parse(SeedPropertyConstants.Coach3Training4Start),
                     End = DateTime.Parse(SeedPropertyConstants.Coach3Training4End),
                     DayOfWeek = SeedPropertyConstants.Coach3Training4DayOfWeek,
                     CoachId = Guid.Parse(SeedPropertyConstants.CoachId3),
                     IsDeleted = false
                 },
               new Training()
               {
                   Id = Guid.Parse(SeedPropertyConstants.Coach3Training5Id),
                   Name = SeedPropertyConstants.Coach3Training5Name,
                   Start = DateTime.Parse(SeedPropertyConstants.Coach3Training5Start),
                   End = DateTime.Parse(SeedPropertyConstants.Coach3Training5End),
                   DayOfWeek = SeedPropertyConstants.Coach3Training5DayOfWeek,
                   CoachId = Guid.Parse(SeedPropertyConstants.CoachId3),
                   IsDeleted = false
               },
               new Training()
               {
                   Id = Guid.Parse(SeedPropertyConstants.Coach3Training6Id),
                   Name = SeedPropertyConstants.Coach3Training6Name,
                   Start = DateTime.Parse(SeedPropertyConstants.Coach3Training6Start),
                   End = DateTime.Parse(SeedPropertyConstants.Coach3Training6End),
                   DayOfWeek = SeedPropertyConstants.Coach3Training6DayOfWeek,
                   CoachId = Guid.Parse(SeedPropertyConstants.CoachId3),
                   IsDeleted = false
               });
        }

        private void SeedCoachAndImages(ModelBuilder builder)
        {
            builder.Entity<Coach>()
                .HasData(
                new Coach()
                {
                    Id = Guid.Parse(SeedPropertyConstants.CoachId1),
                    FirstName = SeedPropertyConstants.CoachFirstName1,
                    LastName = SeedPropertyConstants.CoachLastName1,
                    Biography = SeedPropertyConstants.CoachBiography1,
                    IsDeleted = false
                },
                new Coach()
                {
                    Id = Guid.Parse(SeedPropertyConstants.CoachId2),
                    FirstName = SeedPropertyConstants.CoachFirstName2,
                    LastName = SeedPropertyConstants.CoachLastName2,
                    Biography = SeedPropertyConstants.CoachBiography2,
                    IsDeleted = false
                },
                new Coach()
                {
                    Id = Guid.Parse(SeedPropertyConstants.CoachId3),
                    FirstName = SeedPropertyConstants.CoachFirstName3,
                    LastName = SeedPropertyConstants.CoachLastName3,
                    Biography = SeedPropertyConstants.CoachBiography3,
                    IsDeleted = false
                });

            builder.Entity<CoachImageUrl>()
                .HasData(
                new CoachImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Coach1Img1Id),
                    Url = SeedPropertyConstants.Coach1Img1Url,
                    CoachId = Guid.Parse(SeedPropertyConstants.CoachId1)
                },
                new CoachImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Coach2Img1Id),
                    Url = SeedPropertyConstants.Coach2Img1Url,
                    CoachId = Guid.Parse(SeedPropertyConstants.CoachId2)
                },
                new CoachImageUrl()
                {
                    Id = Guid.Parse(SeedPropertyConstants.Coach3Img1Id),
                    Url = SeedPropertyConstants.Coach3Img1Url,
                    CoachId = Guid.Parse(SeedPropertyConstants.CoachId3)
                }
                );
        }

        private void SeedPostsAndImages(ModelBuilder builder)
        {
            builder.Entity<Post>()
                .HasData(
                new Post()
                {
                    Id = Guid.Parse(SeedPropertyConstants.PostId1),
                    Title = SeedPropertyConstants.PostTitle1,
                    Content = SeedPropertyConstants.PostContent1,
                    IsDeleted = false
                },
                new Post()
                {
                    Id = Guid.Parse(SeedPropertyConstants.PostId2),
                    Title = SeedPropertyConstants.PostTitle2,
                    Content = SeedPropertyConstants.PostContent2,
                    IsDeleted = false
                },
                new Post()
                {
                    Id = Guid.Parse(SeedPropertyConstants.PostId3),
                    Title = SeedPropertyConstants.PostTitle3,
                    Content = SeedPropertyConstants.PostContent3,
                    IsDeleted = false
                }
            );


            builder.Entity<PostImageUrl>()
               .HasData(
               new PostImageUrl()
               {
                   Id = Guid.Parse(SeedPropertyConstants.Post1Img1Id),
                   PostId = Guid.Parse(SeedPropertyConstants.PostId1),
                   Url = SeedPropertyConstants.Post1Img1Url
               },
               new PostImageUrl()
               {
                   Id = Guid.Parse(SeedPropertyConstants.Post1Img2Id),
                   PostId = Guid.Parse(SeedPropertyConstants.PostId1),
                   Url = SeedPropertyConstants.Post1Img2Url
               },
               new PostImageUrl()
               {
                   Id = Guid.Parse(SeedPropertyConstants.Post1Img3Id),
                   PostId = Guid.Parse(SeedPropertyConstants.PostId1),
                   Url = SeedPropertyConstants.Post1Img3Url
               },
               new PostImageUrl()
               {
                   Id = Guid.Parse(SeedPropertyConstants.Post2Img1Id),
                   PostId = Guid.Parse(SeedPropertyConstants.PostId2),
                   Url = SeedPropertyConstants.Post2Img1Url
               },
               new PostImageUrl()
               {
                   Id = Guid.Parse(SeedPropertyConstants.Post2Img2Id),
                   PostId = Guid.Parse(SeedPropertyConstants.PostId2),
                   Url = SeedPropertyConstants.Post2Img2Url
               },
               new PostImageUrl()
               {
                   Id = Guid.Parse(SeedPropertyConstants.Post2Img3Id),
                   PostId = Guid.Parse(SeedPropertyConstants.PostId2),
                   Url = SeedPropertyConstants.Post2Img3Url
               },
               new PostImageUrl()
               {
                   Id = Guid.Parse(SeedPropertyConstants.Post3Img1Id),
                   PostId = Guid.Parse(SeedPropertyConstants.PostId3),
                   Url = SeedPropertyConstants.Post3Img1Url
               },
               new PostImageUrl()
               {
                   Id = Guid.Parse(SeedPropertyConstants.Post3Img2Id),
                   PostId = Guid.Parse(SeedPropertyConstants.PostId3),
                   Url = SeedPropertyConstants.Post3Img2Url
               },
               new PostImageUrl()
               {
                   Id = Guid.Parse(SeedPropertyConstants.Post3Img3Id),
                   PostId = Guid.Parse(SeedPropertyConstants.PostId3),
                   Url = SeedPropertyConstants.Post3Img3Url
               }
            );
        }

        private void SeedUsers(ModelBuilder builder)
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();

            builder.Entity<ApplicationUser>()
                .HasData(
                new ApplicationUser()
                {
                    Id = "a27b0f94-b45b-4c9b-8043-25ef63ddd217",
                    FirstName = "Иван",
                    LastName = "Димитров",
                    UserName = "administrator123",
                    NormalizedUserName = "ADMINISTRATOR123",
                    Email = "ivan@abv.bg",
                    NormalizedEmail = "IVAN@ABV.BG",
                    EmailConfirmed = true,
                    PasswordHash = passwordHasher.HashPassword(null, "adminUser123"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = "+359883008292",
                    Deposit = 0
                },
                 new ApplicationUser()
                 {
                     Id = "2a5434f2-4e92-44ee-8b8b-115b943e0ccf",
                     FirstName = "Даниел",
                     LastName = "Николов",
                     UserName = "regularUser",
                     NormalizedUserName = "REGULARUSER",
                     Email = "daniel@abv.bg",
                     NormalizedEmail = "DANIEL@ABV.BG",
                     EmailConfirmed = true,
                     PasswordHash = passwordHasher.HashPassword(null, "regularUser123"),
                     SecurityStamp = Guid.NewGuid().ToString(),
                     ConcurrencyStamp = Guid.NewGuid().ToString(),
                     PhoneNumber = "+359881008822",
                     Deposit = 30M
                 },
                  new ApplicationUser()
                  {
                      Id = "78ce134c-d04f-4aa9-aa2e-59a2c63e80a8",
                      FirstName = "Кристина",
                      LastName = "Желева",
                      UserName = "employee123",
                      NormalizedUserName = "EMPLOYEE123",
                      Email = "kristina@abv.bg",
                      NormalizedEmail = "KRISTINA@ABV.BG",
                      EmailConfirmed = true,
                      PasswordHash = passwordHasher.HashPassword(null, "employeeUser123"),
                      SecurityStamp = Guid.NewGuid().ToString(),
                      ConcurrencyStamp = Guid.NewGuid().ToString(),
                      PhoneNumber = "+359893009911",
                      Deposit = 0
                  }
                );

            builder.Entity<IdentityRole>()
               .HasData(
                new IdentityRole()
                {
                    Id = "595c4c21-7043-48ad-ae83-b2be56e0e157",
                    ConcurrencyStamp = "c38775b2-ac11-4d47-a095-4c2bc311942f",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                },
                new IdentityRole()
                {
                    Id = "f7f9c64e-78ef-4cec-9d58-25bb9d3c9171",
                    ConcurrencyStamp = "3b2c6060-d984-4364-a870-447465f212a4",
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE",
                });

            builder.Entity<IdentityUserRole<string>>()
                .HasData(
                new IdentityUserRole<string>()
                {
                    RoleId = "595c4c21-7043-48ad-ae83-b2be56e0e157",
                    UserId = "a27b0f94-b45b-4c9b-8043-25ef63ddd217"
                },
                new IdentityUserRole<string>()
                {
                    RoleId = "f7f9c64e-78ef-4cec-9d58-25bb9d3c9171",
                    UserId = "78ce134c-d04f-4aa9-aa2e-59a2c63e80a8"
                });
        }
    }
}
