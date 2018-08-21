namespace LzLeague.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public int TotalScore { get; set; }

        public int? PredictionId { get; set; }
        public Prediction Prediction { get; set; }
    }
}
