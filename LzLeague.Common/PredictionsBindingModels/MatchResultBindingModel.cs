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

        [Required]
        public string Result { get; set; }

        [Required]
        public string Sign { get; set; }
    }
}
