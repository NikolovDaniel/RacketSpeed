using System;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Player;
using RacketSpeed.Core.Models.Post;
using RacketSpeed.Core.Models.SignKidForm;
using RacketSpeed.Core.Services;
using RacketSpeed.Infrastructure.Data;
using RacketSpeed.Infrastructure.Data.Entities;
using RacketSpeed.Infrastructure.Data.Repositories;
using RacketSpeed.Infrastructure.Data.Seed;

namespace RacketSpeed.UnitTests.Services
{
    public class SignKidServiceTests
    {
        private DbContextOptions<ApplicationDbContext> options
            = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase("racketspeedtests")
           .Options;

        private Mock<Repository> repository;
        private ISignKidService signKidService;
        private ApplicationDbContext context;

        [SetUp]
        public async Task Setup()
        {
            context = new ApplicationDbContext(options);

            await context.Database.EnsureCreatedAsync();

            repository = new Mock<Repository>(context);

            signKidService = new SignKidService(repository.Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await context.Database.EnsureDeletedAsync();
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task SignFormsPageCount_ReturnsCorrectCount(int formsPerPage)
        {
            // Arrange
            int allSignKidFormsCount = await this.context.SignedKids.CountAsync();
            int pageCount = (int)Math.Ceiling((allSignKidFormsCount / (double)formsPerPage));
            int expectedPageCount = pageCount == 0 ? 1 : pageCount;

            // Act
            int actualPageCount = this.signKidService.SignFormsPageCount(formsPerPage);

            // Assert
            actualPageCount.Should().Be(expectedPageCount);
        }


        [Test]
        [TestCase(SeedPropertyConstants.SignFormId1)]
        [TestCase(SeedPropertyConstants.SignFormId2)]
        [TestCase(SeedPropertyConstants.SignFormId3)]
        public async Task GetByIdAsync_WithValidId_ReturnsCorrectPost(Guid signKidFormId)
        {
            // Arrange
            var signKidForm = await this.context.SignedKids.FindAsync(signKidFormId);
            var expectedSignKidFormEntity = new SignKidFormModel()
            {
                FullName = signKidForm.FullName,
                PhoneNumber = signKidForm.PhoneNumber,
                EmailAddress = signKidForm.EmailAddress,
                SignOn = signKidForm.SignOn,
                TrainingType = signKidForm.TrainingType,
                Location = signKidForm.Location,
                PrivacyPolicyIsAccepted = signKidForm.PrivacyPolicyIsAccepted,
                Message = string.IsNullOrEmpty(signKidForm.Message) ? "Няма съобщение" : signKidForm.EmailAddress
            };

            // Act
            var actualSignKidFormEntity = await this.signKidService.GetByIdAsync(signKidFormId);

            // Assert
            actualSignKidFormEntity.Should().NotBeNull();
            actualSignKidFormEntity.Should().BeEquivalentTo(expectedSignKidFormEntity, "it is the same sign kid form");
        }

        [Test]
        [TestCase(SeedPropertyConstants.SignFormId1)]
        [TestCase(SeedPropertyConstants.SignFormId2)]
        [TestCase(SeedPropertyConstants.SignFormId3)]
        public async Task DeleteAsync__WithValidId_DeletesCorrectly(Guid signKidFormId)
        {
            // Arrange
            var signKidForm = await this.context.SignedKids.FindAsync(signKidFormId);

            // Act
            await this.signKidService.DeleteAsync(signKidFormId);

            // Assert
            signKidForm.Should().Match<SignKid>(c => c.IsDeleted == true, "we deleted the sign kid form");
        }

        [Test]
        [TestCase(0, 1)]
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(2, 2)]
        public async Task AllAsync_ReturnsCorrectCollection(int start, int formsPerPage)
        {
            // Arrange
            int skipCount = (start - 1) * formsPerPage;
            var signKidForms = await this.context.SignedKids
                .Where(skf => skf.IsDeleted == false)
                .OrderBy(skf => skf.SignOn)
                .Skip(skipCount)
                .Take(formsPerPage)
                .ToListAsync();
            var expectedSignKidFormsCollection = signKidForms
                .Select(skf => new SignKidFormViewModel()
                {
                    Id = skf.Id,
                    PhoneNumber = skf.PhoneNumber,
                    EmailAddress = skf.EmailAddress,
                    FullName = skf.FullName,
                    SignOn = skf.SignOn
                });

            // Act
            var actualSignKidFormsCollection = await this.signKidService.AllAsync(start, formsPerPage);

            // Assert
            actualSignKidFormsCollection.Should().NotBeNull();
            actualSignKidFormsCollection.Should().BeEquivalentTo(expectedSignKidFormsCollection);
        }

        [Test]
        public async Task AddAsync_WithValidModel_AddsCorrectly()
        {
            // Arrange
            var signKidFormsCount = await this.context.SignedKids.CountAsync();
            var expectedSignKidFormsCount = signKidFormsCount + 1;
            var signKidForm = new SignKidFormModel()
            {
                FullName = SeedPropertyConstants.SignFormFullName1,
                PhoneNumber = SeedPropertyConstants.SignFormPhoneNumber1,
                EmailAddress = SeedPropertyConstants.SignFormEmailAddress1,
                SignOn = DateTime.Now,
                TrainingType = SeedPropertyConstants.SignFormTrainingType1,
                Location = SeedPropertyConstants.SignFormLocation1,
                PrivacyPolicyIsAccepted = true,
                Message = null
            };

            // Act
            await this.signKidService.AddAsync(signKidForm);

            // Assert
            var actualSignKidsFormCount = await this.context.SignedKids.CountAsync();
            actualSignKidsFormCount.Should().Be(expectedSignKidFormsCount, "we added 1 SignKid form");
        }
    }
}

