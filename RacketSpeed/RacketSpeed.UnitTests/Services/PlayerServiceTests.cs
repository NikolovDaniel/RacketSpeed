using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Player;
using RacketSpeed.Core.Services;
using RacketSpeed.Infrastructure.Data;
using RacketSpeed.Infrastructure.Data.Entities;
using RacketSpeed.Infrastructure.Data.Repositories;
using RacketSpeed.Infrastructure.Data.Seed;

namespace RacketSpeed.UnitTests.Services
{
    public class PlayerServiceTests
    {
        private DbContextOptions<ApplicationDbContext> options
            = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase("racketspeedtests")
           .Options;

        private Mock<Repository> repository;
        private IPlayerService playerService;
        private ApplicationDbContext context;
        private Fixture fixture = new Fixture();

        [SetUp]
        public async Task Setup()
        {
            context = new ApplicationDbContext(options);

            await context.Database.EnsureCreatedAsync();

            repository = new Mock<Repository>(context);

            playerService = new PlayerService(repository.Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await context.Database.EnsureDeletedAsync();
        }

        [Test]
        public async Task PlayersCountAsync_ReturnsCorrectCount()
        {
            // Arrange
            int expectedCount = await this.context.Players.CountAsync();

            // Act
            int actualCount = await this.playerService.PlayersCountAsync();

            // Assert
            actualCount.Should().Be(expectedCount, "we have the same collection");
        }

        [Test]
        [TestCase(SeedPropertyConstants.PlayerId1)]
        [TestCase(SeedPropertyConstants.PlayerId2)]
        [TestCase(SeedPropertyConstants.PlayerId3)]
        [TestCase(SeedPropertyConstants.PlayerId3)]
        public async Task GetByIdAsync_WithValidId_EditsCorrectly(Guid playerId)
        {
            // Arrange
            var player = await this.context.Players
                .Include(p => p.PlayerImageUrl)
                .FirstOrDefaultAsync(p => p.Id == playerId);
            var expectedPlayer = new PlayerFormModel()
            {
                Id = player.Id,
                FirstName = player.FirstName,
                LastName = player.LastName,
                Biography = player.Biography,
                BirthDate = player.BirthDate,
                BirthPlace = player.BirthPlace,
                WorldRanking = player.WorldRanking,
                Height = player.Height,
                PlayingHand = player.PlayingHand,
                NationalRanking = player.NationalRanking,
                CreatedOn = player.CreatedOn,
                ImageUrl = player.PlayerImageUrl.Url
            };
            // Act
            var actualPlayer = await this.playerService.GetByIdAsync(playerId);

            //// Assert
            //actualPlayer.Should().NotBeNull();
            actualPlayer.Should().BeEquivalentTo(expectedPlayer);
        }

        [Test]
        [TestCase(SeedPropertyConstants.PlayerId1)]
        [TestCase(SeedPropertyConstants.PlayerId2)]
        [TestCase(SeedPropertyConstants.PlayerId3)]
        public async Task EditAsync_WithValidId_EditsCorrectly(Guid playerId)
        {
            // Arrange
            var model = new PlayerFormModel()
            {
                Id = playerId,
                FirstName = SeedPropertyConstants.PlayerLastName2,
                LastName = SeedPropertyConstants.PlayerFirstName3,
                WorldRanking = SeedPropertyConstants.PlayerNationalRanking1,
                NationalRanking = SeedPropertyConstants.PlayerWorldRanking3,
                Biography = SeedPropertyConstants.PlayerBiography1,
                BirthDate = DateTime.Parse(SeedPropertyConstants.PlayerBirthDate2),
                BirthPlace = SeedPropertyConstants.PlayerBirthPlace3,
                PlayingHand = SeedPropertyConstants.PlayerPlayingHand2,
                Height = SeedPropertyConstants.PlayerHeight1,
                ImageUrl = "Lorem ipsum"
            };

            // Act
            await this.playerService.EditAsync(model);

            // Assert
            var modifiedEntity = await this.context.Players
               .Include(p => p.PlayerImageUrl)
               .FirstOrDefaultAsync(p => p.Id == playerId);

            modifiedEntity.Should().NotBeNull();
            modifiedEntity.Id.Should().Be(model.Id);
            modifiedEntity.FirstName.Should().Be(model.FirstName);
            modifiedEntity.LastName.Should().Be(model.LastName);
            modifiedEntity.WorldRanking.Should().Be(model.WorldRanking);
            modifiedEntity.NationalRanking.Should().Be(model.NationalRanking);
            modifiedEntity.Biography.Should().Be(model.Biography);
            modifiedEntity.BirthDate.Should().Be(model.BirthDate);
            modifiedEntity.BirthPlace.Should().Be(model.BirthPlace);
            modifiedEntity.PlayingHand.Should().Be(model.PlayingHand);
            modifiedEntity.Height.Should().Be(model.Height);
            modifiedEntity.PlayerImageUrl.Url.Should().Be(model.ImageUrl);
        }

        [Test]
        [TestCase(SeedPropertyConstants.PlayerId1)]
        [TestCase(SeedPropertyConstants.PlayerId2)]
        [TestCase(SeedPropertyConstants.PlayerId3)]
        public async Task DeleteAsync_WithValidId_DeletesCorrectly(Guid playerId)
        {
            // Arrange
            var player = await this.context.Players.FindAsync(playerId);

            // Act
            await this.playerService.DeleteAsync(playerId);

            // Assert
            player.Should().Match<Player>(c => c.IsDeleted == true, "we deleted the player");

        }

        [Test]
        [TestCase(0, 1, "дА")]
        [TestCase(0, 2, "исТ")]
        [TestCase(0, 3, "А")]
        [TestCase(1, 3, "исТ")]
        [TestCase(2, 1, "А")]
        public async Task AllAsync_WithtKeyword_ReturnsCorrectCollection(int start, int playersPerPage, string keyword)
        {
            // Arrange
            int skipCount = (start - 1) * playersPerPage;
            var players = await this.context.Players
                .Where(p => p.IsDeleted == false &&
                            p.FirstName.ToUpper().Contains(keyword.ToUpper()) ||
                            p.LastName.ToUpper().Contains(keyword.ToUpper()))
                .AsNoTracking()
                .Include(p => p.PlayerImageUrl)
                .Where(p => p.IsDeleted == false)
                .OrderBy(p => p.CreatedOn)
                .Skip(skipCount)
                .Take(playersPerPage)
                .ToListAsync();
            var expectedPlayersCollection = players
                .Select(p => new PlayerViewModel()
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    BirthDate = p.BirthDate,
                    BirthPlace = p.BirthPlace,
                    CreatedOn = p.CreatedOn,
                    Biography = p.Biography,
                    WorldRanking = p.WorldRanking,
                    NationalRanking = p.NationalRanking,
                    PlayingHand = p.PlayingHand,
                    Height = p.Height,
                    ImageUrl = p.PlayerImageUrl.Url
                });


            // Act
            var actualPlayersColleciton = await this.playerService.AllAsync(start, playersPerPage, keyword);

            // Assert
            actualPlayersColleciton.Should().NotBeNull();
            actualPlayersColleciton.Should().BeEquivalentTo(expectedPlayersCollection);
        }

