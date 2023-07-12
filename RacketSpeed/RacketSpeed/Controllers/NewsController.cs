using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Post;

namespace RacketSpeed.Controllers
{
    /// <summary>
    /// Provides functionality to the /News/ route.
    /// Keeps cache for 30 minutes.
    /// </summary>
    [Authorize(Roles = "Administrator")]
    [ResponseCache(Duration = 1800)] 
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
        /// Displays a /News/All page with three posts.
        /// </summary>
        /// <returns>/News/All page.</returns>
        /// <param name="pageCount">Int for page index.</param>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> All(bool isAdministrator, string pageCount = "1")
        {
            ViewData["IsAdministrator"] = isAdministrator && User.IsInRole("Administrator") ? true : false;

            int pageNum = int.Parse(pageCount);

            int postsPerPage = 3;

            var pagesCount = this.postService.PostsPageCount(postsPerPage);

            pageNum = CalculateValidPageNum(pageNum, pagesCount);

            ViewData["pageNum"] = pageNum;

            var posts = await postService.AllAsync(pageNum, postsPerPage);

            return View(new PostsPaginationCountViewModel()
            {
                Posts = posts,
                PageCount = pagesCount
            });
        }

        /// <summary>
        /// Displays a /News/All page with three posts by keyword.
        /// </summary>
        /// <returns>/News/All page.</returns>
        /// <param name="keyword">String used to filter Post Entities.</param>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> AllPostsByKeyword(string keyword, bool isAdministrator, string pageCount = "1")
        {
            ViewData["IsAdministrator"] = isAdministrator && User.IsInRole("Administrator") ? true : false;

            if (string.IsNullOrEmpty(keyword))
            {
                ModelState.AddModelError("KeywordError", "Полето трябва да съдържа поне 1 символ.");
                return View();
            }

            int pageNum = int.Parse(pageCount);

            int postsPerPage = 5;

            var pagesCount = this.postService.PostsPageCount(postsPerPage, keyword);

            pageNum = CalculateValidPageNum(pageNum, pagesCount);

            var posts = await postService.AllAsync(pageNum, postsPerPage, keyword);

            ViewData["keyword"] = keyword;
            ViewData["pageNum"] = pageNum;

            return View(new PostsPaginationCountViewModel()
            {
                Posts = posts,
                PageCount = pagesCount
            });
        }

        /// <summary>
        /// Displays an /Post/Add/ page for Admin users.
        /// </summary>
        /// <returns>/Post/Add/ page.</returns>
        [HttpGet]
        public IActionResult Add()
        {
            PostFormModel model = new PostFormModel();

            return View(model);
        }

        /// <summary>
        /// Gets the data from a HttpPost request and proccesses it.
        /// </summary>
        /// <param name="model">PostFormModel.</param>
        /// <returns>/News/All Page.</returns>
        [HttpPost]
        public async Task<IActionResult> Add(PostFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.ImageUrls.Contains(null))
            {
                ModelState.AddModelError("UrlEmpty", "Всички адреси на снимки трябва да бъдат попълнени.");

                return View(model);
            }

            await this.postService.AddAsync(model);

            return RedirectToAction("All", "News");
        }

        /// <summary>
        /// Displays an /News/Edit/Id Page.
        /// </summary>
        /// <param name="postId">Identificator for Post Entity.</param>
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
        [HttpPost]
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

