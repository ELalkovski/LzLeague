namespace LzLeague.Models
{
    using System.ComponentModel.DataAnnotations;

    public class GroupWinnerPrediction
    {
        [Required]
        public int GroupId { get; set; }
        public Group Group { get; set; }

        [Required]
        public int PredictionId { get; set; }
        public Prediction Prediction { get; set; }

        public string FirstPlace { get; set; }

        public string SecondPlace { get; set; }

        public string EuropaLeague { get; set; }
    }
}
