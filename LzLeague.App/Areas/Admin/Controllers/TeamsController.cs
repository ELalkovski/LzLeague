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

        [Authorize(Roles = "Admin")]
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
    }
}