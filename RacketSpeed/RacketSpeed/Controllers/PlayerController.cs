using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Player;

namespace RacketSpeed.Controllers
{
    /// <summary>
    /// Provides functionality to the /Player/ route.
    /// </summary>
    [Authorize(Roles = "Administrator")]
    public class PlayerController : Controller
    {
        /// <summary>
        /// Player service.
        /// </summary>
        private readonly IPlayerService playerService;

        /// <summary>
        /// DI Player service.
        /// </summary>
        /// <param name="playerService">IPlayerService.</param>
        public PlayerController(IPlayerService playerService)
        {
            this.playerService = playerService;
        }

        /// <summary>
        /// Displays a /Player/All page with three players.
        /// </summary>
        /// <returns>/Player/All page.</returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> All(bool isAdministrator, string pageCount = "1")
        {
            ViewData["IsAdministrator"] = isAdministrator;

            int pageNum = int.Parse(pageCount);

            if (pageNum < 1)
            {
                pageNum = 1;
            }

            int playersPerPage = 4;

            var pagesCount = this.playerService.PlayersPageCount(playersPerPage);

            if (pageNum > pagesCount)
            {
                pageNum = pagesCount;
            }

            ViewData["pageNum"] = pageNum;

            var models = await this.playerService.AllAsync(pageNum, playersPerPage);

            return View(new PlayersPaginationCountViewModel()
            {
                PageCount = pagesCount,
                Players = models
            });
        }

        /// <summary>
        /// Displays a /Player/All page with three players by keyword.
        /// </summary>
        /// <returns>/Player/All/{keyword} page.</returns>
        /// <param name="keyword">String used to filter Player Entities.</param>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> AllPlayersByKeyword(string keyword, bool isAdministrator, string pageCount = "1")
        {
            ViewData["IsAdministrator"] = isAdministrator;

            if (string.IsNullOrEmpty(keyword))
            {
                ModelState.AddModelError("KeywordError", "Полето трябва да съдържа поне 1 символ.");
                return View();
            }

            int pageNum = int.Parse(pageCount);

            int playersPerPage = 5;

            var pagesCount = this.playerService.PlayersPageCount(playersPerPage);
            pageNum = CalculateValidPageNum(pageNum, pagesCount);

            var players = await playerService.AllAsync(pageNum, playersPerPage, keyword);

            ViewData["keyword"] = keyword;
            ViewData["pageNum"] = pageNum;

            return View(new PlayersPaginationCountViewModel()
            {
                Players = players,
                PageCount = pagesCount
            });
        }

        /// <summary>
        /// Displays an /Add/ page for Admin users.
        /// </summary>
        /// <returns>/Player/Add/ page.</returns>
        [HttpGet]
        public IActionResult Add()
        {
            PlayerFormModel model = new PlayerFormModel();

            return View(model);
        }

        /// <summary>
        /// Gets the data from a HttpPost request and proccesses it.
        /// </summary>
        /// <param name="model">PlayerFormModel.</param>
        /// <returns>/Player/All Page.</returns>
        [HttpPost]
        public async Task<IActionResult> Add(PlayerFormModel model)
        {
            model.CreatedOn = DateTime.Now;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await this.playerService.AddAsync(model);

            return RedirectToAction("All", "Player");
        }

        /// <summary>
        /// Displays an /Player/Edit/Id Page.
        /// </summary>
        /// <param name="playerId">Identificator for Player Entity.</param>
        /// <returns>/Player/Edit/Id Page.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid playerId)
        {
            var player = await this.playerService.GetByIdAsync(playerId);

            if (player == null)
            {
                return RedirectToAction("All", "Player");
            }

            return View(player);
        }

        /// <summary>
        /// Gets the data from a HttpPost request and proccesses it.
        /// </summary>
        /// <param name="model">PlayerFormModel.</param>
        /// <returns>/Player/All Page.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(PlayerFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await this.playerService.EditAsync(model);

            return RedirectToAction("All", "Player");
        }

        /// <summary>
        /// Deletes an Player Entity.
        /// </summary>
        /// <param name="playerId">Identificator for Player Entity.</param>
        /// <returns>/Player/All Page.</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(Guid playerId)
        {
            await this.playerService.DeleteAsync(playerId);

            return RedirectToAction("All", "Player");
        }

        /// <summary>
        /// Method to calculate the right page number.
        /// </summary>
        /// <param name="pageNum">Page number that the user wants.</param>
        /// <param name="pagesCount">All pages available.</param>
        /// <returns>Integer which is the correct page number.</returns>
        private int CalculateValidPageNum(int pageNum, int pagesCount)
        {
            if (pageNum <= 0)
            {
                return 1;
            }

            if (pageNum > pagesCount)
            {
                return pagesCount;
            }

            return pageNum;
        }
    }
}

