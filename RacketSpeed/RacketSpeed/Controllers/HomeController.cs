using Microsoft.AspNetCore.Mvc;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Event;
using RacketSpeed.Models;
using System.Diagnostics;

namespace RacketSpeed.Controllers
{
    /// <summary>
    /// Provides functionality to the /Home/ route.
    /// Keeps cache for 15 minutes.
    /// </summary>
    [ResponseCache(Duration = 900)]
    public class HomeController : Controller
    {
        /// <summary>
        /// Event service.
        /// </summary>
        private readonly IEventService eventService;

        /// <summary>
        /// Player service.
        /// </summary>
        private readonly IPlayerService playerService;

        /// <summary>
        /// DI Coach and Player services.
        /// </summary>
        /// <param name="eventService">IEventService.</param>
        /// <param name="playerService">IPlayerService.</param>
        public HomeController(IEventService eventService, IPlayerService playerService)
        {
            this.eventService = eventService;
            this.playerService = playerService;
        }

        /// <summary>
        /// Displays a /Home/Index page.
        /// </summary>
        /// <returns>/Home/Index page.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var memberCount = await this.playerService.PlayersCountAsync();
            var recentThreePosts = await this.eventService.MostRecentEventsAsync();

            var model = new RecentEventsAndPlayerCountViewModel()
            {
                PlayerCount = memberCount,
                RecentEvents = recentThreePosts
            };

            return View(model);
        }

        /// <summary>
        /// Displays a /Home/AboutUs page.
        /// </summary>
        /// <returns>/Home/AboutUs page.</returns>
        public async Task<IActionResult> AboutUs()
        {
            var memberCount = await this.playerService.PlayersCountAsync();

            ViewData["memberCount"] = memberCount;

            return View();
        }

        /// <summary>
        /// Displays a /Home/History page.
        /// </summary>
        /// <returns>/Home/History page.</returns>
        public IActionResult History()
        {
            return View();
        }
    }
}