namespace LzLeague.App.Mapping
{
    using AutoMapper;
    using Common.AdminBindingModels;
    using LzLeague.Models;

    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            this.CreateMap<Article, ArticleBindingModel>()
                .ForMember(x => x.CreateCommentModel, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
