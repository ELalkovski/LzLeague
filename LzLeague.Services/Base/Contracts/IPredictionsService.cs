namespace LzLeague.Services.Base.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LzLeague.Common.AdminBindingModels;
    using LzLeague.Common.PredictionsBindingModels;
    using LzLeague.Models;

    public interface IPredictionsService
    {
        Task CreateUserPrediction(string userId, PredictionBindingModel model);

        Task<Prediction> GetUserPrediction(string userId);

        ICollection<Prediction> GetAllPredictionsForStandings();

        Task UpdateUsersScores(Group group, Match match, AddResultBindingModel resultModel);

        Task<Prediction> GetPrediction(int predictionId);

        Task EditUsersScores(Match match);
    }
}
