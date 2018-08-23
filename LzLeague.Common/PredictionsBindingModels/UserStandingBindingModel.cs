namespace LzLeague.Common.PredictionsBindingModels
{
    public class UserStandingBindingModel
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public int GuessedScores { get; set; }

        public int GuessedResults { get; set; }

        public int GuessedGroupWinners { get; set; }

        public int GuessedSecondPlaces { get; set; }

        public int GuessedElTeams { get; set; }

        public int TotalScore { get; set; }
    }
}
