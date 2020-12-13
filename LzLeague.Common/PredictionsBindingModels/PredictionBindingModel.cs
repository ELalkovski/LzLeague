namespace LzLeague.Common.PredictionsBindingModels
{
    using System.Collections.Generic;

    public class PredictionBindingModel
    {
        public int Id { get; set; }

        public IList<MatchResultBindingModel> MatchesResults { get; set; }

        public ICollection<GroupWinnerBindingModel> GroupWinners { get; set; }
    }
}
