namespace LzLeague.Services.Admin.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.AdminBindingModels;
    using Models;

    public interface ITeamsService
    {
        Task CreateGroup(Group group);

        Task UpdateTeam(Team team);

        ICollection<Group> GetAllGroups();

        Task<ICollection<Team>> GetAllTeams();

        Task<Group> GetGroupById(int id);

        Task<Group> GetGroupByName(string name);

        Task<string> GetTeamLogo(string teamName);

        Task CreateTeam(Team team);

        Task UpdateGroupMatchesCount(Group group);

        Task UpdateTeamsStatistics(Team homeTeam, Team awayTeam, string score, string result, Group group);

        Task<Team> GetTeamByName(string name);

        Task<Team> GetTeamById(int teamId);

        Task Delete(Team team);

        Task EditTeamsStatistics(Match match, string score, string result);

        Task AddPoints(Team team, string statType);

        Task RemovePoints(Team team, string statType);

        Task Update(Team team);
    }
}
