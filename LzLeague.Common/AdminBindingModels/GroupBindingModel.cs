namespace LzLeague.Common.AdminBindingModels
{
    using System.Collections.Generic;
    using Models;

    public class GroupBindingModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int MatchesPlayed { get; set; }

        public ICollection<TeamBindingModel> Teams { get; set; }

        public ICollection<GroupWinnerPrediction> GroupWinnerPredictions { get; set; }
    }
}
