namespace LzLeague.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using LzLeague.Common.AdminBindingModels;
    using LzLeague.Models;
    using LzLeague.Services.Admin.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ArticlesController : Controller
    {
        private readonly IArticlesService _as;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public ArticlesController(IArticlesService _as, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            this._as = _as;
            this.mapper = mapper;
            this.userManager = userManager;
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

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var article = await this._as.GetArticle(id);
            var model = this.mapper.Map<ArticleBindingModel>(article);

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(ArticleBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return this.View(model);
            }

            var article = this.mapper.Map<ArticleBindingModel, Article>(model);

            await this._as.Update(article);
            this.TempData["SuccessMsg"] = "Article has been created successfully.";

            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> AddComment(CreateCommentBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Read", new { articleId = model.ArticleId });
            }

            var authorId = this.userManager.GetUserId(this.User);
            var comment = new Comment
            {
                ArticleId = model.ArticleId,
                AuthorId = authorId,
                Content = model.Content,
                PublicationDate = DateTime.UtcNow
            };

            await this._as.CreateComment(comment);

            return this.RedirectToAction("Read", new { articleId = model.ArticleId });
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

            articleVm.Comments = articleVm
                .Comments
                .OrderByDescending(c => c.PublicationDate)
                .ToList();

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

            await this._as.DeleteAllArticlesComments(articleId);
            await this._as.Delete(article);
            this.TempData["SuccessMsg"] = "Article has been deleted successfully.";

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteComment(int commentId, int articleId)
        {
            var comment = await this._as.GetComment(commentId);

            if (comment == null)
            {
                this.TempData["WarningMsg"] = "Comment you are trying to reach doesn't exist";
                return this.RedirectToAction("Read", new { articleId });
            }
            
            await this._as.DeleteComment(comment);
            this.TempData["SuccessMsg"] = "Comment has been deleted successfully.";

            return this.RedirectToAction("Read", new { articleId });
        }
    }
}