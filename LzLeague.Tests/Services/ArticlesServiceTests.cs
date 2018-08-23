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
        public async Task AddSingleArticleTest()
        {
            // Arrange
            var articlesService = new ArticlesService(this.dbContext);

            // Act 
            await articlesService.Create(new Article { Title = "TestTile", Content = "TestContent" });
            var articles = this.dbContext.Articles;

            // Assert
            Assert.AreEqual(1, articles.Count());
        }

        [TestMethod]
        public async Task AddMultipleArticlesTest()
        {
            // Arrange
            var articlesService = new ArticlesService(this.dbContext);

            // Act 
            await articlesService.Create(new Article { Title = "TestTile1", Content = "TestContent1" });
            await articlesService.Create(new Article { Title = "TestTile2", Content = "TestContent2" });
            await articlesService.Create(new Article { Title = "TestTile3", Content = "TestContent3" });

            var articles = this.dbContext.Articles;

            // Assert
            Assert.AreEqual(3, articles.Count());
        }

        [TestMethod]
        public async Task DeleteArticleTest()
        {
            // Arrange
            var articlesService = new ArticlesService(this.dbContext);
            var articles = this.dbContext.Articles;

            await articlesService.Create(new Article { Title = "TestTile1", Content = "TestContent1" });
            await articlesService.Create(new Article { Title = "ArticleToDelete", Content = "DeleteContent" });

            var articleToDelete = this.dbContext
                .Articles
                .FirstOrDefault(a => a.Title == "ArticleToDelete");

            // Act
            await articlesService.Delete(articleToDelete);

            // Assert
            //Assert.
        }

        [TestInitialize]
        public void InitializeDbContext()
        {
            this.dbContext = MockDbContext.GetContext();
        }
    }
}
