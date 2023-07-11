using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Event;

namespace RacketSpeed.Controllers
{
    /// <summary>
    /// Provides functionality to the /Event/ route.
    /// Keeps cache for 30 minutes.
    /// </summary>
    [ResponseCache(Duration = 1800)]
    [Authorize(Roles = "Administrator")]
    public class EventController : Controller
    {
        /// <summary>
        /// Event service.
        /// </summary>
        private readonly IEventService eventService;

        /// <summary>
        /// DI Event service.
        /// </summary>
        /// <param name="eventService">IEventService.</param>
        public EventController(IEventService eventService)
        {
            this.eventService = eventService;
        }


        /// <summary>
        /// Displays a /Event/All/Category page with three posts.
        /// </summary>
        /// <returns>/Event/All/Category page.</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All(string category)
        {
            bool isInRoleAdministrator = User.IsInRole("Administrator");
            ViewData["IsAdministrator"] = isInRoleAdministrator;

            var posts = await eventService.AllAsync(category, isInRoleAdministrator);

            ViewData["Title"] = category;

            return View(posts);
        }

        /// <summary>
        /// Displays an /Event/Add/ page for Admin users.
        /// </summary>
        /// <returns>/Event/Add/ page.</returns>
        [HttpGet]
        public IActionResult Add()
        {
            var eventModel = new EventFormModel();

            return View(eventModel);
        }

        /// <summary>
        /// Gets the data from a HttpPost request and proccesses it.
        /// </summary>
        /// <param name="model">EventFormModel.</param>
        /// <returns>/Event/All Page.</returns>
        [HttpPost]
        public async Task<IActionResult> Add(EventFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if(model.Start > model.End)
            {
                ModelState.AddModelError("InvalidDates", "Датата за начало на събитието трябва да бъде по-рано от датата на края.");

                return View(model);
            }

            if (model.ImageUrls.Contains(null))
            {
                ModelState.AddModelError("UrlEmpty", "Всички адреси на снимки трябва да бъдат попълнени.");

                return View(model);
            }

            await this.eventService.AddAsync(model);

            return RedirectToAction("All", "Event", new { category = model.Category });
        }

        /// <summary>
        /// Displays an /Event/Edit/Id Page.
        /// </summary>
        /// <param name="eventId">Identificator for Event Entity.</param>
        /// <returns>/Event/Edit/Id Page.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid eventId)
        {
            var eventEntity = await this.eventService.GetByIdAsync(eventId);

            if (eventEntity == null)
            {
                return RedirectToAction("All", "Event", new { category = "Лагери" });
            }

            EventFormModel model = new EventFormModel()
            {
                Id = eventEntity.Id,
                Title = eventEntity.Title,
                Content = eventEntity.Content,
                Start = eventEntity.Start,
                End = eventEntity.End,
                Category = eventEntity.Category,
                Location = eventEntity.Location,
                ImageUrls = eventEntity.ImageUrls
            };

            return View(model);
        }

        /// <summary>
        /// Gets the data from a HttpPost request and proccesses it.
        /// </summary>
        /// <param name="model">EventFormModel.</param>
        /// <returns>/Event/All/Category Page.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(EventFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if(model.Start > model.End)
            {
                ModelState.AddModelError("InvalidDates", "Датата за начало на събитието трябва да бъде по-рано от датата на края.");

                return View(model);
            }

            if (model.ImageUrls.Contains(null))
            {
                ModelState.AddModelError("UrlEmpty", "Всички адреси на снимки трябва да бъдат попълнени.");

                return View(model);
            }

            await this.eventService.EditAsync(model);

            return RedirectToAction("All", "Event", new
            {
                category = model.Category
            });
        }

        /// <summary>
        /// Display a /Event/Details/Id Page.
        /// </summary>
        /// <param name="eventId">Identificator for Event Entity.</param>
        /// <returns>/Event/Details/Id Page.</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid eventId)
        {
            var eventModel = await this.eventService.GetByIdAsync(eventId);

            if (eventModel == null)
            {
                return RedirectToAction("All", "Event");
            }

            return View(eventModel);
        }

        /// <summary>
        /// Deletes an Event Entity.
        /// </summary>
        /// <param name="eventId">Identificator for Event Entity.</param>
        /// <returns>/Event/All/Category Page.</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(Guid eventId)
        {
            var model = await this.eventService.GetByIdAsync(eventId);

            if (model == null)
            {
                return RedirectToAction("All", "Event");
            }
            await this.eventService.DeleteAsync(eventId);

            return RedirectToAction("All", "Event", new { category = model.Category });
        }
    }
}

