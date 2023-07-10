using System;
using System.Xml.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.SignKidForm;
using RacketSpeed.Core.Models.Training;
using RacketSpeed.Core.Services;
using RacketSpeed.Infrastructure.Data;
using RacketSpeed.Infrastructure.Data.Entities;
using RacketSpeed.Infrastructure.Data.Repositories;
using RacketSpeed.Infrastructure.Data.Seed;

namespace RacketSpeed.UnitTests.Services
{
    public class TrainingServiceTests
    {
        private DbContextOptions<ApplicationDbContext> options
            = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase("racketspeedtests")
           .Options;

        private Mock<Repository> repository;
        private ITrainingService trainingService;
        private ApplicationDbContext context;

        [SetUp]
        public async Task Setup()
        {
            context = new ApplicationDbContext(options);

            await context.Database.EnsureCreatedAsync();

            repository = new Mock<Repository>(context);

            trainingService = new TrainingService(repository.Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await context.Database.EnsureDeletedAsync();
        }

        [Test]
        [TestCase(SeedPropertyConstants.Coach1Training1Id)]
        [TestCase(SeedPropertyConstants.Coach2Training2Id)]
        [TestCase(SeedPropertyConstants.Coach3Training3Id)]
        public async Task GetByIdAsync_ReturnsCorrectTraining(Guid trainingId)
        {
            // Arrange
            var expectedTraining = await this.context.Trainings.FindAsync(trainingId);

            // Act
            var actualTraining = await this.trainingService.GetByIdAsync(trainingId);

            // Assert
            actualTraining.Should().NotBeNull();
            actualTraining.Id.Should().Be(expectedTraining.Id);
            actualTraining.Name.Should().Be(expectedTraining.Name);
            actualTraining.Start.Should().Be(expectedTraining.Start);
            actualTraining.End.Should().Be(expectedTraining.End);
            actualTraining.DayOfWeek.Should().Be(expectedTraining.DayOfWeek);
            actualTraining.CoachId.Should().Be(expectedTraining.CoachId);
        }

        [Test]
        [TestCase(SeedPropertyConstants.Coach1Training1Id, SeedPropertyConstants.CoachId1)]
        [TestCase(SeedPropertyConstants.Coach2Training1Id, SeedPropertyConstants.CoachId3)]
        [TestCase(SeedPropertyConstants.Coach3Training1Id, SeedPropertyConstants.CoachId2)]
        public async Task EditAsync_WithValidModel_EditsCorrectly(Guid trainingId, Guid coachId)
        {
            // Arrange
            var trainingModel = new TrainingFormModel()
            {
                Id = trainingId,
                Name = SeedPropertyConstants.Coach1Training1Name,
                Start = DateTime.Parse(SeedPropertyConstants.Coach2Training1Start),
                End = DateTime.Parse(SeedPropertyConstants.Coach2Training1End),
                DayOfWeek = SeedPropertyConstants.Coach1Training3DayOfWeek,
                CoachId = coachId
            };
            // Act
            await this.trainingService.EditAsync(trainingModel);

            // Assert
            var modifiedEntity = await this.context.Trainings.FindAsync(trainingId);

            modifiedEntity.Should().NotBeNull();
            modifiedEntity.Id.Should().Be(trainingModel.Id);
            modifiedEntity.Name.Should().Be(trainingModel.Name);
            modifiedEntity.Start.Should().Be(trainingModel.Start);
            modifiedEntity.End.Should().Be(trainingModel.End);
            modifiedEntity.DayOfWeek.Should().Be(trainingModel.DayOfWeek);
            modifiedEntity.CoachId.Should().Be(trainingModel.CoachId);
        }

        [Test]
        [TestCase(SeedPropertyConstants.Coach1Training1Id)]
        [TestCase(SeedPropertyConstants.Coach2Training1Id)]
        [TestCase(SeedPropertyConstants.Coach3Training1Id)]
        public async Task DeleteAsync__WithValidId_DeletesCorrectly(Guid trainingId)
        {
            // Arrange
            var training = await this.context.Trainings.FindAsync(trainingId);

            // Act
            await this.trainingService.DeleteAsync(trainingId);

            // Assert
            training.Should().Match<Training>(c => c.IsDeleted == true, "we deleted the training");
        }

        [Test]
        [TestCase("Деца до 7")]
        [TestCase("Деца до 11")]
        [TestCase("Деца до 15")]
        [TestCase("Деца до 19")]
        public async Task AllAsync_WithTrainingName_ReturnsCorrectCollection(string trainingName)
        {
            // Arrange
            var allTrainings = await this.context.Trainings
                .Where(t => t.IsDeleted == false && t.Name.ToUpper() == trainingName.ToUpper())
                .ToListAsync();
            var expectedTrainingsCollection = allTrainings
                .Select(t => new TrainingViewModel()
                {
                    Name = t.Name,
                    Start = t.Start,
                    End = t.End,
                    DayOfWeek = t.DayOfWeek
                });

            // Act
            var actualTrainingsCollection = await this.trainingService.AllAsync(trainingName);

            // Assert
            actualTrainingsCollection.Should().NotBeNull();
            actualTrainingsCollection.Should().BeEquivalentTo(expectedTrainingsCollection);
        }


        [Test]
        public async Task AllAsync_WithoutTrainingName_ReturnsCorrectCollection()
        {
            // Arrange
            var allTrainings = await this.context.Trainings
                .Where(t => t.IsDeleted == false)
                .ToListAsync();
            var expectedTrainingsCollection = allTrainings
                .Select(t => new TrainingViewModel()
                {
                    Id = t.Id,
                    Name = t.Name
                });

            // Act
            var actualTrainingsCollection = await this.trainingService.AllAsync();

            // Assert
            actualTrainingsCollection.Should().NotBeNull();
            actualTrainingsCollection.Should().BeEquivalentTo(expectedTrainingsCollection);
        }

        [Test]
        [TestCase(SeedPropertyConstants.CoachId1)]
        [TestCase(SeedPropertyConstants.CoachId2)]
        [TestCase(SeedPropertyConstants.CoachId3)]
        public async Task AddAsync_WithValidModel_AddsCorrectly(Guid coachId)
        {
            // Arrange
            var currentTrainingsCount = await this.context.Trainings.CountAsync();
            var expectedTrainingsCount = currentTrainingsCount + 1;
            var trainingModel = new TrainingFormModel()
            {
                Name = SeedPropertyConstants.Coach1Training1Name,
                Start = DateTime.Parse(SeedPropertyConstants.Coach2Training1Start),
                End = DateTime.Parse(SeedPropertyConstants.Coach2Training1End),
                DayOfWeek = SeedPropertyConstants.Coach1Training3DayOfWeek,
                CoachId = coachId
            };

            // Act
            await this.trainingService.AddAsync(trainingModel);

            // Assert
            var actualTrainingsCount = await this.context.Trainings.CountAsync();
            actualTrainingsCount.Should().Be(expectedTrainingsCount, "we added 1 training form");
        }
    }
}

