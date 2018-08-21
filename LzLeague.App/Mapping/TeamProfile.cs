namespace LzLeague.App.Mapping
{
    using AutoMapper;
    using Common.AdminBindingModels;
    using LzLeague.Models;

    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            this.CreateMap<TeamBindingModel, Team>()
                .ReverseMap();
        }
    }
}
