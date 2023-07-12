using System;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RacketSpeed.Controllers;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Event;
using RacketSpeed.Infrastructure.Data.Seed;

namespace RacketSpeed.UnitTests.Controllers
{
    public class EventControllerTests
    {
        private EventController _controller;
        private Mock<IEventService> _eventServiceMock;

        [SetUp]
        public void Setup()
        {
            _eventServiceMock = new Mock<IEventService>();
            _controller = new EventController(_eventServiceMock.Object);
        }

        [Test]
        public async Task All_GET_ReturnsViewResult()
        {
            // Arrange
            ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "Username"),
                new Claim(ClaimTypes.Role, "Administrator")
            }, "mock"));

            _eventServiceMock
                .Setup(e => e.AllAsync(It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<EventViewModel>() { new EventViewModel() { Id = Guid.NewGuid() } });
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // Act
            var result = await _controller.All("Лагери", false);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().NotBeNull();
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
        }

        [Test]
        public async Task Add_POST_WithValidModel_RedirectsToAll()
        {
            // Arrange
            var model = new EventFormModel
            {
                Id = Guid.NewGuid(),
                Title = SeedPropertyConstants.EventAdultsTitle1,
                Content = SeedPropertyConstants.EventAdultsContent1,
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(5),
                Category = SeedPropertyConstants.EventAdultsCategory1,
                Location = SeedPropertyConstants.EventAdultsLocation1,
                ImageUrls = new string[3] { "url 1", "url 2", "url 3" }
            };

            // Act
            var result = await _controller.Add(model);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result.As<RedirectToActionResult>();
            redirectResult.ActionName.Should().Be("All");
            redirectResult.ControllerName.Should().Be("Event");
            redirectResult.RouteValues.Should().ContainKey("category");
            redirectResult.RouteValues["category"].Should().Be(model.Category);
        }

        [Test]
        public async Task Add_POST_WithImagesNull_ReturnsViewResult()
        {
            // Arrange
            var model = new EventFormModel()
            {
                ImageUrls = new string[3] {"url 1", "url 2", null}
            };

            // Act
            var result = await _controller.Add(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().BeEquivalentTo(model);
            _controller.ModelState.Should().ContainKey("UrlEmpty");
        }

        [Test]
        public async Task Add_POST_WithWrongDates_ReturnsViewResult()
        {
            // Arrange
            var model = new EventFormModel()
            {
                Start = DateTime.Now.AddDays(1),
                End = DateTime.Now
            };

            // Act
            var result = await _controller.Add(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().BeEquivalentTo(model);
            _controller.ModelState.Should().ContainKey("InvalidDates");
        }

        [Test]
        public async Task Add_POST_WithInvalidModel_ReturnsViewResult()
        {
            // Arrange
            var model = new EventFormModel();
            _controller.ModelState.AddModelError("InvalidProperty", "Invalid property error message");

            // Act
            var result = await _controller.Add(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().Be(model);
        }

        [Test]
        public async Task Edit_GET_WithExistingEventId_ReturnsViewResult()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var eventEntity = new EventDetailsViewModel
            {
                Title = SeedPropertyConstants.EventAdultsTitle1,
                Content = SeedPropertyConstants.EventAdultsContent1,
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(5),
                Category = SeedPropertyConstants.EventAdultsCategory1,
                Location = SeedPropertyConstants.EventAdultsLocation1,
                ImageUrls = new string[3] { "url 1", "url 2", "url 3" }
            };
            _eventServiceMock.Setup(m => m.GetByIdAsync(eventId)).ReturnsAsync(eventEntity);

            // Act
            var result = await _controller.Edit(eventId);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().NotBeNull();
        }

        [Test]
        public async Task Edit_GET_WithNonExistingEventId_RedirectsToAll()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            _eventServiceMock.Setup(m => m.GetByIdAsync(eventId)).ReturnsAsync((EventDetailsViewModel)null);

            // Act
            var result = await _controller.Edit(eventId);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result.As<RedirectToActionResult>();
            redirectResult.ActionName.Should().Be("All");
            redirectResult.ControllerName.Should().Be("Event");
            redirectResult.RouteValues.Should().ContainKey("category");
            redirectResult.RouteValues["category"].Should().Be("Лагери");
        }

        [Test]
        public async Task Edit_POST_WithValidModel_RedirectsToAll()
        {
            // Arrange
            var model = new EventFormModel
            {
                Id = Guid.NewGuid(),
                Title = SeedPropertyConstants.EventAdultsTitle1,
                Content = SeedPropertyConstants.EventAdultsContent1,
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(5),
                Category = SeedPropertyConstants.EventAdultsCategory1,
                Location = SeedPropertyConstants.EventAdultsLocation1,
                ImageUrls = new string[3] { "url 1", "url 2", "url 3" }
            };

            // Act
            var result = await _controller.Edit(model);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result.As<RedirectToActionResult>();
            redirectResult.ActionName.Should().Be("All");
            redirectResult.ControllerName.Should().Be("Event");
            redirectResult.RouteValues.Should().ContainKey("category");
            redirectResult.RouteValues["category"].Should().Be(model.Category);
        }

        [Test]
        public async Task Edit_POST_WithImagesNull_ReturnsViewResult()
        {
            // Arrange
            var model = new EventFormModel()
            {
                ImageUrls = new string[3] { "url 1", "url 2", null }
            };

            // Act
            var result = await _controller.Edit(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().BeEquivalentTo(model);
            _controller.ModelState.Should().ContainKey("UrlEmpty");
        }

        [Test]
        public async Task Edit_POST_WithWrongDates_ReturnsViewResult()
        {
            // Arrange
            var model = new EventFormModel()
            {
                Start = DateTime.Now.AddDays(1),
                End = DateTime.Now
            };

            // Act
            var result = await _controller.Edit(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().BeEquivalentTo(model);
            _controller.ModelState.Should().ContainKey("InvalidDates");
        }

        [Test]
        public async Task Edit_POST_WithInvalidModel_ReturnsViewResult()
        {
            // Arrange
            var model = new EventFormModel();
            _controller.ModelState.AddModelError("InvalidProperty", "Invalid property error message");

            // Act
            var result = await _controller.Edit(model);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().Be(model);
        }

        [Test]
        public async Task Details_WithExistingEventId_ReturnsViewResult()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var eventEntity = new EventDetailsViewModel
            {
                Title = SeedPropertyConstants.EventAdultsTitle1,
                Content = SeedPropertyConstants.EventAdultsContent1,
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(5),
                Category = SeedPropertyConstants.EventAdultsCategory1,
                Location = SeedPropertyConstants.EventAdultsLocation1,
                ImageUrls = new string[3] { "url 1", "url 2", "url 3" }
            };
            _eventServiceMock.Setup(m => m.GetByIdAsync(eventId)).ReturnsAsync(eventEntity);

            // Act
            var result = await _controller.Details(eventId);

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().NotBeNull();
        }

        [Test]
        public async Task Details_WithNonExistingEventId_RedirectsToAll()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            _eventServiceMock.Setup(m => m.GetByIdAsync(eventId)).ReturnsAsync((EventDetailsViewModel)null);

            // Act
            var result = await _controller.Details(eventId);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result.As<RedirectToActionResult>();
            redirectResult.ActionName.Should().Be("All");
            redirectResult.ControllerName.Should().Be("Event");
        }

        [Test]
        public async Task Delete_WithExistingEventId_RedirectsToAll()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var model = new EventDetailsViewModel
            {
                Title = SeedPropertyConstants.EventAdultsTitle1,
                Content = SeedPropertyConstants.EventAdultsContent1,
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(5),
                Category = SeedPropertyConstants.EventAdultsCategory1,
                Location = SeedPropertyConstants.EventAdultsLocation1,
                ImageUrls = new string[3] { "url 1", "url 2", "url 3" }
            };
            _eventServiceMock.Setup(m => m.GetByIdAsync(eventId)).ReturnsAsync(model);

            // Act
            var result = await _controller.Delete(eventId);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result.As<RedirectToActionResult>();
            redirectResult.ActionName.Should().Be("All");
            redirectResult.ControllerName.Should().Be("Event");
            redirectResult.RouteValues.Should().ContainKey("category");
            redirectResult.RouteValues["category"].Should().Be(model.Category);
        }

        [Test]
        public async Task Delete_WithNonExistingEventId_RedirectsToAll()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            _eventServiceMock.Setup(m => m.GetByIdAsync(eventId)).ReturnsAsync((EventDetailsViewModel)null);

            // Act
            var result = await _controller.Delete(eventId);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result.As<RedirectToActionResult>();
            redirectResult.ActionName.Should().Be("All");
            redirectResult.ControllerName.Should().Be("Event");
        }
    }
}

