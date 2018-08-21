namespace LzLeague.Services.Admin
{
    using System.Security.Policy;
    using System.Threading.Tasks;
    using Common.AdminBindingModels;
    using Contracts;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models;

    public class AdminAccountService : IAdminAccountService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AdminAccountService(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<bool> SignIn(LoginBindingModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

            return result.Succeeded;
        }
    }
}
