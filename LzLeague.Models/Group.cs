namespace LzLeague.Models
{
    using System.Collections.Generic;

    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int MatchesPlayed { get; set; }

        public ICollection<Team> Teams { get; set; }

        public ICollection<GroupWinnerPrediction> GroupWinnerPredictions { get; set; }
    }
}
