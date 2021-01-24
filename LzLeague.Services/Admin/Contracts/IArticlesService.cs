namespace LzLeague.Services.Admin.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LzLeague.Models;

    public interface IArticlesService
    {
        Task Create(Article article);

        Task Update(Article article);

        Task CreateComment(Comment comment);

        ICollection<Article> GetAll();

        Task Delete(Article article);

        Task DeleteComment(Comment comment);

        Task DeleteAllArticlesComments(int articleId);

        Task<Article> GetArticle(int id);

        Task<Comment> GetComment(int id);
    }
}
