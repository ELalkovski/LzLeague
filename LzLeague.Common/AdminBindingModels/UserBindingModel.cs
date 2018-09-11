namespace LzLeague.Common.AdminBindingModels
{
    using LzLeague.Models;

    public class UserBindingModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public int TotalScore { get; set; }

        public bool IsApproved { get; set; }

        public int? PredictionId { get; set; }
        public Prediction Prediction { get; set; }
    }
}
