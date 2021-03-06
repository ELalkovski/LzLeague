﻿namespace LzLeague.App.Areas.Identity.Pages.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using LzLeague.Models;
    using LzLeague.Services.Admin.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly IUsersService us;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ExternalLoginModel> _logger;

        public ExternalLoginModel(
            IUsersService us,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<ExternalLoginModel> logger)
        {
            this.us = us;
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string LoginProvider { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            return this.RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = this.Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = this._signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");
            if (remoteError != null)
            {
                this.ErrorMessage = $"Error from external provider: {remoteError}";
                return this.RedirectToPage("./Login", new {ReturnUrl = returnUrl });
            }
            var info = await this._signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                this.ErrorMessage = "Error loading external login information.";
                return this.RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await this._signInManager
                .ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor : true);

            if (result.Succeeded)
            {
                var user = await this.us.GetUserByEmail(info.Principal.FindFirstValue(ClaimTypes.Email));

                if (user == null)
                {
                    this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                    return this.RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                }

                if (!user.IsApproved)
                {
                    this.TempData["WarningMsg"] = "You are already registered with your facebook profile, but you are not approved by an admin.";
                    await this._signInManager.SignOutAsync();

                    return this.RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                }

                this._logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return this.LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return this.RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                this.ReturnUrl = returnUrl;
                this.LoginProvider = info.LoginProvider;

                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    this.Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
                return this.Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await this._signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                this.ErrorMessage = "Error loading external login information during confirmation.";
                return this.RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (this.ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = this.Input.Email, Email = this.Input.Email };
                var result = await this._userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    result = await this._userManager.AddLoginAsync(user, info);

                    await this._userManager.AddToRoleAsync(user, "User");
                    //await this._signInManager.SignInAsync(user, isPersistent: false);
                    this._logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                    return this.RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                }
                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            //if (this.ModelState.IsValid)
            //{
            //    var user = new ApplicationUser { UserName = this.Input.Email, Email = this.Input.Email };
            //    var result = await this._userManager.CreateAsync(user);

            //    if (result.Succeeded)
            //    {
            //        result = await this._userManager.AddLoginAsync(user, info);

            //        if (result.Succeeded)
            //        {
            //            await this._userManager.AddToRoleAsync(user, "User");
            //            await this._signInManager.SignInAsync(user, isPersistent: false);
            //            this._logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

            //            return this.LocalRedirect(returnUrl);
            //        }
            //    }
            //    foreach (var error in result.Errors)
            //    {
            //        this.ModelState.AddModelError(string.Empty, error.Description);
            //    }
            //}

            this.LoginProvider = info.LoginProvider;
            this.ReturnUrl = returnUrl;

            return this.Page();
        }
    }
}
