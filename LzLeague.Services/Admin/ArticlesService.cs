namespace LzLeague.Services.Admin
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LzLeague.Data;
    using LzLeague.Models;
    using LzLeague.Services.Admin.Contracts;
    using Microsoft.EntityFrameworkCore;

    public class ArticlesService : IArticlesService
    {
        private readonly LzLeagueContext db;

        public ArticlesService(LzLeagueContext db)
        {
            this.db = db;
        }

        public async Task Create(Article article)
        {
            this.db.Articles.Add(article);
            await this.db.SaveChangesAsync();
        }

        public ICollection<Article> GetAll()
        {
            return this.db
                .Articles
                .ToList();
        }

        public async Task Delete(Article article)
        {
            this.db.Articles.Remove(article);
            await this.db.SaveChangesAsync();
        }

        public async Task<Article> GetArticle(int id)
        {
            var article = await this.db
                .Articles
                .FirstOrDefaultAsync(a => a.Id == id);

            return article;
        }
    }
}
