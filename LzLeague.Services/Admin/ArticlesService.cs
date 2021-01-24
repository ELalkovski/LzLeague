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

        public async Task Update(Article article)
        {
            this.db.Articles.Update(article);
            await this.db.SaveChangesAsync();
        }

        public async Task CreateComment(Comment comment)
        {
            this.db.Comments.Add(comment);
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
            article.Comments = new List<Comment>();
            this.db.Articles.Remove(article);
            await this.db.SaveChangesAsync();
        }

        public async Task DeleteAllArticlesComments(int articleId)
        {
            var comments = this.db.Comments
                .Where(c => c.ArticleId == articleId);

            this.db.Comments.RemoveRange(comments);
            await this.db.SaveChangesAsync();
        }

        public async Task<Article> GetArticle(int id)
        {
            var article = await this.db
                .Articles
                .AsNoTracking()
                .Include(a => a.Comments)
                .ThenInclude(c => c.Author)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            return article;
        }

        public async Task<Comment> GetComment(int id)
        {
            var comment = await this.db
                .Comments
                .Include(c => c.Article)
                .Include(c => c.Author)
                .FirstOrDefaultAsync(c => c.Id == id);

            return comment;
        }

        public async Task DeleteComment(Comment comment)
        {
            this.db.Comments.Remove(comment);
            await this.db.SaveChangesAsync();
        }
    }
}
