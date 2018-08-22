namespace LzLeague.Common.PredictionsBindingModels
{
    using System.ComponentModel.DataAnnotations;
    using LzLeague.Common.AdminBindingModels;
    using LzLeague.Models;

    public class GroupWinnerBindingModel
    {
        public int PredictionId { get; set; }
        public Prediction Prediction { get; set; }

        public int GroupId { get; set; }
        public GroupBindingModel Group { get; set; }

        [Required]
        public string FirstPlace { get; set; }

        [Required]
        public string SecondPlace { get; set; }

        [Required]
        public string EuropaLeague { get; set; }
    }
}
