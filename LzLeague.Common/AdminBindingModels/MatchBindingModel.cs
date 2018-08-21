namespace LzLeague.Common.AdminBindingModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Models;

    public class MatchBindingModel
    {
        public MatchBindingModel()
        {
            this.MatchResultPredictions = new List<MatchResultPrediction>();
            this.AvailableGroups = new List<GroupBindingModel>();
            this.AvailableTeams = new List<TeamBindingModel>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a valid Group")]
        public int? GroupId { get; set; }
        public Group Group { get; set; }

        [Required(ErrorMessage = "Please select home team")]
        public string HomeTeam { get; set; }

        [Required(ErrorMessage = "Please select away team")]
        public string AwayTeam { get; set; }

        public string Result { get; set; }

        public string WinnerSign { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        [Required]
        public TimeSpan? BeginTime { get; set; }

        public ICollection<MatchResultPrediction> MatchResultPredictions { get; set; }

        public IEnumerable<GroupBindingModel> AvailableGroups { get; set; }

        public IEnumerable<TeamBindingModel> AvailableTeams { get; set; }
    }
}
