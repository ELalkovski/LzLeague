namespace LzLeague.Services.Admin
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LzLeague.Data;
    using LzLeague.Models;
    using LzLeague.Services.Admin.Contracts;
    using Microsoft.EntityFrameworkCore;

    public class UsersService : IUsersService
    {
        private readonly LzLeagueContext db;

        public UsersService(LzLeagueContext db)
        {
            this.db = db;
        }

        public ICollection<ApplicationUser> GetUsers()
        {
            return this.db
                .Users
                .Include(u => u.Prediction)
                .ToList();
        }

        public async Task UpdateUserPrediction(
            ICollection<MatchResultPrediction> matchesResults, 
            ICollection<GroupWinnerPrediction> groupWinners)
        {
            await this.UpdateUserMatchesResults(matchesResults);
            await this.UpdateUserGroupsWinners(groupWinners);
        }

        private async Task UpdateUserGroupsWinners(ICollection<GroupWinnerPrediction> groupWinners)
        {
            foreach (var modifiedGroupWinner in groupWinners)
            {
                var existingGroupWinner = await this.db
                    .GroupsWinnersPredictions
                    .FirstOrDefaultAsync(
                        m => m.PredictionId == modifiedGroupWinner.PredictionId && m.GroupId == modifiedGroupWinner.GroupId);

                this.db.Entry(existingGroupWinner).CurrentValues.SetValues(modifiedGroupWinner);
                this.db.Entry(existingGroupWinner).Property(x => x.PredictionId).IsModified = false;
                this.db.Entry(existingGroupWinner).Property(x => x.GroupId).IsModified = false;
                await this.db.SaveChangesAsync();
            }
        }

        private async Task UpdateUserMatchesResults(ICollection<MatchResultPrediction> matchesResults)
        {
            foreach (var modifiedMatchResult in matchesResults)
            {
                var existingMatchResult = await this.db
                    .MatchesResultsPredictions
                    .FirstOrDefaultAsync(
                        m => m.PredictionId == modifiedMatchResult.PredictionId && m.MatchId == modifiedMatchResult.MatchId);

                this.db.Entry(existingMatchResult).CurrentValues.SetValues(modifiedMatchResult);
                this.db.Entry(existingMatchResult).Property(x => x.PredictionId).IsModified = false;
                this.db.Entry(existingMatchResult).Property(x => x.MatchId).IsModified = false;
                await this.db.SaveChangesAsync();
            }
        }
    }
}