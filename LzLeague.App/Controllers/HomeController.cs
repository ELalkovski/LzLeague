namespace LzLeague.App.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using LzLeague.Common.AdminBindingModels;
    using LzLeague.Models;
    using LzLeague.Services.Admin.Contracts;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly IArticlesService _as;
        private readonly IMapper mapper;
        private readonly IEmailSender _emailSender;

        public HomeController(IArticlesService _as, IMapper mapper, IEmailSender emailSender)
        {
            this._as = _as;
            this.mapper = mapper;
            this._emailSender = emailSender;
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
