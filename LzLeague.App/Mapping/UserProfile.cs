namespace LzLeague.App.Mapping
{
    using AutoMapper;
    using LzLeague.Common.AdminBindingModels;
    using LzLeague.Models;

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            this.CreateMap<ApplicationUser, UserBindingModel>()
                .ReverseMap();
        }
    }
}
