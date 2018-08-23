namespace LzLeague.Services.Admin.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LzLeague.Models;

    public interface IArticlesService
    {
        Task Create(Article article);

        ICollection<Article> GetAll();

        Task Delete(Article article);

        Task<Article> GetArticle(int id);
    }
}
