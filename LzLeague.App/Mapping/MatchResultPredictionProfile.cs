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
                .ForMember(dest => dest.PredictionSign, opt => opt.MapFrom(x => x.WinnerSign))
                .ForMember(dest => dest.PredictionResult, opt => opt.MapFrom(x => x.Result))
                .ReverseMap();
        }
    }
}
