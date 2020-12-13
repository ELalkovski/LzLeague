namespace LzLeague.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Match
    {
        public int Id { get; set; }

        public int? GroupId { get; set; }
        public Group Group { get; set; }

        [Required]
        public int HomeTeamId { get; set; }
        public Team HomeTeam { get; set; }

        [Required]
        public int AwayTeamId { get; set; }
        public Team AwayTeam { get; set; }

        public string Result { get; set; }

        public string WinnerSign { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan BeginTime { get; set; }

        public ICollection<MatchResultPrediction> MatchResultPredictions { get; set; }
    }
}
