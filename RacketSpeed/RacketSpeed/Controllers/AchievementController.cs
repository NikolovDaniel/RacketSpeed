using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RacketSpeed.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Post;
using RacketSpeed.Core.Models.Achievement;

namespace RacketSpeed.Controllers
{
    /// <summary>
    /// Provides functionality to the /Achievement/ route.
    /// </summary>
    [Authorize(Roles = "Administrator")]
    public class AchievementController : Controller
    {
        /// <summary>
        /// Achievement service.
        /// </summary>
        private readonly IAchievementService achievementService;

        /// <summary>
        /// DI Achievement service.
        /// </summary>
        /// <param name="achievementService">IPostService.</param>
        public AchievementController(IAchievementService achievementService)
        {
            this.achievementService = achievementService;
        }

        /// <summary>
        /// Displays an /Achievement/All page with all achievements.
        /// </summary>
        /// <returns>/Achievement/All page.</returns>
        [AllowAnonymous]
        public async Task<IActionResult> All()
        {
            var models = await this.achievementService.AllAsync();


            models = new List<AchievementViewModel>()
            {
                new AchievementViewModel()
                {
                    Date = DateTime.Today,
                    Location = "Haskovo",
                    Content = "First place at the tournament in Haskovo."
                }
            };

            return View(models);
        }

        /// <summary>
        /// Displays an /Add/ page for Admin users.
        /// </summary>
        /// <returns>/Achievement/Add/ page.</returns>
        [HttpGet]
        public IActionResult Add()
        {
            AchievementFormModel model = new AchievementFormModel();

            return View(model);
        }

        /// <summary>
        /// Gets the data from a HttpPost request and proccesses it.
        /// </summary>
        /// <param name="model">AchievementFormModel.</param>
        /// <returns>/Achievement/All Page.</returns>
        [HttpPost]
        public async Task<IActionResult> Add(AchievementFormModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    ModelState.AddModelError($"{error.Key}", $"{error.Value}");
                }

                return View(model);
            }

            await this.achievementService.AddAsync(model);
             
            return RedirectToAction("All", "Achievement");
        }

        /// <summary>
        /// Displays an /Achievement/Edit/Id Page.
        /// </summary>
        /// <param name="id">Identificator for Post Entity.</param>
        /// <returns>/Achievement/Edit/Id Page.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var achievement = await this.achievementService.GetByIdAsync(id);

            if (achievement == null)
            {
                return RedirectToAction("All", "Achievement");
            }

            AchievementFormModel model = new AchievementFormModel()
            {
               Id = achievement.Id,
               Date = achievement.Date,
               Location = achievement.Location,
               Content = achievement.Content
            };

            return View(model);
        }

        /// <summary>
        /// Gets the data from a HttpPost request and proccesses it.
        /// </summary>
        /// <param name="model">AchievementFormModel.</param>
        /// <returns>/Achievement/All Page.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(AchievementFormModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    ModelState.AddModelError($"{error.Key}", $"{error.Value}");
                }

                return View(model);
            }

            await this.achievementService.EditAsync(model);

            return RedirectToAction("All", "Achievement");
        }

        /// <summary>
        /// Deletes an Achievement Entity.
        /// </summary>
        /// <param name="id">Identificator for Achievement Entity.</param>
        /// <returns>/Achievement/All Page.</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await this.achievementService.DeleteAsync(id);

            return RedirectToAction("All", "Achievement");
        }
    }
}

