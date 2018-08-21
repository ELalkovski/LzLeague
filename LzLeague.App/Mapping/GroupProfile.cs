namespace LzLeague.App.Mapping
{
    using AutoMapper;
    using Common.AdminBindingModels;
    using LzLeague.Common.PredictionsBindingModels;
    using LzLeague.Models;

    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            this.CreateMap<Group, GroupBindingModel>()
                .ReverseMap();

            this.CreateMap<Group, GroupWinnerBindingModel>()
                .ForMember(x => x.GroupId, opt => opt.MapFrom(g => g.Id))
                .ForMember(x => x.Group, opt => opt.MapFrom(g => g))
                .ReverseMap();

            //.ForMember(x => x.FirstPlace, opt => opt.Ignore())
            //.ForMember(x => x.SecondPlace, opt => opt.Ignore())
            //.ForMember(x => x.EuropaLeague, opt => opt.Ignore());
        }
    }
}
