namespace LzLeague.App.Mapping
{
    using AutoMapper;
    using LzLeague.Models;

    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            this.CreateMap<Article, ArticleProfile>()
                .ReverseMap();
        }
    }
}
