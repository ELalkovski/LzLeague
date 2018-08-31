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
    using Microsoft.AspNetCore.Mvc;

    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IUsersService us;
        private readonly IMapper mapper;
        private readonly IPredictionsService ps;

        public UsersController(IUsersService us, IMapper mapper, IPredictionsService ps)
        {
            this.us = us;
            this.mapper = mapper;
            this.ps = ps;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var users = this.us
                .GetUsers()
                .Where(u => u.Email != "emil27778@gmail.com")
                .ToList();

            var usersVm = this.mapper
                .Map<ICollection<ApplicationUser>, ICollection<UserBindingModel>>(users);

            return this.View(usersVm);
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