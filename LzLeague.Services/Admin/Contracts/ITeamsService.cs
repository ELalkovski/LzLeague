namespace LzLeague.Services.Admin.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.AdminBindingModels;
    using Models;

    public interface ITeamsService
    {
        Task CreateGroup(Group group);

        ICollection<Group> GetAllGroups();

        Task<ICollection<Team>> GetAllTeams();

        Task<Group> GetGroupById(int id);

        Task<Group> GetGroupByName(string name);

        Task CreateTeam(Team team);

        Task UpdateGroupMatchesCount(Group group);

        Task UpdateTeamsStatistics(string homeTeamName, string awayTeamName, string score, string result, Group group);

        Task<Team> GetTeamByName(string name);

        Task<Team> GetTeamById(int teamId);

        Task Delete(Team team);
    }
}
