namespace LzLeague.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Prediction
    {
        public Prediction()
        {
            this.MatchResultsPredictions = new List<MatchResultPrediction>();
            this.GroupsWinners = new List<GroupWinnerPrediction>();
        }

        public int Id { get; set; }

        [Required]
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }

        public int GuessedScores { get; set; }

        public int GuessedResults { get; set; }

        public int GuessedGroupWinners { get; set; }

        public int GuessedSecondPlaces { get; set; }

        public int GuessedElTeams { get; set; }

        public ICollection<MatchResultPrediction> MatchResultsPredictions { get; set; }

        public ICollection<GroupWinnerPrediction> GroupsWinners { get; set; }
    }
}