        [Test]
        [TestCase(0, 1)]
        [TestCase(0, 2)]
        [TestCase(0, 3)]
        [TestCase(2, 1)]
        [TestCase(2, 3)]
        public async Task AllAsync_WithoutKeyword_ReturnsCorrectCollection(int start, int playersPerPage)
        {
            // Arrange
            int skipCount = (start - 1) * playersPerPage;
            var players = await this.context.Players
                .Where(p => p.IsDeleted == false)
                .AsNoTracking()
                .Include(p => p.PlayerImageUrl)
                .OrderBy(p => p.CreatedOn)
                .Skip(skipCount)
                .Take(playersPerPage)
                .ToListAsync();
            var expectedPlayersCollection = players
                .Select(p => new PlayerViewModel()
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    BirthDate = p.BirthDate,
                    BirthPlace = p.BirthPlace,
                    CreatedOn = p.CreatedOn,
                    Biography = p.Biography,
                    WorldRanking = p.WorldRanking,
                    NationalRanking = p.NationalRanking,
                    PlayingHand = p.PlayingHand,
                    Height = p.Height,
                    ImageUrl = p.PlayerImageUrl.Url
                });


            // Act
            var actualPlayersColleciton = await this.playerService.AllAsync(start, playersPerPage);

            // Assert
            actualPlayersColleciton.Should().NotBeNull();
            actualPlayersColleciton.Should().BeEquivalentTo(expectedPlayersCollection);
        }

