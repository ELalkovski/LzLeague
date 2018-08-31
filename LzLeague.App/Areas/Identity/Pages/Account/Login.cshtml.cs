namespace LzLeague.App.Areas.Identity.Pages.Account
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LzLeague.Common.AuthBindingModels;
    using Microsoft.AspNetCore.Authorization;
    using LzLeague.Models;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger)
        {
            this._signInManager = signInManager;
            this._logger = logger;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            if (!User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(this.ErrorMessage))
                {
                    this.ModelState.AddModelError(string.Empty, this.ErrorMessage);
                }

                returnUrl = returnUrl ?? Url.Content("~/");

                // Clear the existing external cookie to ensure a clean login process
                await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                this.ExternalLogins = (await this._signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                this.ReturnUrl = returnUrl;

                return this.Page();
            }

            return this.LocalRedirect("~/");
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");

            if (this.ModelState.IsValid)
            {
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await this._signInManager.PasswordSignInAsync(this.Input.Email, this.Input.Password, this.Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    this._logger.LogInformation("User logged in.");

                    return this.LocalRedirect(returnUrl);
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                    return this.Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }
    }
}
