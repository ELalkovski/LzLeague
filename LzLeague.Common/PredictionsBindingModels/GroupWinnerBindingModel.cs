namespace LzLeague.Common.PredictionsBindingModels
{
    using System.ComponentModel.DataAnnotations;
    using LzLeague.Common.AdminBindingModels;

    public class GroupWinnerBindingModel
    {
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
