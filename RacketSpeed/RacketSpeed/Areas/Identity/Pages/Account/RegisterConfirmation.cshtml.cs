// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
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
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _sender;
        private readonly SendGridEmailSender _sendGridEmailSender;

        public RegisterConfirmationModel(UserManager<ApplicationUser> userManager,
            IEmailSender sender,
            SendGridEmailSender sendGridEmailSender)
        {
            _userManager = userManager;
            _sender = sender;
            _sendGridEmailSender = sendGridEmailSender;
        }

        public string Email { get; set; }

        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string accountName, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }
            returnUrl = returnUrl ?? Url.Content("~/");

            var user = await _userManager.FindByNameAsync(accountName.Normalize());
            if (user == null)
            {
                return NotFound($"Неуспешно заредихме потребителя с този емайл: '{email}'.");
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callBackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = user.Id, code = token },
            protocol: Request.Scheme);

            Email = email;

            await _sendGridEmailSender.SendEmailAsync(
                Email,
                 "Потвърди емайла си",
                 $"Потвърди акаунта си като <a href='{HtmlEncoder.Default.Encode(callBackUrl)}'>кликнеш тук!</a>."
             );

            return Page();
        }
    }
}
