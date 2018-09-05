namespace LzLeague.App.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using LzLeague.Common.AdminBindingModels;
    using LzLeague.Common.PredictionsBindingModels;
    using LzLeague.Models;
    using LzLeague.Services.Admin.Contracts;
    using LzLeague.Services.Base.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IUsersService us;
        private readonly IMapper mapper;
        private readonly IPredictionsService ps;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(IUsersService us, IMapper mapper, IPredictionsService ps, UserManager<ApplicationUser> userManager)
        {
            this.us = us;
            this.mapper = mapper;
            this.ps = ps;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var users = this.us
                .GetUsers()
                .Where(u => !this.userManager.IsInRoleAsync(u, "Admin").Result)
                .ToList();

            var usersVm = this.mapper
                .Map<ICollection<ApplicationUser>, ICollection<UserBindingModel>>(users);

            return this.View(usersVm);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string email)
        {
            var user = await this.us
                .GetUserByEmail(email);

            if (user == null)
            {
                this.TempData["WarningMsg"] = "User you are trying to reach does not exist.";
                return this.RedirectToAction("Index");
            }

            await this.us.Delete(user);
            this.TempData["SuccessMsg"] = "User has been removed successfully.";

            return this.RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAdminRole(string email)
        {
            var user = await this.us
                .GetUserByEmail(email);

            if (user == null)
            {
                this.TempData["WarningMsg"] = "User you are trying to reach does not exist.";
                return this.RedirectToAction("Index");
            }

            user.IsApproved = true;
            await this.userManager.AddToRoleAsync(user, "Admin");
            await this.us.UpdateUser(user);

            this.TempData["SuccessMsg"] = "User has been promoted to Admin role successfully.";

            return this.RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(string email)
        {
            var user = await this.us
                .GetUserByEmail(email);

            if (user == null)
            {
                this.TempData["WarningMsg"] = "User you are trying to reach does not exist.";
                return this.RedirectToAction("Index");
            }

            user.IsApproved = true;
            await this.us.UpdateUser(user);

            this.TempData["SuccessMsg"] = "User has been approved successfully.";

            return this.RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Ban(string email)
        {
            var user = await this.us
                .GetUserByEmail(email);

            if (user == null)
            {
                this.TempData["WarningMsg"] = "User you are trying to reach does not exist.";
                return this.RedirectToAction("Index");
            }

            user.IsApproved = false;
            await this.us.UpdateUser(user);

            this.TempData["SuccessMsg"] = "User has been banned successfully.";

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditPrediction(int predictionId)
        {
            var prediction = await this.ps.GetPrediction(predictionId);

            if (prediction == null)
            {
                this.TempData["WarningMsg"] = "No prediction";
                return this.RedirectToAction("Index");
            }

            var predictionVm = this.mapper
                .Map<Prediction, PredictionBindingModel>(prediction);

            predictionVm.MatchesResults = this.mapper
                .Map<ICollection<MatchResultPrediction>, ICollection<MatchResultBindingModel>>(prediction
                    .MatchResultsPredictions);
            predictionVm.GroupWinners = this.mapper
                .Map<ICollection<GroupWinnerPrediction>, ICollection<GroupWinnerBindingModel>>(prediction
                    .GroupsWinners);

            return this.View(predictionVm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditPrediction(PredictionBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var matchesResults = this.mapper
                .Map<ICollection<MatchResultBindingModel>, ICollection<MatchResultPrediction>>(model.MatchesResults);
            var groupWinners = this.mapper
                .Map<ICollection<GroupWinnerBindingModel>, ICollection<GroupWinnerPrediction>>(model.GroupWinners);

            await this.us.UpdateUserPrediction(matchesResults, groupWinners);

            this.TempData["SuccessMsg"] = "Prediction was updated successfully.";

            return this.RedirectToAction("EditPrediction", new {predictionId = model.Id});
        }
    }
}