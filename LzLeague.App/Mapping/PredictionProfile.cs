namespace LzLeague.App.Mapping
{
    using AutoMapper;
    using LzLeague.Common.PredictionsBindingModels;
    using LzLeague.Models;

    public class PredictionProfile : Profile
    {
        public PredictionProfile()
        {
            this.CreateMap<Prediction, PredictionBindingModel>()
                .ReverseMap();

            // WARNING: Possible bugs because of the ReverseMap()

            this.CreateMap<Prediction, UserStandingBindingModel>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(x => x.Owner.FullName))
                .ForMember(dest => dest.TotalScore, opt => opt.MapFrom(x => x.Owner.TotalScore))
                .ReverseMap();
        }
    }
}