        [Test]
        [TestCase(1, "дА")]
        [TestCase(2, "исТ")]
        [TestCase(3, "А")]
        public async Task PlayersPageCount_WithKeyword_ReturnsCorrectCount(int playersPerPage, string keyword)
        {
            // Arrange
            int allPlayersCount = await this.context.Players
                .Where(p => p.IsDeleted == false &&
                       p.FirstName.ToUpper().Contains(keyword.ToUpper()) ||
                       p.LastName.ToUpper().Contains(keyword.ToUpper()))
                .CountAsync();
            int pageCount = (int)Math.Ceiling((allPlayersCount / (double)playersPerPage));
            int expectedPageCount = pageCount == 0 ? 1 : pageCount;
            // Act
            int actualPageCount = this.playerService.PlayersPageCount(playersPerPage, keyword);

            // Assert
            actualPageCount.Should().Be(expectedPageCount);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task PlayersPageCount_WithoutKeyword_ReturnsCorrectCount(int playersPerPage)
        {
            // Arrange
            int allPlayersCount = await this.context.Players.Where(p => p.IsDeleted == false).CountAsync();
            int expectedPageCount = (int)Math.Ceiling((allPlayersCount / (double)playersPerPage));

            // Act
            int actualPageCount = this.playerService.PlayersPageCount(playersPerPage);

            // Assert
            actualPageCount.Should().Be(expectedPageCount);
        }

        [Test]
        public async Task AddAsync_WithValidModel_AddsCorrectly()
        {
            // Arrange
            int createCount = 3;
            var players = fixture.Build<PlayerFormModel>()
                .With(p => p.FirstName, SeedPropertyConstants.PlayerFirstName1)
                .With(p => p.LastName, SeedPropertyConstants.PlayerLastName1)
                .With(p => p.Biography, SeedPropertyConstants.PlayerBiography1)
                .With(p => p.BirthDate, DateTime.Parse(SeedPropertyConstants.PlayerBirthDate1))
                .With(p => p.BirthPlace, SeedPropertyConstants.PlayerBirthPlace1)
                .With(p => p.PlayingHand, SeedPropertyConstants.PlayerPlayingHand1)
                .With(p => p.Height, SeedPropertyConstants.PlayerHeight1)
                .With(p => p.WorldRanking, SeedPropertyConstants.PlayerWorldRanking1)
                .With(p => p.NationalRanking, SeedPropertyConstants.PlayerNationalRanking1)
                .With(p => p.CreatedOn, DateTime.Parse(SeedPropertyConstants.PlayerCreatedOn1))
                .With(p => p.ImageUrl, "test test test")
                .CreateMany(createCount);
            var playersCount = await this.context.Players.CountAsync();
            var expectedPlayerCount = playersCount + players.Count();

            // Act
            foreach (var model in players)
            {
                await this.playerService.AddAsync(model);
            }

            // Assert
            var actualPlayersCount = await this.context.Players.CountAsync();
            actualPlayersCount.Should().Be(expectedPlayerCount, "we added 3 entities");
        }
    }
}

