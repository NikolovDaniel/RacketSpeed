using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RacketSpeed.Controllers;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Event;
using RacketSpeed.Infrastructure.Data.Seed;

namespace RacketSpeed.UnitTests.Controllers
{
    public class HomeControllerTests
    {
        private HomeController _controller;
        private Mock<IEventService> _eventServiceMock;
        private Mock<IPlayerService> _playerServiceMock;

        [SetUp]
        public void Setup()
        {
            _eventServiceMock = new Mock<IEventService>();
            _playerServiceMock = new Mock<IPlayerService>();
            _controller = new HomeController(_eventServiceMock.Object, _playerServiceMock.Object);
        }

        [Test]
        public async Task Index_GET_ReturnsViewResult()
        {
            // Arrange
            var memberCount = 10;
            var recentEvents = new List<EventHomePageViewModel>
            {
                new EventHomePageViewModel()
                {
                  Id = Guid.NewGuid(),
                  Title = SeedPropertyConstants.EventAdultsTitle1,
                  Content = SeedPropertyConstants.EventAdultsContent1,
                  ImageUrl = "url 1"
                }
             };
            _playerServiceMock.Setup(m => m.PlayersCountAsync()).ReturnsAsync(memberCount);
            _eventServiceMock.Setup(m => m.MostRecentEventsAsync()).ReturnsAsync(recentEvents);

            // Act
            var result = await _controller.Index();

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.Model.Should().NotBeNull();
            var model = viewResult.Model.As<RecentEventsAndPlayerCountViewModel>();
            model.PlayerCount.Should().Be(memberCount);
            model.RecentEvents.Should().BeEquivalentTo(recentEvents);
        }

        [Test]
        public async Task AboutUs_GET_ReturnsViewResult()
        {
            // Arrange
            var memberCount = 5;
            _playerServiceMock.Setup(m => m.PlayersCountAsync()).ReturnsAsync(memberCount);

            // Act
            var result = await _controller.AboutUs();

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
            viewResult.ViewData.Should().ContainKey("memberCount");
            viewResult.ViewData["memberCount"].Should().Be(memberCount);
        }

        [Test]
        public void History_GET_ReturnsViewResult()
        {
            // Act
            var result = _controller.History();

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result.As<ViewResult>();
            viewResult.ViewName.Should().BeNull();
        }
    }
}

