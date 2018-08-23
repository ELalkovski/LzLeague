﻿namespace LzLeague.App.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using LzLeague.Common.AdminBindingModels;
    using LzLeague.Models;
    using LzLeague.Services.Admin.Contracts;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly IArticlesService _as;
        private readonly IMapper mapper;

        public HomeController(IArticlesService _as, IMapper mapper)
        {
            this._as = _as;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var articles = this._as.GetAll();
            var articlesVm = this.mapper
                .Map<ICollection<Article>, ICollection<ArticleBindingModel>>(articles)
                .OrderByDescending(a => a.Id)
                .ToList();

            return this.View(articlesVm);
        }

        [HttpGet]
        public IActionResult Rules()
        {
            return this.View();
        }
    }
}
