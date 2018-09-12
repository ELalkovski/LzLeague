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
        [RegularExpression("^(?!.*Group Winner).*$", ErrorMessage = "You haven't selected Group winner!")]
        public string FirstPlace { get; set; }

        [Required]
        [RegularExpression("^(?!.*Second Place).*$", ErrorMessage = "You haven't selected Second place!")]
        public string SecondPlace { get; set; }

        [Required]
        [RegularExpression("^(?!.*Europa League Qualifier).*$", ErrorMessage = "You haven't selected Europa League qualifier!")]
        public string EuropaLeague { get; set; }
    }
}
