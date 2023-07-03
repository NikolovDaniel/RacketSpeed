using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Coach;

namespace RacketSpeed.Controllers
{
    /// <summary>
    /// Provides functionality to the /Coach/ route.
    /// Keeps cache for 30 minutes.
    /// </summary>
    [ResponseCache(Duration = 1800)]
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
        [HttpGet]
        public async Task<IActionResult> All(bool isAdministrator)
        {
            var models = await this.coachService.AllAsync();

            ViewData["IsAdministrator"] = isAdministrator;

            return View(models);
        }

        /// <summary>
        /// Displays an Coach/Add/ page for Admin users.
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
                return View(model);
            }

            await this.coachService.AddAsync(model);

            return RedirectToAction("All", "Coach");
        }

        /// <summary>
        /// Displays an /Coach/Edit/Id Page.
        /// </summary>
        /// <param name="coachId">Identificator for Coach Entity.</param>
        /// <returns>/Coach/Edit/Id Page.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid coachId)
        {
            bool withTrainings = true;

            var coach = await this.coachService.GetByIdAsync(coachId, withTrainings);

            if (coach == null)
            {
                return RedirectToAction("All", "Coach");
            }

            return View(coach);
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
                return View(model);
            }

            await this.coachService.EditAsync(model);

            return RedirectToAction("All", "Coach");
        }

        /// <summary>
        /// Display a /Coach/Details/Id Page.
        /// </summary>
        /// <param name="coachId">Identificator for Coach Entity.</param>
        /// <returns>/Coach/Details/Id Page.</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid coachId)
        {
            bool withTrainings = true;

            var coach = await this.coachService.GetByIdAsync(coachId, withTrainings);

            if (coach == null)
            {
                return RedirectToAction("All", "Coach");
            }

            return View(coach);
        }

        /// <summary>
        /// Deletes an Coach Entity.
        /// </summary>
        /// <param name="coachId">Identificator for Coach Entity.</param>
        /// <returns>/Coach/All Page.</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(Guid coachId)
        {
            await this.coachService.DeleteAsync(coachId);

            return RedirectToAction("All", "Coach");
        }
    }
}

