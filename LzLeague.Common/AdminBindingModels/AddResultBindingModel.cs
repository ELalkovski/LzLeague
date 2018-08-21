namespace LzLeague.Common.AdminBindingModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using LzLeague.Models;

    public class AddResultBindingModel
    {
        [Required(ErrorMessage = "Please select a valid Group.")]
        public int GroupId { get; set; }
        public Group Group { get; set; }

        [Required(ErrorMessage = "Please select valid game.")]
        public int MatchId { get; set; }

        [Required(ErrorMessage = "Score field is required.")]
        public string Result { get; set; }

        [Required(ErrorMessage = "Result field is required.")]
        public string WinnerSign { get; set; }

        public IEnumerable<GroupBindingModel> AvailableGroups { get; set; }
    }
}
