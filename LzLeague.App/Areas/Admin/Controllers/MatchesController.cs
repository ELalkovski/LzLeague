namespace LzLeague.App.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Common.AdminBindingModels;
    using LzLeague.Models;
    using LzLeague.Services.Base.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Admin.Contracts;

    [Area("Admin")]
    public class MatchesController : Controller
    {
        private readonly IMapper mapper;
        private readonly ITeamsService ts;
        private readonly IMatchService ms;
        private readonly IPredictionsService ps;

        public MatchesController(ITeamsService ts, IMatchService ms, IMapper mapper, IPredictionsService ps)
        {
            this.ts = ts;
            this.ms = ms;
            this.mapper = mapper;
            this.ps = ps;
        }

        [HttpGet("/Matches/Index")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            var matches = this.ms.GetAllMatches();
            var matchesVm = this.mapper
                .Map<IEnumerable<Match>, IEnumerable<MatchBindingModel>>(matches)
                .ToList();

            var matchesGroupedByDate = matchesVm
                .GroupBy(m => m.Date, m => m, (key, value) => new MatchGroupDisplayModel
            {
                Date = key,
                Matches = value.ToList()
            });

            return this.View(matchesGroupedByDate);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            var model = new MatchBindingModel
            {
                AvailableGroups = this.SetAvailableGroups()
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(MatchBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.AvailableGroups = this.SetAvailableGroups();
                return this.View(model);
            }

            var match = this.mapper.Map<Match>(model);
            await this.ms.CreateMatch(match);

            this.TempData["SuccessMsg"] = "Match game has been added successfully.";

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddResult()
        {
            var model = new AddResultBindingModel()
            {
                AvailableGroups = this.SetAvailableGroups()
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddResult(AddResultBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.AvailableGroups = this.SetAvailableGroups();

                return this.View(model);
            }

            var group = await this.ts.GetGroupById(model.GroupId);

            if (group.MatchesPlayed == 12)
            {
                model.AvailableGroups = this.SetAvailableGroups();
                this.TempData["WarningMsg"] = $"All results for the games of this Group {group.Name} are already fixed.";

                return this.View(model);
            }

            var match = await this.ms.GetMatch(model.MatchId);

            await this.ts.UpdateGroupMatchesCount(group);
            await this.ms.UpdateMatchResults(model);
            await this.ts.UpdateTeamsStatistics(match.HomeTeam, match.AwayTeam, model.Result, model.WinnerSign, group);
            await this.ps.UpdateUsersScores(group, match, model);

            this.TempData["SuccessMsg"] = "Match result has been added successfully and users scores are updated.";

            return this.RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, User")]
        public async Task<JsonResult> GetAvailableTeams(int groupId)
        {
            var teams = await this.ts.GetAllTeams();
            var availableTeamsVm = this.mapper
                .Map<IEnumerable<Team>, IEnumerable<TeamBindingModel>>(teams)
                .Where(t => t.GroupId == groupId);

            return this.Json(availableTeamsVm);
        }

        [Authorize(Roles = "Admin")]
        public JsonResult GetAvailableMatchesByGroup(int groupId)
        {
            var matches = this.ms.GetMatchesByGroup(groupId);
            var matchesVm = this.mapper
                .Map<ICollection<Match>, ICollection<MatchBindingModel>>(matches)
                .Where(m => m.Result == null && m.WinnerSign == null)
                .ToList();

            return this.Json(matchesVm);
        }

        private IEnumerable<GroupBindingModel> SetAvailableGroups()
        {
            var availableGroups = this.ts.GetAllGroups();

            return this.mapper
                .Map<IEnumerable<Group>, IEnumerable<GroupBindingModel>>(availableGroups);
        }
    }
}