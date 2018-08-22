namespace LzLeague.Services.Admin.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LzLeague.Models;

    public interface IUsersService
    {
        ICollection<ApplicationUser> GetUsers();

        Task UpdateUserPrediction(ICollection<MatchResultPrediction> matchesResults, ICollection<GroupWinnerPrediction> groupWinners);
    }
}
