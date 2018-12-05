namespace LzLeague.Services.Admin.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LzLeague.Models;

    public interface IUsersService
    {
        ICollection<ApplicationUser> GetUsers();

        Task<ApplicationUser> GetUserByEmail(string email);

        Task Delete(ApplicationUser user);

        Task UpdateUser(ApplicationUser user);

        Task UpdateUserPrediction(ICollection<MatchResultPrediction> matchesResults, ICollection<GroupWinnerPrediction> groupWinners);

        Task AddPoints(ApplicationUser user, string pointsType);

        Task RemovePoints(ApplicationUser user, string pointsType);
    }
}
