using System;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RacketSpeed.Controllers;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Player;
using RacketSpeed.Infrastructure.Data.Seed;

namespace RacketSpeed.UnitTests.Controllers
{
    public class PlayerControllerTests
    {
        private PlayerController _controller;
        private Mock<IPlayerService> _playerServiceMock;
        private ClaimsPrincipal RegularUser = new ClaimsPrincipal(new ClaimsIdentity(new[]
         {
                new Claim(ClaimTypes.Name, "Username"),
                new Claim(ClaimTypes.Role, "RegularUser")
            }, "mock"));

        [SetUp]
        public void Setup()
        {
            _playerServiceMock = new Mock<IPlayerService>();
            _controller = new PlayerController(_playerServiceMock.Object);
        }

        [Test]
        public async Task All_GET_ReturnsViewResult()
        {
            // Arrange
            var pageCount = "1";
            var pageNum = 1;
            var playersPerPage = 4;
            var pagesCount = 2;
            var players = new List<PlayerViewModel>
            {
                new PlayerViewModel()
                {
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
                }
            };
            _playerServiceMock.Setup(m => m.PlayersPageCount(playersPerPage)).Returns(pagesCount);
            _playerServiceMock.Setup(m => m.AllAsync(pageNum, playersPerPage)).ReturnsAsync(players);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = RegularUser };

            // Act
            var result = await _controller.All(false, pageCount);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().NotBeNull();
            var model = viewResult.Model.As<PlayersPaginationCountViewModel>();
            model.Players.Should().BeEquivalentTo(players);
            model.PageCount.Should().Be(pagesCount);
            viewResult.ViewData.Should().ContainKey("IsAdministrator");
            viewResult.ViewData["IsAdministrator"].Should().Be(false);
            viewResult.ViewData.Should().ContainKey("pageNum");
            viewResult.ViewData["pageNum"].Should().Be(pageNum);
        }

        [Test]
        public async Task AllPlayersByKeyword_GET_ReturnsViewResult()
        {
            // Arrange
            var pageCount = "1";
            var keyword = "test";
            var pageNum = 1;
            var playersPerPage = 5;
            var pagesCount = 3;
            var players = new List<PlayerViewModel>
            {
                 new PlayerViewModel()
                 {
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
                 }
            };
            _playerServiceMock.Setup(m => m.PlayersPageCount(playersPerPage, keyword)).Returns(pagesCount);
            _playerServiceMock.Setup(m => m.AllAsync(pageNum, playersPerPage, keyword)).ReturnsAsync(players);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = RegularUser };

