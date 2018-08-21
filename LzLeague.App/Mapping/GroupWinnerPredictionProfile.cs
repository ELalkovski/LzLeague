namespace LzLeague.App.Mapping
{
    using AutoMapper;
    using LzLeague.Common.PredictionsBindingModels;
    using LzLeague.Models;

    public class GroupWinnerPredictionProfile : Profile
    {
        public GroupWinnerPredictionProfile()
        {
            this.CreateMap<GroupWinnerPrediction, GroupWinnerBindingModel>()
                .ReverseMap();
        }
    }
}
