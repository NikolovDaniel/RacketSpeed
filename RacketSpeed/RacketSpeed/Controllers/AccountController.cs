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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly SendGridConfiguration _sendGridConfiguration;
        private readonly SendGridEmailSender _sendGridEmailSender;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<SendGridConfiguration> optionsAccessor,            SendGridEmailSender sendGridEmailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _sendGridConfiguration = optionsAccessor.Value;
            _sendGridEmailSender = sendGridEmailSender;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult ResendConfirmationEmail()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
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

            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                // Handle the case when user is not found or email is not confirmed
                return RedirectToAction("ForgotPassword"); // Redirect back to the form
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink = Url.Page(
               "/Account/ResetPassword",
               pageHandler: null,
               values: new { userId = user.Id, code = token },
               protocol: Request.Scheme);

            // Use your SendGridEmailSender to send the password reset email
            await _sendGridEmailSender.SendEmailAsync(email, "Промени паролата си.",
                $"Променете паролата си като кликнете върху този линк: <a href='{resetLink}'>линк</a>");

            return RedirectToAction("ResetPassword", "Account", new { area = "Identity" });
        }
    }
}

