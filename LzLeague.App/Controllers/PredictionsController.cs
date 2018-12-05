namespace LzLeague.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using LzLeague.Common.PredictionsBindingModels;
    using LzLeague.Models;
    using LzLeague.Services.Admin.Contracts;
    using LzLeague.Services.Base.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class PredictionsController : Controller
    {
        private readonly ITeamsService ts;
        private readonly IMatchService ms;
        private readonly IPredictionsService ps;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public PredictionsController(ITeamsService ts, IMatchService ms, IPredictionsService ps, IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            this.ts = ts;
            this.ms = ms;
            this.ps = ps;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UserPrediction()
        {
            var userId = this.userManager.GetUserId(this.User);
            var existingPrediction = await this.ps.GetUserPrediction(userId);
            PredictionBindingModel model;

            if (existingPrediction == null)
            {
                model = await this.GetPrePopulatedModel();
                
                return this.View(model);
            }

            this.TempData["ExistingPrediction"] = "true";

            model = this.mapper.Map<PredictionBindingModel>(existingPrediction);

            model.MatchesResults = this.mapper
                .Map<ICollection<MatchResultPrediction>, ICollection<MatchResultBindingModel>>(existingPrediction
                    .MatchResultsPredictions)
                    .OrderBy(x => x.Match.Date)
                    .ThenBy(x => x.Match.Group.Name)
                    .ToList();

            await this.PopulateTeamsLogos(model.MatchesResults.ToList());

            model.GroupWinners = this.mapper
                .Map<ICollection<GroupWinnerPrediction>, ICollection<GroupWinnerBindingModel>>(existingPrediction
                    .GroupsWinners)
                    .OrderBy(x => x.Group.Name)
                    .ToList();

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UserPrediction(PredictionBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                await this.PopulateTeamsLogos(model.MatchesResults.ToList());
                return this.View(model);
            }

            var userId = this.userManager.GetUserId(this.User);

            await this.ps.CreateUserPrediction(userId, model);
            this.TempData["SuccessMsg"] = "Your prediction was submitted successfully.";

            return this.RedirectToAction("UserPrediction");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> ViewPrediction(string userId)
        {
            var prediction = await this.ps.GetUserPrediction(userId);
            var user = await this.userManager.FindByIdAsync(userId);

            if (prediction == null)
            {
                this.TempData["WarningMsg"] = "Prediction you are trying to reach doesn't exist.";
                return this.RedirectToAction("UsersStandings");
            }

            var predictionVm = this.mapper.Map<PredictionBindingModel>(prediction);

            predictionVm.MatchesResults = this.mapper
                .Map<ICollection<MatchResultPrediction>, ICollection<MatchResultBindingModel>>(prediction
                    .MatchResultsPredictions)
                    .OrderBy(x => x.Match.Date)
                    .ThenBy(x => x.Match.Group.Name)
                    .ToList();

            await this.PopulateTeamsLogos(predictionVm.MatchesResults.ToList());

            predictionVm.GroupWinners = this.mapper
                .Map<ICollection<GroupWinnerPrediction>, ICollection<GroupWinnerBindingModel>>(prediction
                    .GroupsWinners);

            this.TempData["FullName"] = user.FullName;

            return this.View(predictionVm);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public IActionResult UsersStandings()
        {
            var predictions = this.ps.GetAllPredictionsForStandings();
            var predictionsVm = this.mapper
                .Map<ICollection<Prediction>, ICollection<UserStandingBindingModel>>(predictions)
                .OrderByDescending(p => p.TotalScore)
                .ThenByDescending(p => p.GuessedScores)
                .ThenByDescending(p => p.GuessedResults)
                .ToList();

            return this.View(predictionsVm);
        }

        private async Task<PredictionBindingModel> GetPrePopulatedModel()
        {
            var matchesVm = this.mapper
                .Map<IEnumerable<Match>, IEnumerable<MatchResultBindingModel>>(this.ms.GetAllMatches())
                .OrderBy(m => m.Match.Group.Name)
                .ThenBy(m => m.Match.Date)
                .ToList();

            await this.PopulateTeamsLogos(matchesVm);
            
            var groupsVm = this.mapper
                .Map<IEnumerable<Group>, IEnumerable<GroupWinnerBindingModel>>(this.ts.GetAllGroups())
                .OrderBy(x => x.Group.Name)
                .ToList();

            var model = new PredictionBindingModel
            {
                MatchesResults = matchesVm,
                GroupWinners = groupsVm
            };

            return model;
        }

        private async Task PopulateTeamsLogos(List<MatchResultBindingModel> matchesVm)
        {
            foreach (var match in matchesVm)
            {
                match.HomeTeamLogo = await this.ts.GetTeamLogo(match.Match.HomeTeam);
                match.AwayTeamLogo = await this.ts.GetTeamLogo(match.Match.AwayTeam);
            }
        }
    }
}