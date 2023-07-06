using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RacketSpeed.Email;
using RacketSpeed.Infrastructure.Data.Entities;

namespace RacketSpeed.Controllers
{
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

        [HttpGet]
        public IActionResult ResendConfirmationEmail()
        {
            return View();
        }

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
                return RedirectToAction("ResendConfirmationEmail");
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

            await _sendGridEmailSender.SendEmailAsync(
                 user.Email,
                 "Потвърди емайла си",
                 $"Потвърди акаунта си като <a href='{HtmlEncoder.Default.Encode(callBackUrl)}'>кликнеш тук!</a>."
             );

            ViewData["IsResended"] = true;

            return View();
        }
    }
}

