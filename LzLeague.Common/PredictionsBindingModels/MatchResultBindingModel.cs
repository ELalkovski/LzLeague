namespace LzLeague.Common.PredictionsBindingModels
{
    using System.ComponentModel.DataAnnotations;
    using LzLeague.Common.AdminBindingModels;
    using LzLeague.Models;

    public class MatchResultBindingModel
    {
        public int PredictionId { get; set; }
        public Prediction Prediction { get; set; }

        public int MatchId { get; set; }
        public MatchBindingModel Match { get; set; }

        public string HomeTeamLogo { get; set; }

        public string AwayTeamLogo { get; set; }

        [Required(ErrorMessage = "This match Score is empty!")]
        [RegularExpression(@"^\d+:\d+$", ErrorMessage = "Your Score is not in correct format! The correct format is 1:1, 2:0 etc.")]
        public string PredictionResult { get; set; }

        [Required(ErrorMessage = "This match result is empty!")]
        [RegularExpression("^(?!.*Result).*$", ErrorMessage = "You haven't selected Result sign!")]
        public string PredictionSign { get; set; }
    }
}
