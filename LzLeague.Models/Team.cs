namespace LzLeague.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Team
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Points { get; set; }

        public string ImageUrl { get; set; }

        public int Position { get; set; }

        public int Wins { get; set; }

        public int Loses { get; set; }

        public int Draws { get; set; }

        public int GoalsReceived { get; set; }

        public int GoalsScored { get; set; }

        [Required]
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public IList<Match> HomePlayedMatches { get; set; }

        public IList<Match> AwayPlayedMatches { get; set; }
    }
}
