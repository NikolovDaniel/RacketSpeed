#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using RacketSpeed.Email;
using RacketSpeed.Infrastructure.Data.Entities;

namespace RacketSpeed.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SendGridEmailSender _sendGridEmailSender;


        public ForgotPasswordModel(UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            SendGridEmailSender sendGridEmailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _sendGridEmailSender = sendGridEmailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                var emailSubject = string.Format(EmailBuilder.ForgotPassword, user.FirstName, HtmlEncoder.Default.Encode(callbackUrl));

                await _sendGridEmailSender.SendEmailAsync(
                    Input.Email,
                       $"{user.FirstName}, потвърждение за промяна на парола.",
                       emailSubject);

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
