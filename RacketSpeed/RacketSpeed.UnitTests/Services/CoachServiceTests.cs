using System;
using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Coach;
using RacketSpeed.Core.Models.Training;
using RacketSpeed.Core.Services;
using RacketSpeed.Infrastructure.Data;
using RacketSpeed.Infrastructure.Data.Entities;
using RacketSpeed.Infrastructure.Data.Repositories;
using RacketSpeed.Infrastructure.Data.Seed;
using RacketSpeed.UnitTests.TestsData;

namespace RacketSpeed.UnitTests.Services
{
    public class CoachServiceTests
    {
        private DbContextOptions<ApplicationDbContext> options
           = new DbContextOptionsBuilder<ApplicationDbContext>()
          .UseInMemoryDatabase("racketspeedtests")
          .Options;

        private Mock<Repository> repository;
        private ICoachService coachService;
        private ApplicationDbContext context;

        [SetUp]
        public async Task Setup()
        {
            context = new ApplicationDbContext(options);

            await context.Database.EnsureCreatedAsync();

            repository = new Mock<Repository>(context);

            coachService = new CoachService(repository.Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await context.Database.EnsureDeletedAsync();
        }

        [Test]
        [TestCase("91B835CC-6FD2-45A8-AD89-76CB59913CD6", true)]
        [TestCase("91B835CC-6FD2-45A8-AD89-86CB59913CD6", false)]
        [TestCase("91B835CC-6FD2-45A8-AD89-96CB59913CD6", true)]
        public async Task GetByIdAsync_WithInvalidId_ReturnsNull(Guid coachId, bool withTrainings)
        {
            // Act
            var result = await this.coachService.GetByIdAsync(coachId, withTrainings);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        [TestCase(SeedPropertyConstants.CoachId1, true)]
        [TestCase(SeedPropertyConstants.CoachId1, false)]
        [TestCase(SeedPropertyConstants.CoachId2, true)]
        [TestCase(SeedPropertyConstants.CoachId3, false)]
        public async Task GetByIdAsync_WithValidId_ReturnsCorrectCoachAndTrainings(Guid coachId, bool withTrainings)
        {
            var coach = await this.context.Coaches.FindAsync(coachId);
            var coachTrainings = await this.context.Trainings
                .Where(ct => ct.CoachId == coachId && ct.IsDeleted == false).ToListAsync();
            var expectedResult = new CoachTrainingsViewModel()
            {
                ImageUrl = coach.CoachImageUrl.Url,
                FirstName = coach.FirstName,
                LastName = coach.LastName,
                Biography = coach.Biography,
                Trainings = withTrainings && coachTrainings != null ? coachTrainings
                .Select(t => new TrainingViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    Start = t.Start,
                    End = t.End,
                    DayOfWeek = t.DayOfWeek
                })
                .ToList() : new List<TrainingViewModel>()
            };

            // Act
            var result = await this.coachService.GetByIdAsync(coachId, withTrainings);

            // Assert
            coach.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult, "we passed valid coach id");
        }

