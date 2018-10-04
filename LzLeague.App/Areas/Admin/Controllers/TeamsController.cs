namespace LzLeague.App.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Common.AdminBindingModels;
    using LzLeague.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Admin.Contracts;

    [Area("Admin")]
    public class TeamsController : Controller
    {
        private readonly ITeamsService ts;
        private readonly IMapper mapper;

        public TeamsController(ITeamsService ts, IMapper mapper)
        {
            this.ts = ts;
            this.mapper = mapper;
        }

        [HttpGet("/Teams")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            var groups = this.ts.GetAllGroups();
            var groupsViewModels = this.mapper
                .Map<IEnumerable<Group>, IEnumerable<GroupBindingModel>>(groups)
                .ToList();
            
            return this.View(groupsViewModels);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return this.View(new TeamBindingModel());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(TeamBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var group = await this.ts.GetGroupByName(model.Group.Name);

            if (group == null)
            {
                group = new Group
                {
                    Name = model.Group.Name
                };

                await this.ts.CreateGroup(group);
            }

            var team = this.mapper.Map<Team>(model);
            team.GroupId = group.Id;

            await this.ts.CreateTeam(team);

            this.TempData["SuccessMsg"] = "Team has been added Successfully!";

            return this.RedirectToAction("Index");
        }

        [HttpGet("/Details/{teamId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int teamId)
        {
            var team = await this.ts.GetTeamById(teamId);
            var teamVm = this.mapper.Map<Team, TeamBindingModel>(team);

            return this.View(teamVm);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int teamId)
        {
            var team = await this.ts.GetTeamById(teamId);

            if (team == null)
            {
                this.TempData["WarningMsg"] = "Sorry, team you are trying to reach doesn't exist.";
                return this.RedirectToAction("Index");
            }
            if (team.Group.MatchesPlayed > 0)
            {
                this.TempData["WarningMsg"] = 
                    $"Matches from Group {team.Group.Name} have already being played. You cannot delete team that started playing.";
                return this.RedirectToAction("Index");
            }

            await this.ts.Delete(team);

            this.TempData["SuccessMsg"] = "Team was deleted successfully.";
            
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeStatistics(int teamId, string statType)
        {
            var team = await this.ts.GetTeamById(teamId);

            if (team == null)
            {
                this.TempData["WarningMsg"] = "Sorry, team you are trying to reach doesn't exist.";
                return this.RedirectToAction("Index");
            }

            await this.ts.AddStatistics(team, statType);

            return this.RedirectToAction("Details", new { teamId });
        }
    }
}