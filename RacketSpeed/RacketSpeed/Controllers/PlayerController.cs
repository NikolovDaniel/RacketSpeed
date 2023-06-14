using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Achievement;
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
        /// Displays a /Player/All page with all players.
        /// </summary>
        /// <returns>/Player/All page.</returns>
        [AllowAnonymous]
        public async Task<IActionResult> All()
        {
            var models = await this.playerService.AllAsync();


            models = new List<PlayerViewModel>()
            {
                new PlayerViewModel()
                {
                    FirstName = "Daniel",
                    LastName = "Nikolov",
                    Biography = "I am the best palyer ever the wolrd has seen",
                    Ranking = 15,
                    BirthDate = DateTime.Parse("26/08/1998"),
                }
            };

            return View(models);
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
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    ModelState.AddModelError($"{error.Key}", $"{error.Value}");
                }

                return View(model);
            }

            await this.playerService.AddAsync(model);

            return RedirectToAction("All", "Player");
        }

        /// <summary>
        /// Displays an /Player/Edit/Id Page.
        /// </summary>
        /// <param name="id">Identificator for Player Entity.</param>
        /// <returns>/Player/Edit/Id Page.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var player = await this.playerService.GetByIdAsync(id);

            if (player == null)
            {
                return RedirectToAction("All", "Player");
            }

            PlayerFormModel model = new PlayerFormModel()
            {
                Id = player.Id,
                FirstName = player.FirstName,
                LastName = player.LastName,
                Biography = player.Biography,
                BirthDate = player.BirthDate,
                Ranking = player.Ranking
            };

            return View(model);
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
                foreach (var error in ModelState)
                {
                    ModelState.AddModelError($"{error.Key}", $"{error.Value}");
                }

                return View(model);
            }

            await this.playerService.EditAsync(model);

            return RedirectToAction("All", "Player");
        }

        /// <summary>
        /// Deletes an Player Entity.
        /// </summary>
        /// <param name="id">Identificator for Player Entity.</param>
        /// <returns>/Player/All Page.</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await this.playerService.DeleteAsync(id);

            return RedirectToAction("All", "Player");
        }
    }
}

