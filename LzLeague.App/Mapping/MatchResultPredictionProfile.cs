namespace LzLeague.App.Mapping
{
    using AutoMapper;
    using LzLeague.Common.PredictionsBindingModels;
    using LzLeague.Models;

    public class MatchResultPredictionProfile : Profile
    {
        public MatchResultPredictionProfile()
        {
            this.CreateMap<MatchResultPrediction, MatchResultBindingModel>()
                .ForMember(dest => dest.Sign, opt => opt.MapFrom(x => x.WinnerSign))
                .ReverseMap();
        }
    }
}
