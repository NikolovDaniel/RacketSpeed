using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Training;

namespace RacketSpeed.Controllers
{
    /// <summary>
    /// Provides functionality to the /Trainingx/ route.
    /// </summary>
    [Authorize(Roles = "Administrator")]
    public class TrainingController : Controller
    {
        /// <summary>
        /// Coach service.
        /// </summary>
        private readonly ICoachService coachService;
        private readonly ITrainingService trainingService;

        /// <summary>
        /// DI Coach service.
        /// DI Training service.
        /// </summary>
        /// <param name="coachService">ICoachService.</param>
        /// <param name="trainingService">ITrainingService.</param>
        public TrainingController(ICoachService coachService, ITrainingService trainingService)
        {
            this.coachService = coachService;
            this.trainingService = trainingService;
        }

        /// <summary>
        /// Display 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All(string trainingName)
        {
            var allTrainings = await this.trainingService.AllAsync(trainingName);

            ViewData["Title"] = $"{trainingName} години";

            return View(allTrainings);
        }

        /// <summary>
        /// Displays a Training/Add page.
        /// </summary>
        /// <returns>/Training/Add page.</returns>
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var coaches = await this.coachService.AllAsync();

            var coachTrainingModels = coaches
                .Select(c => new TrainingCoachFormModel()
                {
                    Id = c.Id,
                    Name = $"{c.FirstName} {c.LastName}"
                })
                .ToList();
            

            return View(new TrainingFormModel()
            {
                Coaches = coachTrainingModels
            });
        }

        /// <summary>
        /// Gets the data from a HttpPost request and proccesses it.
        /// </summary>
        /// <param name="model">TrainingFormModel.</param>
        /// <returns>/Coach/All Page.</returns>
        [HttpPost]
        public async Task<IActionResult> Add(TrainingFormModel model)
        {
            if (!ModelState.IsValid)
            {
                var coaches = await this.coachService.AllAsync();

                var coachTrainingModels = coaches
                    .Select(c => new TrainingCoachFormModel()
                    {
                        Id = c.Id,
                        Name = $"{c.FirstName} {c.LastName}"
                    })
                    .ToList();

                model.Coaches = coachTrainingModels;

                return View(model);
            }

            await this.trainingService.AddAsync(model);

            return RedirectToAction("All", "Coach");
        }

        /// <summary>
        /// Displays an /Training/Edit/Id Page.
        /// </summary>
        /// <param name="trainingId">Identificator for Training Entity.</param>
        /// <returns>/Training/Edit/Id Page.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid trainingId)
        {
            var training = await this.trainingService.GetByIdAsync(trainingId);
            var coaches = await this.coachService.AllAsync();

            if (training == null)
            {
                return RedirectToAction("All", "Coach");
            }

            training.Coaches = coaches
                .Select(c => new TrainingCoachFormModel()
                {
                    Id = c.Id,
                    Name = $"{c.FirstName} {c.LastName}"
                });

            return View(training);
        }

        /// <summary>
        /// Gets the data from a HttpPost request and proccesses it.
        /// </summary>
        /// <param name="model">TrainingFormModel.</param>
        /// <returns>/Coach/All Page.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(TrainingFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await this.trainingService.EditAsync(model);

            return RedirectToAction("All", "Coach");
        }

        /// <summary>
        /// Deletes an Training Entity.
        /// </summary>
        /// <param name="trainingId">Identificator for Training Entity.</param>
        /// <returns>/Coach/All Page.</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(Guid trainingId)
        {
            await this.trainingService.DeleteAsync(trainingId);

            return RedirectToAction("All", "Coach");
        }
    }
}

