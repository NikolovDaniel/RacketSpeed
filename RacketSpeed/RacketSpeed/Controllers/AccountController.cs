using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RacketSpeed.Email;
using RacketSpeed.Infrastructure.Data.Entities;

namespace RacketSpeed.Controllers
{
    /// <summary>
    /// Provides functionality to the /Account/ route.
    /// </summary>
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SendGridEmailSender _sendGridEmailSender;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SendGridEmailSender sendGridEmailSender)
        {
            _userManager = userManager;
            _sendGridEmailSender = sendGridEmailSender;
        }

        /// <summary>
        /// Displays /Account/ResendConfirmationEmail page.
        /// </summary>
        /// <returns>/Account/ResendConfirmationEmail page.</returns>
        [HttpGet]
        public IActionResult ResendConfirmationEmail()
        {
            return View();
        }

        /// <summary>
        /// Gets the data from a HttpPost request and proccesses it.
        /// </summary>
        /// <param name="email">User's email.</param>
        /// <returns>/Account/ResendConfirmationEmail Page.</returns>
        [HttpPost]
        public async Task<IActionResult> ResendConfirmationEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("InvalidEmail", "Полето е задължително.");
                return View();
            }
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return RedirectToPage("/Account/Register", new { area = "Identity" });
            }

            if (user.EmailConfirmed)
            {
                return RedirectToAction("Index", "Home");
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callBackUrl = Url.Page(
            "/Account/ConfirmEmail",
             pageHandler: null,
             values: new { area = "Identity", userId = user.Id, code = token },
             protocol: Request.Scheme);

            var resendEmailConfirmation = string.Format(EmailBuilder.EmailConfirmation, user.UserName, HtmlEncoder.Default.Encode(callBackUrl));

            await _sendGridEmailSender.SendEmailAsync(
                 user.Email,
                  $"{user.FirstName}, потвърждение за емайл адрес",
                 resendEmailConfirmation);

            ViewData["IsResended"] = true;

            return View();
        }
    }
}

