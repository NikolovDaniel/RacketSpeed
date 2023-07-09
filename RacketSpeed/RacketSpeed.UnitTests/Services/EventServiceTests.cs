using System;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Event;
using RacketSpeed.Core.Services;
using RacketSpeed.Infrastructure.Data;
using RacketSpeed.Infrastructure.Data.Entities;
using RacketSpeed.Infrastructure.Data.Repositories;
using RacketSpeed.Infrastructure.Data.Seed;
using RacketSpeed.UnitTests.TestsData;

namespace RacketSpeed.UnitTests.Services
{
    public class EventServiceTests
    {
        private DbContextOptions<ApplicationDbContext> options
            = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase("racketspeedtests")
           .Options;

        private Mock<Repository> repository;
        private IEventService eventService;
        private ApplicationDbContext context;
        private Fixture fixture = new Fixture();

        [SetUp]
        public async Task Setup()
        {
            context = new ApplicationDbContext(options);

            await context.Database.EnsureCreatedAsync();

            repository = new Mock<Repository>(context);

            eventService = new EventService(repository.Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await context.Database.EnsureDeletedAsync();
        }

        [Test]
        public async Task MostRecentEventsAsync_ReturnsEventsInCorrectOrder()
        {
            // Arrange
            var numberOfPosts = 3;
            var allEvents = await this.context.Events.ToListAsync();
            var expectedEventEntities = allEvents
                .Where(e => e.IsDeleted == false)
                .OrderByDescending(e => e.Start)
                .Take(numberOfPosts)
                .Select(e => new EventHomePageViewModel()
                {
                    Id = e.Id,
                    Title = e.Title,
                    Content = e.Content,
                    ImageUrl = e.EventImageUrls.FirstOrDefault().Url
                });
            // Act
            var actualEventEntities = await this.eventService.MostRecentEventsAsync();

            // Assert
            actualEventEntities.Should().NotBeNull();
            actualEventEntities.Should().BeEquivalentTo(expectedEventEntities);
        }

        [Test]
        [TestCase(SeedPropertyConstants.EventAdultsId1)]
        [TestCase(SeedPropertyConstants.EventCampId1)]
        [TestCase(SeedPropertyConstants.EventForFunId1)]
        [TestCase(SeedPropertyConstants.EventKidsId1)]
        public async Task GetByIdAsync_WithValidId_ReturnsCorrectEvent(Guid eventId)
        {
            // Arrange
            var eventEntity = await this.context.Events.FindAsync(eventId);
            var expectedEventEntity = new EventDetailsViewModel()
            {
                Id = eventEntity.Id,
                Title = eventEntity.Title,
                Content = eventEntity.Content,
                Start = eventEntity.Start,
                End = eventEntity.End,
                Location = eventEntity.Location,
                Category = eventEntity.Category,
                ImageUrls = eventEntity.EventImageUrls
                .Select(img => img.Url)
                .ToArray()
            };

            // Act
            var actualEventEntity = await this.eventService.GetByIdAsync(eventId);

            // Assert
            actualEventEntity.Should().NotBeNull();
            actualEventEntity.Should().BeEquivalentTo(expectedEventEntity, "it is the same event");
        }

        [Test]
        public async Task EditAsync_WithInvalidImages_DoesNotEdit()
        {
            // Arrange
            var eventId = SeedPropertyConstants.EventAdultsId1;
            var originalEntity = await this.context.Events
               .Include(e => e.EventImageUrls)
               .FirstOrDefaultAsync(e => e.Id == Guid.Parse(eventId));
            var model = new EventFormModel()
            {
                Id = Guid.Parse(eventId),
                Title = SeedPropertyConstants.EventAdultsTitle1,
                Content = SeedPropertyConstants.EventAdultsContent1,
                Start = DateTime.Parse(SeedPropertyConstants.EventAdultsStart1),
                End = DateTime.Parse(SeedPropertyConstants.EventAdultsEnd1),
                Category = SeedPropertyConstants.EventAdultsCategory1,
                Location = SeedPropertyConstants.EventAdultsLocation1,
                ImageUrls = new string[3] { "url 1", "url 2", "url 3" }
            };

            // Act
            await this.eventService.EditAsync(model);

            // Assert
            var modifiedEntity = await this.context.Events
                .Include(e => e.EventImageUrls)
                .FirstOrDefaultAsync(e => e.Id == Guid.Parse(eventId));

            modifiedEntity.Should().NotBeNull();
            modifiedEntity.Should().BeEquivalentTo(originalEntity, "we passed invalid image urls");
        }


        [Test]
        public async Task EditAsync_WithValidId_EditsCorrectly()
        {
            // Arrange
            var eventId = SeedPropertyConstants.EventAdultsId1;
            var model = new EventFormModel()
            {
                Id = Guid.Parse(eventId),
                Title = SeedPropertyConstants.EventAdultsTitle1,
                Content = SeedPropertyConstants.EventAdultsContent1,
                Start = DateTime.Parse(SeedPropertyConstants.EventAdultsStart1),
                End = DateTime.Parse(SeedPropertyConstants.EventAdultsEnd1),
                Category = SeedPropertyConstants.EventAdultsCategory1,
                Location = SeedPropertyConstants.EventAdultsLocation1,
                ImageUrls = new string[3] { "url 1", "url 2", "url 3" }
            };

            // Act
            await this.eventService.EditAsync(model);

            // Assert
            var modifiedEntity = await this.context.Events
                .Include(e => e.EventImageUrls)
                .FirstOrDefaultAsync(e => e.Id == Guid.Parse(eventId));

            modifiedEntity.Should().NotBeNull();
            modifiedEntity.Title.Should().Be(model.Title);
            modifiedEntity.Content.Should().Be(model.Content);
            modifiedEntity.Start.Should().Be(model.Start);
            modifiedEntity.End.Should().Be(model.End);
            modifiedEntity.Category.Should().Be(model.Category);
            modifiedEntity.Location.Should().Be(model.Location);
            modifiedEntity.EventImageUrls.Should().OnlyContain(e => e.Url.Contains("url"));
        }

        [Test]
        [TestCase(SeedPropertyConstants.EventAdultsId1)]
        [TestCase(SeedPropertyConstants.EventCampId1)]
        [TestCase(SeedPropertyConstants.EventForFunId1)]
        [TestCase(SeedPropertyConstants.EventKidsId1)]
        public async Task DeleteAsync__WithValidId_DeletesCorrectly(Guid eventId)
        {
            // Arrange
            var eventEntity = await this.context.Events.FindAsync(eventId);

            // Act
            await this.eventService.DeleteAsync(eventId);

            // Assert
            eventEntity.Should().Match<Event>(c => c.IsDeleted == true, "we deleted the coach");
        }

        [Test]
        [TestCase("Турнири за Любители", false)]
        [TestCase("Турнири за Деца", false)]
        [TestCase("Развлечения", true)]
        [TestCase("Лагери", true)]
        public async Task AllAsync_WithLessThanThreeRecords_ReturnsCorrectEventsForAllRoles(string category, bool isAdministrator)
        {
            // Arrange
            int createCount = 2;
            var eventsForAdults = fixture.Build<Event>()
            .With(e => e.Category, category)
            .With(e => e.IsDeleted, false)
            .With(e => e.Title, SeedPropertyConstants.EventAdultsTitle1)
            .With(e => e.Content, SeedPropertyConstants.EventAdultsContent1)
            .With(e => e.Start, DateTime.Parse(SeedPropertyConstants.EventAdultsStart1))
            .With(e => e.End, DateTime.Parse(SeedPropertyConstants.EventAdultsEnd1))
            .With(e => e.Location, SeedPropertyConstants.EventAdultsLocation1)
            .With(e => e.EventImageUrls,
                        new List<EventImageUrl>()
                        {
                            new EventImageUrl() {Url = "Lorem ipsum"},
                            new EventImageUrl() {Url = "Lorem ipsum"},
                            new EventImageUrl() {Url = "Lorem ipsum"},
                        })
            .CreateMany(createCount);
            var allEntities = await this.context.Events.ToListAsync();
            this.context.RemoveRange(allEntities);
            await this.context.Events.AddRangeAsync(eventsForAdults);
            await this.context.SaveChangesAsync();
            var expectedEventsCount = 2; // because we have hard coded to get maximum of 3 events

            // Act
            var allEvents = await this.eventService.AllAsync(category, isAdministrator);
            var actualEventsCount = allEvents.Count();

            // Assert
            allEvents.Should().OnlyContain(e => e.Category == category);
            actualEventsCount.Should().Be(expectedEventsCount, "we have less than 3 entities in the database");
        }

        [Test]
        [TestCase("Турнири за Любители", false)]
        [TestCase("Турнири за Деца", false)]
        [TestCase("Развлечения", false)]
        [TestCase("Лагери", false)]
        public async Task AllAsync_WithAdministratorRoleFalse_ReturnsCorrectEvents(string category, bool isAdministrator)
        {
            // Arrange
            int createCount = 10;
            var eventsForAdults = fixture.Build<Event>()
            .With(e => e.Category, category)
            .With(e => e.IsDeleted, false)
            .With(e => e.Title, SeedPropertyConstants.EventAdultsTitle1)
            .With(e => e.Content, SeedPropertyConstants.EventAdultsContent1)
            .With(e => e.Start, DateTime.Parse(SeedPropertyConstants.EventAdultsStart1))
            .With(e => e.End, DateTime.Parse(SeedPropertyConstants.EventAdultsEnd1))
            .With(e => e.Location, SeedPropertyConstants.EventAdultsLocation1)
            .With(e => e.EventImageUrls,
                        new List<EventImageUrl>()
                        {
                            new EventImageUrl() {Url = "Lorem ipsum"},
                            new EventImageUrl() {Url = "Lorem ipsum"},
                            new EventImageUrl() {Url = "Lorem ipsum"},
                        })
            .CreateMany(createCount);

            await this.context.Events.AddRangeAsync(eventsForAdults);
            await this.context.SaveChangesAsync();
            var expectedEventsCount = 3; // because we have hard coded to get maximum of 3 events

            // Act
            var allEvents = await this.eventService.AllAsync(category, isAdministrator);
            var actualEventsCount = allEvents.Count();

            // Assert
            allEvents.Should().OnlyContain(e => e.Category == category);
            actualEventsCount.Should().Be(expectedEventsCount, "we are regular user and should receive 3 events");
        }

        [Test]
        [TestCase("Турнири за Любители", true)]
        [TestCase("Турнири за Деца", true)]
        [TestCase("Развлечения", true)]
        [TestCase("Лагери", true)]
        public async Task AllAsync_WithAdministratorRole_ReturnsCorrectEvents(string category, bool isAdministrator)
        {
            // Arrange
            var createCount = 10;
            var eventsForAdults = fixture.Build<Event>()
            .With(e => e.Category, category)
            .With(e => e.IsDeleted, false)
            .With(e => e.Title, SeedPropertyConstants.EventAdultsTitle1)
            .With(e => e.Content, SeedPropertyConstants.EventAdultsContent1)
            .With(e => e.Start, DateTime.Parse(SeedPropertyConstants.EventAdultsStart1))
            .With(e => e.End, DateTime.Parse(SeedPropertyConstants.EventAdultsEnd1))
            .With(e => e.Location, SeedPropertyConstants.EventAdultsLocation1)
            .With(e => e.EventImageUrls,
                        new List<EventImageUrl>()
                        {
                            new EventImageUrl() {Url = "Lorem ipsum"},
                            new EventImageUrl() {Url = "Lorem ipsum"},
                            new EventImageUrl() {Url = "Lorem ipsum"},
                        })
            .CreateMany(createCount);

            await this.context.Events.AddRangeAsync(eventsForAdults);
            await this.context.SaveChangesAsync();
            var expectedEventsCount = await this.context.Events.Where(e => e.IsDeleted == false && e.Category == category).CountAsync();

            // Act
            var allEvents = await this.eventService.AllAsync(category, isAdministrator);
            var actualEventsCount = allEvents.Count();

            // Assert
            allEvents.Should().OnlyContain(e => e.Category == category);
            actualEventsCount.Should().Be(expectedEventsCount, "we are administrator and want all adult events");
        }

        [Test]
        public async Task AddAsync_ModelWithoutImages_DoesNotAdd()
        {
            // Arrange
            var expectedEventCount = await this.context.Events.CountAsync();
            var model = EventServiceTestData.InvalidEventFormModel();

            // Act
            await this.eventService.AddAsync(model);

            // Assert
            var actualEventCount = await this.context.Events.CountAsync();
            actualEventCount.Should().Be(expectedEventCount, "we tried to add invalid entity");
        }

        [Test]
        public async Task AddAsync_WithValidModel_AddsCorrectly()
        {
            // Arrange
            var eventModels = EventServiceTestData.ListWithEventFormModels();
            var eventsCount = await this.context.Events.CountAsync();
            var expectedEventCount = eventsCount + eventModels.Count();

            // Act
            foreach (var model in eventModels)
            {
                await this.eventService.AddAsync(model);
            }

            // Assert
            var actualEventCount = await this.context.Events.CountAsync();
            actualEventCount.Should().Be(expectedEventCount, "we added 3 entities");
        }
    }
}

