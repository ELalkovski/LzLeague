namespace LzLeague.Services.Base
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LzLeague.Common.AdminBindingModels;
    using LzLeague.Common.PredictionsBindingModels;
    using LzLeague.Data;
    using LzLeague.Models;
    using LzLeague.Services.Base.Contracts;
    using Microsoft.EntityFrameworkCore;

    public class PredictionsService : IPredictionsService
    {
        private const int GuessedScorePoints = 5;
        private const int GuessedResultPoints = 2;
        private const int GuessedWinnerPoints = 7;
        private const int GuessedSecondPlacePoints = 5;
        private const int GuessedElQualifierPoints = 3;

        private readonly LzLeagueContext db;

        public PredictionsService(LzLeagueContext db)
        {
            this.db = db;
        }

        public async Task CreateUserPrediction(string userId, PredictionBindingModel model)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.Id == userId);

            Prediction prediction = new Prediction
            {
                OwnerId = userId,
            };

            await this.db.Predictions.AddAsync(prediction);
            await this.db.SaveChangesAsync();
            
            foreach (var resultModel in model.MatchesResults)
            {
                await this.CreateMatchResultPrediction(prediction.Id, resultModel);
            }
            foreach (var groupWinnerModel in model.GroupWinners)
            {
                await this.CreateGroupWinnerPrediction(prediction.Id, groupWinnerModel);
            }

            user.PredictionId = prediction.Id;
            this.db.Users.Update(user);
            await this.db.SaveChangesAsync();
        }

        public async Task<Prediction> GetUserPrediction(string userId)
        {
            var prediction = await this.db
                .Predictions
                .Include(p => p.MatchResultsPredictions)
                .ThenInclude(mrp => mrp.Match)
                .Include(p => p.GroupsWinners)
                .ThenInclude(gwp => gwp.Group)
                .FirstOrDefaultAsync(p => p.OwnerId == userId);

            return prediction;
        }

        public ICollection<Prediction> GetAllPredictionsForStandings()
        {
            var predictions = this.db
                .Predictions
                .Include(p => p.Owner)
                .ToList();

            return predictions;
        }

        public async Task UpdateUsersScores(Group group, Match match, AddResultBindingModel resultModel)
        {
            await this.UpdateScoresAndResults(match, resultModel);

            if (group.MatchesPlayed == 12)
            {
                await this.UpdateRankingResults(group);
            }
        }

        public async Task<Prediction> GetPrediction(int predictionId)
        {
            var prediction = await this.db
                .Predictions
                .Include(p => p.MatchResultsPredictions)
                .ThenInclude(mrp => mrp.Match)
                .Include(p => p.GroupsWinners)
                .ThenInclude(gwp => gwp.Group)
                .FirstOrDefaultAsync(p => p.Id == predictionId);

            return prediction;
        }

        private async Task UpdateRankingResults(Group group)
        {
            var winner = group.Teams.FirstOrDefault(t => t.Position == 1);
            var secondPlace = group.Teams.FirstOrDefault(t => t.Position == 2);
            var thirdPlace = group.Teams.FirstOrDefault(t => t.Position == 3);

            foreach (var winnerPrediction in group.GroupWinnerPredictions)
            {
                var user = await this.db
                    .Users
                    .FirstOrDefaultAsync(u => u.Id == winnerPrediction.Prediction.OwnerId);

                if (winnerPrediction.FirstPlace == winner.Name)
                {
                    user.TotalScore += GuessedWinnerPoints;
                }
                if (winnerPrediction.SecondPlace == secondPlace.Name)
                {
                    user.TotalScore += GuessedSecondPlacePoints;
                }
                if (winnerPrediction.EuropaLeague == thirdPlace.Name)
                {
                    user.TotalScore += GuessedElQualifierPoints;
                }

                this.db.Users.Update(user);
            }

            await this.db.SaveChangesAsync();
        }

        private async Task UpdateScoresAndResults(Match match, AddResultBindingModel resultModel)
        {
            foreach (var resultPrediction in match.MatchResultPredictions)
            {
                var user = await this.db
                    .Users
                    .FirstOrDefaultAsync(u => u.Id == resultPrediction.Prediction.OwnerId);

                if (resultPrediction.Result.Trim() == resultModel.Result.Trim())
                {
                    resultPrediction.Prediction.GuessedScores++;
                    user.TotalScore += GuessedScorePoints;
                }

                if (resultPrediction.WinnerSign.Trim() == resultModel.WinnerSign.Trim())
                {
                    resultPrediction.Prediction.GuessedResults++;
                    user.TotalScore += GuessedResultPoints;
                }

                this.db.Users.Update(user);
                this.db.Predictions.Update(resultPrediction.Prediction);
            }

            await this.db.SaveChangesAsync();
        }

        private async Task CreateMatchResultPrediction(int predictionId, MatchResultBindingModel model)
        {
            var matchResult = new MatchResultPrediction
            {
                PredictionId = predictionId,
                MatchId = model.MatchId,
                Result = model.PredictionResult,
                WinnerSign = model.PredictionSign
            };

            this.db.MatchesResultsPredictions.Add(matchResult);
            await this.db.SaveChangesAsync();
        }

        private async Task CreateGroupWinnerPrediction(int predictionId, GroupWinnerBindingModel model)
        {
            var groupWinner = new GroupWinnerPrediction
            {
                PredictionId = predictionId,
                GroupId = model.GroupId,
                FirstPlace = model.FirstPlace,
                SecondPlace = model.SecondPlace,
                EuropaLeague = model.EuropaLeague
            };

            await this.db.GroupsWinnersPredictions.AddAsync(groupWinner);
            await this.db.SaveChangesAsync();
        }
    }
}
