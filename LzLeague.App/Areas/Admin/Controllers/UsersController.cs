namespace LzLeague.App.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using AutoMapper;
    using LzLeague.Common.AdminBindingModels;
    using LzLeague.Models;
    using LzLeague.Services.Admin.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IUsersService us;
        private readonly IMapper mapper;

        public UsersController(IUsersService us, IMapper mapper)
        {
            this.us = us;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var users = this.us.GetUsers();
            var usersVm = this.mapper
                .Map<ICollection<ApplicationUser>, ICollection<UserBindingModel>>(users);

            return this.View(usersVm);
        }
    }
}