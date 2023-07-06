#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RacketSpeed.Email;
using RacketSpeed.Infrastructure.Data.Entities;
using RacketSpeed.Infrastructure.Utilities;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace RacketSpeed.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IOptions<SendGridConfiguration> optionsAccessor,            SendGridEmailSender sendGridEmailSender)        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
        }

   
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = DataConstants.RequiredFieldErrorMessage)]
            [StringLength(30,
                MinimumLength = 5,
                ErrorMessage = "Потребителското име трябва да съдържа между 5 и 30 символа.")]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required(ErrorMessage = DataConstants.RequiredFieldErrorMessage)]
            [RegularExpression("^\\+?[1-9][0-9]{7,14}$",
            ErrorMessage = DataConstants.SignKidPhoneNumberErrorMessage)]
            [Display(Name = "Телефонен номер")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = DataConstants.RequiredFieldErrorMessage)]
            [StringLength(50,
                MinimumLength = 3,
                ErrorMessage = "Първото име трябва да съдържа между 3 и 50 букви.")]
            [Display(Name = "Първо име")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = DataConstants.RequiredFieldErrorMessage)]
            [StringLength(50,
                MinimumLength = 3,
                ErrorMessage = "Фамилното име трябва да съдържа между 3 и 50 букви.")]
            [Display(Name = "Фамилно име")]
            public string LastName { get; set; }

            [Required(ErrorMessage = DataConstants.RequiredFieldErrorMessage)]
            [EmailAddress(ErrorMessage = "Емайла не е валиден.")]
            [Display(Name = "Емайл")]
            public string Email { get; set; }

            [Required(ErrorMessage = DataConstants.RequiredFieldErrorMessage)]
            [StringLength(100, ErrorMessage = "{0} трябва да бъде поне {2} и най-много {1} символа.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Парола")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Потвърди парола")]
            [Compare("Password", ErrorMessage = "Двете пароли трябва да съвпадат.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = Input.UserName,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    PhoneNumber = Input.PhoneNumber
                };

                await _userStore.SetUserNameAsync(user, Input.UserName, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);

                    var loggedInUser = await _userManager.FindByIdAsync(userId);

                    await _userManager.AddClaimAsync(loggedInUser,
                        new Claim("FirstName", $"{loggedInUser.FirstName}"));

                    if (_userManager.Options.SignIn.RequireConfirmedEmail)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, accountName = Input.UserName, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
