namespace LzLeague.Tests.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using LzLeague.Data;
    using LzLeague.Models;
    using LzLeague.Services.Admin;
    using LzLeague.Tests.Mocks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ArticlesServiceTests
    {
        private LzLeagueContext dbContext;

        [TestMethod]
        public async Task AddSingleArticle_Test()
        {
            // Arrange
            var articlesService = new ArticlesService(this.dbContext);

            // Act 
            await articlesService.Create(new Article { Title = "TestTitle", Content = "TestContent" });
            var articles = this.dbContext.Articles;

            // Assert
            Assert.AreEqual(1, articles.Count());
        }

        [TestMethod]
        public async Task AddMultipleArticles_Test()
        {
            // Arrange
            var articlesService = new ArticlesService(this.dbContext);

            // Act 
            await articlesService.Create(new Article { Title = "TestTitle2", Content = "TestContent2" });
            await articlesService.Create(new Article { Title = "TestTitle3", Content = "TestContent3" });
            await articlesService.Create(new Article { Title = "TestTitle4", Content = "TestContent4" });

            var articles = this.dbContext.Articles;

            // Assert
            Assert.AreEqual(3, articles.Count());
        }

        [TestMethod]
        public async Task DeleteArticle_Test()
        {
            // Arrange
            var articlesService = new ArticlesService(this.dbContext);
            var articles = this.dbContext.Articles;

            articles.Add(new Article { Title = "ArticleToDelete", Content = "DeleteContent" });
            await this.dbContext.SaveChangesAsync();

            var articleToDelete = this.dbContext
                .Articles
                .FirstOrDefault(a => a.Title == "ArticleToDelete");

            // Act
            await articlesService.Delete(articleToDelete);

            // Assert
            Assert.AreEqual(false, articles.Any(a => a.Title == "ArticleToDelete"));
        }

        [TestMethod]
        public async Task GetArticleById_Test()
        {
            // Arrange
            var articlesService = new ArticlesService(this.dbContext);

            this.dbContext.Articles.Add(new Article { Title = "GetTitle", Content = "GetContent" });
            await this.dbContext.SaveChangesAsync();

            // Act
            var article = await articlesService.GetArticle(1);

            // Assert
            Assert.AreEqual("GetTitle", article.Title);
        }

        [TestMethod]
        public async Task GetArticleById_ShouldReturnNull_Test()
        {
            // Arrange
            var articlesService = new ArticlesService(this.dbContext);

            this.dbContext.Articles.Add(new Article { Title = "TestTitle1", Content = "TestContent1" });
            this.dbContext.Articles.Add(new Article { Title = "TestTitle2", Content = "TestContent2" });
            await this.dbContext.SaveChangesAsync();

            // Act
            var article = await articlesService.GetArticle(15);

            // Assert
            Assert.AreEqual(null, article);
        }

        [TestMethod]
        public async Task GetAllArticles_Test()
        {
            // Arrange
            var articlesService = new ArticlesService(this.dbContext);

            this.dbContext.Articles.Add(new Article { Title = "TestTitle1", Content = "TestContent1" });
            this.dbContext.Articles.Add(new Article { Title = "TestTitle2", Content = "TestContent2" });
            this.dbContext.Articles.Add(new Article { Title = "TestTitle3", Content = "TestContent3" });
            await this.dbContext.SaveChangesAsync();

            // Act
            var articles = articlesService.GetAll();

            // Assert
            Assert.AreEqual(3, articles.Count);
        }

        [TestInitialize]
        public void InitializeDbContext()
        {
            this.dbContext = MockDbContext.GetContext();
        }
    }
}
