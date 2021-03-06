﻿namespace LzLeague.App.Mapping
{
    using AutoMapper;
    using Common.AdminBindingModels;
    using LzLeague.Common.PredictionsBindingModels;
    using LzLeague.Models;

    public class MatchProfile : Profile
    {
        public MatchProfile()
        {
            this.CreateMap<Match, MatchBindingModel>()
                .ForMember(x => x.AvailableGroups, opt => opt.Ignore())
                .ForMember(x => x.AvailableTeams, opt => opt.Ignore())
                .ReverseMap();

            this.CreateMap<Match, MatchResultBindingModel>()
                .ForMember(x => x.MatchId, opt => opt.MapFrom(m => m.Id))
                .ForMember(x => x.Match, opt => opt.MapFrom(m => m));
        }
    }
}
