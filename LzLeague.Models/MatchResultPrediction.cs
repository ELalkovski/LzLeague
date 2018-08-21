namespace LzLeague.Models
{
    using System.ComponentModel.DataAnnotations;

    public class MatchResultPrediction
    {
        [Required]
        public int PredictionId { get; set; }
        public Prediction Prediction { get; set; }

        [Required]
        public int MatchId { get; set; }
        public Match Match { get; set; }

        public string Result { get; set; }

        public string WinnerSign { get; set; }
    }
}
