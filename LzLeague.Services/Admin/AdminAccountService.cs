namespace LzLeague.Services.Admin
{
    using System.Threading.Tasks;
    using Common.AdminBindingModels;
    using Contracts;
    using Microsoft.AspNetCore.Identity;
    using Models;

    public class AdminAccountService : IAdminAccountService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AdminAccountService(SignInManager<ApplicationUser> signInManager)
        {
            this._signInManager = signInManager;
        }

        public async Task<bool> SignIn(LoginBindingModel model)
        {
            var result = await this._signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

            return result.Succeeded;
        }
    }
}
