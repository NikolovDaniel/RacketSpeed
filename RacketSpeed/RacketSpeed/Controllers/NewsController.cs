using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Post;

namespace RacketSpeed.Controllers
{
    /// <summary>
    /// Provides functionality to the /News/ route.
    /// </summary>
    [Authorize(Roles = "Administrator")]
    public class NewsController : Controller
    {
        /// <summary>
        /// Post service.
        /// </summary>
        private readonly IPostService postService;

        /// <summary>
        /// DI Post service.
        /// </summary>
        /// <param name="postService">IPostService.</param>
        public NewsController(IPostService postService)
        {
            this.postService = postService;
        }

        /// <summary>
        /// Displays a /News/All page with all posts.
        /// </summary>
        /// <returns>/News/All page.</returns>
        [AllowAnonymous]
        public async Task<IActionResult> All()
        {
            var posts = await postService.AllAsync();


            var models = new List<PostViewModel>()
            {
                new PostViewModel()
                {
                    Title
                    = "SECOND PLACE for the men's team of the Racquet Speed Badminton Club at the 2023 Men's and Women's \"A\" Group State Team Championship!",
                    Content
                    = @"🟥⬛ After extremely tight matches on the final day of the championship, our men's team managed to fight for second place and was one step away from the title of Bulgaria!

🟥⬛ The men's team of BC Rocket Speed in the composition of: Milan Dratva, Dimitar Yanakiev, Evgeni Panev, Anton Dalov and manager: Gergin Angelov achieved a historic success for the club after it managed to win the title of second place in Bulgaria with its first participation in the State Team championship ""A"" group!🥈🏆🇧🇬
🟥⬛ After playing a total of 5 team matches in the championship and extremely difficult matches, our team managed to achieve its great success thanks to the four victories it won in the group stages and in the final four with the following teams and results:

29/04/2023 Group 2

BC Rocket Speed - BC Kardzhali 4️⃣ : 1️⃣
BC Rocket Speed - BC Vias 3️⃣ : 2️⃣
30/04/2023 FINAL FOUR

BC Rocket Speed - Victory BC 2️⃣ : 3️⃣
BC Rocket Speed - BC Levski-Lyulin 3️⃣ : 2️⃣
BC Rocket Speed - BC Stara Zagora 4️⃣ : 1️⃣
🏆🥈 Thanks to all the players of the men's team of BC Rocket Speed, who fought with heart and soul and gave their best for the team! Thanks also to all the fans of the club who supported our men's team throughout the championship! There is no doubt that the second place will motivate us even more and next year we will chase the title with even greater strength and ambition! 💪🟥⬛🏆🇧🇬"
                },
            };

            return View(models);
        }

        /// <summary>
        /// Displays an /Add/ page for Admin users.
        /// </summary>
        /// <returns>/AddPost/ page.</returns>
        [HttpGet]
        public IActionResult Add()
        {
            PostViewModel model = new PostViewModel();

            return View(model);
        }

        /// <summary>
        /// Gets the data from a HttpPost request and proccesses it.
        /// </summary>
        /// <param name="model">PostFormModel..</param>
        /// <returns>/News/All Page.</returns>
        [HttpPost]
        public async Task<IActionResult> Add(PostFormModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    ModelState.AddModelError($"{error.Key}", $"{error.Value}");
                }

                return View(model);
            }

            await this.postService
                .AddAsync(new PostViewModel()
                {
                    Title = model.Title,
                    Content = model.Content,
                });

            return RedirectToAction("All", "News");
        }

        /// <summary>
        /// Displays an /Edit/Id Page.
        /// </summary>
        /// <param name="id">Identificator for Post Entity.</param>
        /// <returns>/Edit/Id Page.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var post = await this.postService.GetByIdAsync(id);

            if (post == null)
            {
                return View();
            }

            PostFormModel model = new PostFormModel()
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content
            };

            return View(model);
        }

        /// <summary>
        /// Gets the data from a HttpPost request and proccesses it.
        /// </summary>
        /// <param name="model">PostFormModel.</param>
        /// <returns>/News/All Page.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(PostFormModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    ModelState.AddModelError($"{error.Key}", $"{error.Value}");
                }

                return View(model);
            }

            await this.postService
                .EditAsync(new PostViewModel()
                {
                    Id = model.Id,
                    Title = model.Title,
                    Content = model.Content
                });

            return RedirectToAction("All", "News");
        }

        /// <summary>
        /// Deletes an Post Entity.
        /// </summary>
        /// <param name="id">Identificator for Post Entity.</param>
        /// <returns>/News/All Page.</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await this.postService.DeleteAsync(id);

            return RedirectToAction("All", "News");
        }
    }
}

