namespace LzLeague.App.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using LzLeague.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._emailSender = emailSender;
        }

        public string Username { get; set; }

        public string FullName { get; set; }

        public int TotalScore { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string FullName { get; set; }

            [Required]
            public int TotalScore { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this._userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
            }

            var userName = await this._userManager.GetUserNameAsync(user);
            var email = await this._userManager.GetEmailAsync(user);

            this.Username = userName;

            this.Input = new InputModel
            {
                Email = email,
                FullName = user.FullName,
                TotalScore = user.TotalScore
            };

            this.IsEmailConfirmed = await this._userManager.IsEmailConfirmedAsync(user);

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var user = await this._userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
            }

            var email = await this._userManager.GetEmailAsync(user);
            user.FullName = this.Input.FullName;
            await this._userManager.UpdateAsync(user);

            if (this.Input.Email != email)
            {
                var setEmailResult = await this._userManager.SetEmailAsync(user, this.Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await this._userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }
            
            await this._signInManager.RefreshSignInAsync(user);
            this.TempData["Success"] = "Your profile has been updated successfully.";

            return this.RedirectToPage();
        }
    }
}