            // Act
            var result = await _controller.AllPlayersByKeyword(keyword, false, pageCount);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().NotBeNull();
            var model = viewResult.Model.As<PlayersPaginationCountViewModel>();
            model.Players.Should().BeEquivalentTo(players);
            model.PageCount.Should().Be(pagesCount);
            viewResult.ViewData.Should().ContainKey("IsAdministrator");
            viewResult.ViewData["IsAdministrator"].Should().Be(false);
            viewResult.ViewData.Should().ContainKey("keyword");
            viewResult.ViewData["keyword"].Should().Be(keyword);
            viewResult.ViewData.Should().ContainKey("pageNum");
            viewResult.ViewData["pageNum"].Should().Be(pageNum);
        }

        [Test]
        public void Add_GET_ReturnsViewResult()
        {
            // Act
            var result = _controller.Add();

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().NotBeNull();
            viewResult.Model.Should().BeOfType<PlayerFormModel>();
        }

        [Test]
        public async Task Add_POST_WithValidModel_ReturnsRedirectToAction()
        {
            // Arrange
            var model = new PlayerFormModel
            {
                FirstName = SeedPropertyConstants.PlayerFirstName1,
                LastName = SeedPropertyConstants.PlayerLastName2,
                WorldRanking = SeedPropertyConstants.PlayerWorldRanking3,
                Biography = SeedPropertyConstants.PlayerBiography1,
                BirthDate = DateTime.Parse(SeedPropertyConstants.PlayerBirthDate2),
                CreatedOn = DateTime.Parse(SeedPropertyConstants.PlayerCreatedOn3),
                NationalRanking = SeedPropertyConstants.PlayerNationalRanking2,
                BirthPlace = SeedPropertyConstants.PlayerBirthPlace1,
                Height = SeedPropertyConstants.PlayerHeight2,
                PlayingHand = SeedPropertyConstants.PlayerPlayingHand3,
                ImageUrl = "url 1"
            };
            _playerServiceMock.Setup(m => m.AddAsync(model)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Add(model);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result.As<RedirectToActionResult>();
            redirectResult.ActionName.Should().Be("All");
            redirectResult.ControllerName.Should().Be("Player");
        }

        [Test]
        public async Task Add_POST_WithInvalidModel_ReturnsViewResult()
        {
            // Arrange
            var model = new PlayerFormModel
            {
                FirstName = "Даниел .Н"
            };
            _controller.ModelState.AddModelError("InvalidProperty", "Invalid property value");

            // Act
            var result = await _controller.Add(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().Be(model);
        }

        [Test]
        public async Task Edit_GET_WithValidId_ReturnsViewResult()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var player = new PlayerFormModel
            {
                FirstName = SeedPropertyConstants.PlayerFirstName1,
                LastName = SeedPropertyConstants.PlayerLastName2,
                WorldRanking = SeedPropertyConstants.PlayerWorldRanking3,
                Biography = SeedPropertyConstants.PlayerBiography1,
                BirthDate = DateTime.Parse(SeedPropertyConstants.PlayerBirthDate2),
                CreatedOn = DateTime.Parse(SeedPropertyConstants.PlayerCreatedOn3),
                NationalRanking = SeedPropertyConstants.PlayerNationalRanking2,
                BirthPlace = SeedPropertyConstants.PlayerBirthPlace1,
                Height = SeedPropertyConstants.PlayerHeight2,
                PlayingHand = SeedPropertyConstants.PlayerPlayingHand3,
                ImageUrl = "url 1"
            };
            _playerServiceMock.Setup(m => m.GetByIdAsync(playerId)).ReturnsAsync(player);

            // Act
            var result = await _controller.Edit(playerId);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().NotBeNull();
            var model = viewResult.Model.As<PlayerFormModel>();
            model.Should().Be(player);
        }

        [Test]
        public async Task Edit_GET_WithInvalidId_ReturnsRedirectToAction()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            _playerServiceMock.Setup(m => m.GetByIdAsync(playerId)).ReturnsAsync((PlayerFormModel)null);

            // Act
            var result = await _controller.Edit(playerId);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result.As<RedirectToActionResult>();
            redirectResult.ActionName.Should().Be("All");
            redirectResult.ControllerName.Should().Be("Player");
        }

        [Test]
        public async Task Edit_POST_WithValidModel_ReturnsRedirectToAction()
        {
            // Arrange
            var model = new PlayerFormModel
            {
                FirstName = SeedPropertyConstants.PlayerFirstName1,
                LastName = SeedPropertyConstants.PlayerLastName2,
                WorldRanking = SeedPropertyConstants.PlayerWorldRanking3,
                Biography = SeedPropertyConstants.PlayerBiography1,
                BirthDate = DateTime.Parse(SeedPropertyConstants.PlayerBirthDate2),
                CreatedOn = DateTime.Parse(SeedPropertyConstants.PlayerCreatedOn3),
                NationalRanking = SeedPropertyConstants.PlayerNationalRanking2,
                BirthPlace = SeedPropertyConstants.PlayerBirthPlace1,
                Height = SeedPropertyConstants.PlayerHeight2,
                PlayingHand = SeedPropertyConstants.PlayerPlayingHand3,
                ImageUrl = "url 1"
            };
            _playerServiceMock.Setup(m => m.EditAsync(model)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Edit(model);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result.As<RedirectToActionResult>();
            redirectResult.ActionName.Should().Be("All");
            redirectResult.ControllerName.Should().Be("Player");
        }

        [Test]
        public async Task Edit_POST_WithInvalidModel_ReturnsViewResult()
        {
            // Arrange
            var model = new PlayerFormModel
            {
                FirstName = "Даниел Н."
            };
            _controller.ModelState.AddModelError("InvalidProperty", "Invalid property value");

            // Act
            var result = await _controller.Edit(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().Be(model);
        }

        [Test]
        public async Task Delete_POST_ReturnsRedirectToAction()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            _playerServiceMock.Setup(m => m.DeleteAsync(playerId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(playerId);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result.As<RedirectToActionResult>();
            redirectResult.ActionName.Should().Be("All");
            redirectResult.ControllerName.Should().Be("Player");
        }
    }
}

