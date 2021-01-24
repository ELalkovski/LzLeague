namespace LzLeague.Common.AdminBindingModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Models;

    public class TeamBindingModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Points { get; set; }

        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        public int Position { get; set; }

        public int PlayedMatchesCount { get; set; }

        public int Wins { get; set; }

        public int Loses { get; set; }

        public int Draws { get; set; }

        public int GoalsReceived { get; set; }

        public int GoalsScored { get; set; }

        public int? GroupId { get; set; }
        public GroupBindingModel Group { get; set; }

        public ICollection<Match> PlayedMatches { get; set; }
    }
}
