namespace LzLeague.App.Areas.Admin.Controllers
{
    using System.Threading.Tasks;
    using Common.AdminBindingModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Admin.Contracts;

    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly IAdminAccountService aas;

        public AccountController(IAdminAccountService aas)
        {
            this.aas = aas;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            var model = new LoginBindingModel();

            return this.View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var signInSucceeded = await this.aas.SignIn(model);

            if (!signInSucceeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return this.View(model);
            }

            return RedirectToAction("Index", "Home", new {area = ""});
        }
    }
}