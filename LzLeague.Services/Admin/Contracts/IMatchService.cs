namespace LzLeague.Services.Admin.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LzLeague.Common.AdminBindingModels;
    using Models;

    public interface IMatchService
    {
        Task CreateMatch(Match match);

        Task Delete(Match match);

        ICollection<Match> GetAllMatches();

        ICollection<Match> GetMatchesByGroup(int groupId);

        Task<Match> GetMatch(int matchId);

        Task UpdateMatchResults(AddResultBindingModel model);
    }
}
