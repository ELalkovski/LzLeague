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
        public string HomeTeam { get; set; }

        [Required]
        public string AwayTeam { get; set; }

        public string Result { get; set; }

        public string WinnerSign { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan BeginTime { get; set; }

        public ICollection<MatchResultPrediction> MatchResultPredictions { get; set; }
    }
}
