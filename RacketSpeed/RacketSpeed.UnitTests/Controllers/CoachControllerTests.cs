using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using RacketSpeed.Controllers;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Coach;
using System.Security.Claims;

namespace RacketSpeed.UnitTests.Controllers
{
    [TestFixture]
    public class CoachControllerTests
    {
        private Mock<ICoachService> _coachServiceMock;
        private CoachController _controller;

        [SetUp]
        public void Setup()
        {
            _coachServiceMock = new Mock<ICoachService>();
            _controller = new CoachController(_coachServiceMock.Object);
        }

        [Test]
        public async Task All_WithValidUser_ReturnsViewResult()
        {
            // Arrange
            var models = new List<CoachViewModel>
            {
                new CoachViewModel { Id = Guid.NewGuid(), FirstName = "Даниел" },
                new CoachViewModel { Id = Guid.NewGuid(), FirstName = "Даниел Н." }
            };
            _coachServiceMock.Setup(service => service.AllAsync()).ReturnsAsync(models);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
               {
                    new Claim(ClaimTypes.Name, "Username"),
                    new Claim(ClaimTypes.Role, "Administrator")
                }, "mock"))
            };
            // Act
            var result = await _controller.All(true);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.Model.Should().BeEquivalentTo(models);
            viewResult.ViewData["IsAdministrator"].Should().Be(true);
        }

        [Test]
        public async Task All_WithInvalidUser_ReturnsUnauthorizedResult()
        {
            // Arrange
            var models = new List<CoachViewModel>
            {
                new CoachViewModel { Id = Guid.NewGuid(), FirstName = "Даниел" },
                new CoachViewModel { Id = Guid.NewGuid(), FirstName = "Даниел Н." }
            };

            _coachServiceMock.Setup(service => service.AllAsync()).ReturnsAsync(models);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new ClaimsIdentity())
            };

            // Act
            var result = await _controller.All(false);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.Model.Should().BeEquivalentTo(models);
            viewResult.ViewData["IsAdministrator"].Should().Be(false);
        }

        [Test]
        public void Add_GET_WithValidUser_ReturnsViewResult()
        {
            // Act
            var result = _controller.Add();

            // Assert
            result.Should().BeOfType<ViewResult>();
            result.As<ViewResult>().Model.Should().BeOfType<CoachFormModel>();
        }

        [Test]
        public async Task Add_POST_WithValidModel_RedirectsToAllAction()
        {
            // Arrange
            var model = new CoachFormModel { FirstName = "Даниел" };

            // Act
            var result = await _controller.Add(model);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.As<RedirectToActionResult>().ActionName.Should().Be("All");
            result.As<RedirectToActionResult>().ControllerName.Should().Be("Coach");
        }

        [Test]
        public async Task Add_POST_WithInvalidModel_ReturnsViewResult()
        {
            // Arrange
            var model = new CoachFormModel();
            _controller.ModelState.AddModelError("Invalid data", "Invalid data");

            // Act
            var result = await _controller.Add(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            result.As<ViewResult>().Model.Should().BeEquivalentTo(model);
            _controller.ModelState.Keys.Should().Contain("Invalid data");
        }

        [Test]
        public async Task Edit_GET_WithValidCoachId_ReturnsViewResult()
        {
            // Arrange
            var coachId = Guid.NewGuid();
            var coach = new CoachTrainingsViewModel { Id = coachId, FirstName = "Даниел Н." };
            _coachServiceMock.Setup(service => service.GetByIdAsync(coachId, true)).ReturnsAsync(coach);

            // Act
            var result = await _controller.Edit(coachId);

            // Assert
            result.Should().BeOfType<ViewResult>();
            result.As<ViewResult>().Model.Should().BeEquivalentTo(coach);
        }

        [Test]
        public async Task Edit_GET_WithInvalidCoachId_ReturnsRedirectToAllAction()
        {
            // Arrange
            var coachId = Guid.NewGuid();
            _coachServiceMock.Setup(service => service.GetByIdAsync(coachId, true)).ReturnsAsync((CoachTrainingsViewModel)null);

            // Act
            var result = await _controller.Edit(coachId);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.As<RedirectToActionResult>().ActionName.Should().Be("All");
            result.As<RedirectToActionResult>().ControllerName.Should().Be("Coach");
        }

        [Test]
        public async Task Edit_POST_WithValidModel_RedirectsToAllAction()
        {
            // Arrange
            var model = new CoachFormModel { Id = Guid.NewGuid(), FirstName = "Нов Даниел" };

            // Act
            var result = await _controller.Edit(model);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.As<RedirectToActionResult>().ActionName.Should().Be("All");
            result.As<RedirectToActionResult>().ControllerName.Should().Be("Coach");
        }

        [Test]
        public async Task Edit_POST_WithInvalidModel_ReturnsViewResult()
        {
            // Arrange
            CoachFormModel model = null;
            _controller.ModelState.AddModelError("Invalid data", "Invalid data");
            // Act
            var result = await _controller.Edit(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            result.As<ViewResult>().Model.Should().BeEquivalentTo(model);
            _controller.ModelState.Keys.Should().Contain("Invalid data");
        }

        [Test]
        public async Task Details_WithValidCoachId_ReturnsViewResult()
        {
            // Arrange
            var coachId = Guid.NewGuid();
            var coach = new CoachTrainingsViewModel { Id = coachId, FirstName = "Даниел Н." };
            _coachServiceMock.Setup(service => service.GetByIdAsync(coachId, true)).ReturnsAsync(coach);

            // Act
            var result = await _controller.Details(coachId);

            // Assert
            result.Should().BeOfType<ViewResult>();
            result.As<ViewResult>().Model.Should().BeEquivalentTo(coach);
        }

        [Test]
        public async Task Details_WithInvalidCoachId_ReturnsRedirectToAllAction()
        {
            // Arrange
            var coachId = Guid.NewGuid();
            _coachServiceMock.Setup(service => service.GetByIdAsync(coachId, true)).ReturnsAsync((CoachTrainingsViewModel)null);

            // Act
            var result = await _controller.Details(coachId);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.As<RedirectToActionResult>().ActionName.Should().Be("All");
            result.As<RedirectToActionResult>().ControllerName.Should().Be("Coach");
        }

        [Test]
        public async Task Delete_WithValidCoachId_RedirectsToAllAction()
        {
            // Arrange
            var coachId = Guid.NewGuid();

            // Act
            var result = await _controller.Delete(coachId);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.As<RedirectToActionResult>().ActionName.Should().Be("All");
            result.As<RedirectToActionResult>().ControllerName.Should().Be("Coach");
        }
    }
}

