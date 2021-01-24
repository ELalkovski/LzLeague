namespace LzLeague.App.Areas.Identity.Pages.Account
{
    using System.Linq;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using LzLeague.Common.AuthBindingModels;
    using Microsoft.AspNetCore.Authorization;
    using LzLeague.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._logger = logger;
            this._emailSender = emailSender;
            this.Input = new RegisterInputModel();
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");

            if (this.ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = this.Input.Email,
                    Email = this.Input.Email,
                    FullName = this.Input.FullName
                };

                var result = await this._userManager.CreateAsync(user, this.Input.Password);

                if (result.Succeeded)
                {
                    this._logger.LogInformation("User created a new account with password.");

                    //var code = await this._userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = this.Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { userId = user.Id, code = code },
                    //    protocol: this.Request.Scheme);

                    //await this._emailSender.SendEmailAsync(this.Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (this._userManager.Users.Count() > 1)
                    {
                        await this._userManager.AddToRoleAsync(user, "User");
                    }
                    else
                    {
                        user.IsApproved = true;
                        await this._userManager.UpdateAsync(user);
                        await this._userManager.AddToRoleAsync(user, "Admin");
                    }

                    //await this._signInManager.SignInAsync(user, isPersistent: false);
                    return this.RedirectToPage("/Account/Login");
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }
    }
}
