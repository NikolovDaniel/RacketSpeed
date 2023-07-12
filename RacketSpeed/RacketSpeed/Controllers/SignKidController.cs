using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.SignKidForm;

namespace RacketSpeed.Controllers
{
    /// <summary>
    /// Provides functionality to the /SignKid/ route.
    /// </summary>
    [Authorize(Roles = "Administrator")]
    public class SignKidController : Controller
    {
        /// <summary>
        /// Post service.
        /// </summary>
        private readonly ISignKidService signKidService;

        /// <summary>
        /// DI SignKid service.
        /// </summary>
        /// <param name="signKidService">ISignKidService.</param>
        public SignKidController(ISignKidService signKidService)
        {
            this.signKidService = signKidService;
        }

        /// <summary>
        /// Displays an /SignKid/ Page.
        /// </summary>
        /// <returns>/SignKid/ Form page.</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignForm()
        {
            var model = new SignKidFormModel();

            return View(model);
        }

        /// <summary>
        /// Gets the data from a HttpPost request and proccesses it.
        /// </summary>
        /// <param name="model">SignKidFormModel.</param>
        /// <returns>/SuccesCreated/ Page.</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignForm(SignKidFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.PrivacyPolicyIsAccepted == false)
            {
                ModelState.AddModelError("PrivacyPolicy", "Политиката за Поверителност трябва да бъде приета.");
                return View(model);
            }

            await this.signKidService.AddAsync(model);
            ViewData["SuccessfullyAdded"] = $"Успешно изпратихте запитващата форма за тренировки.";
            return View(); 
        }

        /// <summary>
        /// Displays a /News/All page with three posts.
        /// </summary>
        /// <returns>/News/All page.</returns>
        /// <param name="pageCount">Int for page index.</param>
        [HttpGet]
        public async Task<IActionResult> All(string pageCount = "1")
        {
            int pageNum = int.Parse(pageCount);

            int formsPerPage = 6;

            var pagesCount = this.signKidService.SignFormsPageCount(formsPerPage);

            pageNum = CalculateValidPageNum(pageNum, pagesCount);

            ViewData["pageNum"] = pageNum;

            var posts = await signKidService.AllAsync(pageNum, formsPerPage);

            return View(new SignKidFormsPaginationViewModel()
            {
                PageCount = pagesCount,
                SignedKidForms = posts
            });
        }

        /// <summary>
        /// Display a /SignKid/Details/Id Page.
        /// </summary>
        /// <param name="signKidFormId">Identificator for SignKid Entity.</param>
        /// <returns>/SignKid/Details/Id Page.</returns>
        [HttpGet]
        public async Task<IActionResult> Details(Guid signKidFormId)
        {
            var signKidForm = await this.signKidService.GetByIdAsync(signKidFormId);

            if (signKidForm == null)
            {
                return RedirectToAction("All", "SignKid");
            }

            return View(signKidForm);
        }

        /// <summary>
        /// Deletes a SignKid Entity.
        /// </summary>
        /// <param name="signKidFormId">Identificator for SignKid Entity.</param>
        /// <returns>/SignKid/All Page.</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(Guid signKidFormId)
        {
            await this.signKidService.DeleteAsync(signKidFormId);

            return RedirectToAction("All", "SignKid");
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