        [Test]
        [TestCase("91B835CC-6FD2-45A8-AD89-76CB59913CD6")]
        [TestCase("91B835CC-6FD2-45A8-AD89-86CB59913CD6")]
        [TestCase("91B835CC-6FD2-45A8-AD89-96CB59913CD6")]
        public async Task GetByIdAsync_WithInvalidId_ReturnsNull(Guid coachId)
        {
            // Act
            var result = await this.coachService.GetByIdAsync(coachId);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        [TestCase(SeedPropertyConstants.CoachId1)]
        [TestCase(SeedPropertyConstants.CoachId2)]
        [TestCase(SeedPropertyConstants.CoachId3)]
        public async Task GetByIdAsync_WithValidId_ReturnsCorrectCoach(Guid coachId)
        {
            // Arrange
            var coach = await this.context.Coaches.FindAsync(coachId);
            var expectedResult = new CoachFormModel()
            {
                Id = coach.Id,
                FirstName = coach.FirstName,
                LastName = coach.LastName,
                Biography = coach.Biography,
                ImageUrl = coach.CoachImageUrl.Url
            };

            // Act
            var result = await this.coachService.GetByIdAsync(coachId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult, "we thought we will get this result");
        }

        [Test]
        public async Task EditAsync_WithValidModel_EditsCorrectly()
        {
            // Arrange
            var coach = await this.context.Coaches.FirstAsync();
            var model = new CoachFormModel()
            {
                Id = coach.Id,
                FirstName = "Драган",
                LastName = "Драганов",
                Biography = "Lorem ipsum,Lorem ipsum,Lorem ipsum,Lorem ipsum,Lorem ipsum,",
                ImageUrl = "Lorem ipsum"
            };

            // Act
            await this.coachService.EditAsync(model);

            // Assert
            coach.Should().Match<Coach>(c => c.FirstName == model.FirstName);
            coach.Should().Match<Coach>(c => c.LastName == model.LastName);
            coach.Should().Match<Coach>(c => c.Biography == model.Biography);
            coach.Should().Match<Coach>(c => c.CoachImageUrl.Url == model.ImageUrl);
        }

        [Test]
        [TestCase(SeedPropertyConstants.CoachId1)]
        [TestCase(SeedPropertyConstants.CoachId2)]
        [TestCase(SeedPropertyConstants.CoachId3)]
        public async Task DeleteAsync_WithValidId_DeletesCorrectly(Guid coachId)
        {
            // Arrange
            var coach = await this.context.Coaches.FindAsync(coachId);

            // Act
            await this.coachService.DeleteAsync(coachId);

            // Assert
            coach.Should().Match<Coach>(c => c.IsDeleted == true, "we deleted the coach");
        }

        [Test]
        [TestCase(SeedPropertyConstants.CoachId1, "Понеделник", "2023-06-07 18:00:00.000")]
        [TestCase(SeedPropertyConstants.CoachId1, "Понеделник", "2023-06-07 19:30:00.000")]
        [TestCase(SeedPropertyConstants.CoachId1, "Сряда", "2023-06-07 16:00:00.000")]
        [TestCase(SeedPropertyConstants.CoachId1, "Сряда", "2023-06-07 17:30:00.000")]
        public void HasTraining_WithNoTrainingsAtThatDayAndTime_ReturnsFalse(Guid coachId, string dayOfWeek, DateTime start)
        {
            // Act
            bool result = this.coachService.HasTraining(coachId, dayOfWeek, start);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        [TestCase(SeedPropertyConstants.CoachId1, "Понеделник", "2023-06-07 20:00:00.000")]
        [TestCase(SeedPropertyConstants.CoachId1, "Понеделник", "2023-06-07 20:30:00.000")]
        [TestCase(SeedPropertyConstants.CoachId1, "Понеделник", "2023-06-07 20:45:00.000")]
        [TestCase(SeedPropertyConstants.CoachId1, "Сряда", "2023-06-07 18:00:00.000")]
        [TestCase(SeedPropertyConstants.CoachId1, "Сряда", "2023-06-07 18:30:00.000")]
        [TestCase(SeedPropertyConstants.CoachId1, "Сряда", "2023-06-07 18:45:00.000")]
        public async Task HasTraining_WithOverlapingrainingTime_ReturnsTrue(Guid coachId, string dayOfWeek, DateTime start)
        {
            // Arrange
            var trainings = CoachServiceTestData.ListWithTrainings();
            await this.context.Trainings.AddRangeAsync(trainings);
            await this.context.SaveChangesAsync();

            // Act
            bool result = this.coachService.HasTraining(coachId, dayOfWeek, start);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public async Task AllAsync_ReturnAllCoachesCorrectly()
        {
            // Arrange
            await this.context.Coaches.AddRangeAsync(CoachServiceTestData.ListWithCoaches());
            await this.context.SaveChangesAsync();
            var expectedResult = await this.context.Coaches
                .Include(x => x.CoachImageUrl)
                .Where(c => c.IsDeleted == false)
                .Select(c => new CoachViewModel()
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    ImageUrl = c.CoachImageUrl.Url
                })
                .ToListAsync();

            // Act
            var result = await this.coachService.AllAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task AddAsync_WithValidModel_AddsCorrectly()
        {
            // Arrange
            var expectedResult = await this.context.Coaches.CountAsync();
            var coach = CoachServiceTestData.ValidCoachForm();

            // Act
            await this.coachService.AddAsync(coach);
            var result = await this.context.Coaches.CountAsync();

            // Assert
            result.Should().Be(expectedResult + 1);
        }
    }
}

