namespace LzLeague.Common.AdminBindingModels
{
    using LzLeague.Models;

    public class UserBindingModel
    {
        public string Email { get; set; }

        public string FullName { get; set; }

        public int TotalScore { get; set; }

        public int? PredictionId { get; set; }
        public Prediction Prediction { get; set; }
    }
}
