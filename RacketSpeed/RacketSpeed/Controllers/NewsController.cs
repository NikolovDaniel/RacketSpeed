using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Post;
using RacketSpeed.TestStrings;

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

            return View(posts);
        }

        /// <summary>
        /// Displays an /Add/ page for Admin users.
        /// </summary>
        /// <returns>/Add/ page.</returns>
        [HttpGet]
        public IActionResult Add()
        {
            PostFormModel model = new PostFormModel();

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

            if (model.ImageUrls.Contains(null))
            {
                ModelState.AddModelError("UrlEmpty", "All URLs should be filled.");

                return View(model);
            }

            await this.postService.AddAsync(model);

            return RedirectToAction("All", "News");
        }

        /// <summary>
        /// Displays an /News/Edit/Id Page.
        /// </summary>
        /// <param name="id">Identificator for Post Entity.</param>
        /// <returns>/News/Edit/Id Page.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid postId)
        {
            var post = await this.postService.GetByIdAsync(postId);

            if (post == null)
            {
                return RedirectToAction("All", "News");
            }

            PostFormModel model = new PostFormModel()
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                ImageUrls = post.ImageUrls
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

            await this.postService.EditAsync(model);

            return RedirectToAction("All", "News");
        }

        /// <summary>
        /// Deletes a Post Entity.
        /// </summary>
        /// <param name="postId">Identificator for Post Entity.</param>
        /// <returns>/News/All Page.</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(Guid postId)
        {
            await this.postService.DeleteAsync(postId);

            return RedirectToAction("All", "News");
        }

        /// <summary>
        /// Display a /News/Details/Id Page.
        /// </summary>
        /// <param name="postId">Identificator for Post Entity.</param>
        /// <returns>/News/Details/Id Page.</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid postId)
        {
            var post = await this.postService.GetByIdAsync(postId);

            if (post == null)
            {
                return RedirectToAction("All", "News");
            }

            return View(post);
        }
    }
}

