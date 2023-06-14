using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Coach;

namespace RacketSpeed.Controllers
{
    /// <summary>
    /// Provides functionality to the /Coach/ route.
    /// </summary>
    [Authorize(Roles = "Administrator")]
    public class CoachController : Controller
    {
        /// <summary>
        /// Coach service.
        /// </summary>
        private readonly ICoachService coachService;

        /// <summary>
        /// DI Coach service.
        /// </summary>
        /// <param name="coachService">ICoachService.</param>
        public CoachController(ICoachService coachService)
        {
            this.coachService = coachService;
        }

        /// <summary>
        /// Displays a /Coach/All page with all coaches.
        /// </summary>
        /// <returns>/Coach/All page.</returns>
        [AllowAnonymous]
        public async Task<IActionResult> All()
        {
            var models = await this.coachService.AllAsync();

            models = new List<CoachViewModel>()
            {
                new CoachViewModel()
                {
                    FirstName = "Daniel",
                    LastName = "Nikolov",
                    Biography = "I am the best palyer ever the wolrd has seen"
                }
            };

            return View(models);
        }

        /// <summary>
        /// Displays an /Add/ page for Admin users.
        /// </summary>
        /// <returns>/Coach/Add/ page.</returns>
        [HttpGet]
        public IActionResult Add()
        {
            CoachFormModel model = new CoachFormModel();

            return View(model);
        }

        /// <summary>
        /// Gets the data from a HttpPost request and proccesses it.
        /// </summary>
        /// <param name="model">CoachFormModel.</param>
        /// <returns>/Coach/All Page.</returns>
        [HttpPost]
        public async Task<IActionResult> Add(CoachFormModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    ModelState.AddModelError($"{error.Key}", $"{error.Value}");
                }

                return View(model);
            }

            await this.coachService.AddAsync(model);

            return RedirectToAction("All", "Coach");
        }

        /// <summary>
        /// Displays an /Coach/Edit/Id Page.
        /// </summary>
        /// <param name="id">Identificator for Coach Entity.</param>
        /// <returns>/Coach/Edit/Id Page.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var coach = await this.coachService.GetByIdAsync(id, false);

            if (coach == null)
            {
                return RedirectToAction("All", "Coach");
            }

            CoachFormModel model = new CoachFormModel()
            {
              Id = coach.Id,
              FirstName = coach.FirstName,
              LastName = coach.LastName,
              Biography = coach.Biography
            };

            return View(model);
        }

        /// <summary>
        /// Gets the data from a HttpPost request and proccesses it.
        /// </summary>
        /// <param name="model">CoachFormModel.</param>
        /// <returns>/Coach/All Page.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(CoachFormModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    ModelState.AddModelError($"{error.Key}", $"{error.Value}");
                }

                return View(model);
            }

            await this.coachService.EditAsync(model);

            return RedirectToAction("All", "Coach");
        }

        /// <summary>
        /// Deletes an Coach Entity.
        /// </summary>
        /// <param name="id">Identificator for Coach Entity.</param>
        /// <returns>/Coach/All Page.</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await this.coachService.DeleteAsync(id);

            return RedirectToAction("All", "Coach");
        }
    }
}

