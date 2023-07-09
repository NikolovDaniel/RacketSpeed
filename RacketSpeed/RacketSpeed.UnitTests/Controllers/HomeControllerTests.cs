using Microsoft.AspNetCore.Mvc;
using RacketSpeed.Controllers;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Event;
using Assert = Xunit.Assert;

namespace RacketSpeed.UnitTests.Controllers
{
    public class HomeControllerTests
    {
        private Mock<IEventService> eventService = new Mock<IEventService>();
        private Mock<IPlayerService> playerService = new Mock<IPlayerService>();
        private HomeController homeController;

        [SetUp]
        public void Setup()
        {
            eventService
                .Setup(x => x.MostRecentEventsAsync()).ReturnsAsync(new List<EventHomePageViewModel>() { });

            playerService
             .Setup(x => x.PlayersCountAsync()).ReturnsAsync(3);

            this.homeController = new HomeController(eventService.Object, playerService.Object);
        }

        //[Test]
        //public async Task Index_OnLoad_ReturnsCorrectData()
        //{
        //    // act
        //    var count = new List<string>().Count();

        //    // assert
        //    Assert.Equal(0, count);
        //}
    }
}

