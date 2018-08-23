namespace LzLeague.App.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using LzLeague.Common.AdminBindingModels;
    using LzLeague.Models;
    using LzLeague.Services.Admin.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ArticlesController : Controller
    {
        private readonly IArticlesService _as;
        private readonly IMapper mapper;

        public ArticlesController(IArticlesService _as, IMapper mapper)
        {
            this._as = _as;
            this.mapper = mapper;
        }
        
        [HttpGet]
        [AllowAnonymous]
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
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            var model = new ArticleBindingModel();

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(ArticleBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var article = this.mapper.Map<ArticleBindingModel, Article>(model);

            await this._as.Create(article);
            this.TempData["SuccessMsg"] = "Article has been created successfully.";

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Read(int articleId)
        {
            var article = await this._as.GetArticle(articleId);

            if (article == null)
            {
                this.TempData["WarningMsg"] = "Article you are trying to reach doesn't exist";
                return this.RedirectToAction("Index");
            }

            var articleVm = this.mapper.Map<Article, ArticleBindingModel>(article);

            return this.View(articleVm);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int articleId)
        {
            var article = await this._as.GetArticle(articleId);

            if (article == null)
            {
                this.TempData["WarningMsg"] = "Article you are trying to reach doesn't exist";
                return this.RedirectToAction("Read", new { articleId });
            }

            await this._as.Delete(article);
            this.TempData["SuccessMsg"] = "Article has been deleted successfully.";

            return this.RedirectToAction("Index");
        }
    }
}