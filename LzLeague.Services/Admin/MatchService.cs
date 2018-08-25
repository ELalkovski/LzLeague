namespace LzLeague.Services.Admin
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;
    using LzLeague.Common.AdminBindingModels;
    using LzLeague.Data;
    using LzLeague.Models;
    using Microsoft.EntityFrameworkCore;

    public class MatchService : IMatchService
    {
        private readonly LzLeagueContext db;

        public MatchService(LzLeagueContext db)
        {
            this.db = db;
        }

        public async Task CreateMatch(Match match)
        {
            await this.db.Matches.AddAsync(match);
            await this.db.SaveChangesAsync();
        }

        public async Task Delete(Match match)
        {
            this.db.Matches.Remove(match);
            await this.db.SaveChangesAsync();
        }

        public async Task<Match> GetMatch(int matchId)
        {
            return await this.db
                .Matches
                .Include(m => m.MatchResultPredictions)
                .ThenInclude(mrp => mrp.Prediction)
                .FirstOrDefaultAsync(m => m.Id == matchId);
        }

        public async Task UpdateMatchResults(AddResultBindingModel model)
        {
            var match = await this.GetMatch(model.MatchId);

            match.Result = model.Result;
            match.WinnerSign = model.WinnerSign.ToLower();

            this.db.Matches.Update(match);
            await this.db.SaveChangesAsync();
        }

        public ICollection<Match> GetAllMatches()
        {
            return this.db
                .Matches
                .Include(m => m.MatchResultPredictions)
                .ThenInclude(mrp => mrp.Prediction)
                .Include(m => m.MatchResultPredictions)
                .ThenInclude(mrp => mrp.Match)
                .Include(m => m.Group)
                .ToList();
        }

        public ICollection<Match> GetMatchesByGroup(int groupId)
        {
            return this.db
                .Matches
                .Where(m => m.GroupId == groupId)
                .Include(m => m.Group)
                .ToList();
        }
    }
}
