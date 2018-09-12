namespace LzLeague.Services.Admin
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;
    using Data;
    using LzLeague.Common.AdminBindingModels;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class TeamsService : ITeamsService
    {
        private const int WinPoints = 3;
        private const int DrawPoints = 1;
        
        private readonly LzLeagueContext db;

        public TeamsService(LzLeagueContext db)
        {
            this.db = db;
        }

        public async Task CreateGroup(Group group)
        {
            this.db.Groups.Add(group);
            await this.db.SaveChangesAsync();
        }

        public ICollection<Group> GetAllGroups()
        {
            var groups = this.db
                .Groups
                .Include(g => g.Teams)
                .Include(g => g.GroupWinnerPredictions)
                .ToList();

            foreach (var group in groups)
            {
                group.Teams = group.Teams
                    .OrderByDescending(t => t.Points)
                    .ToList();
            }

            return groups;
        }

        public async Task<Group> GetGroupById(int id)
        {
            var group = await this.db
                .Groups
                .Include(g => g.Teams)
                .Include(g => g.GroupWinnerPredictions)
                .ThenInclude(gwp => gwp.Prediction)
                .FirstOrDefaultAsync(g => g.Id == id);

            return group;
        }

        public async Task<Group> GetGroupByName(string name)
        {
            var group = await this.db
                .Groups
                .Include(g => g.Teams)
                .Include(g => g.GroupWinnerPredictions)
                .FirstOrDefaultAsync(g => g.Name == name);

            return group;
        }

        public async Task<string> GetTeamLogo(string teamName)
        {
            var team = await this.db
                .Teams
                .FirstOrDefaultAsync(t => t.Name == teamName);

            return team.ImageUrl;
        }

        public async Task CreateTeam(Team team)
        {
            this.db.Teams.Add(team);
            await this.db.SaveChangesAsync();
        }

        public async Task UpdateGroupMatchesCount(Group group)
        {
            group.MatchesPlayed++;
            this.db.Groups.Update(group);
            await this.db.SaveChangesAsync();
        }

        public async Task UpdateTeamsStatistics
            (string homeTeamName,
            string awayTeamName,
            string score,
            string result,
            Group group)
        {
            var homeTeam = await this.GetTeamByName(homeTeamName);
            var awayTeam = await this.GetTeamByName(awayTeamName);
            var goalsArgs = score
                .Split(':')
                .Select(int.Parse)
                .ToList();

            homeTeam.GoalsScored += goalsArgs[0];
            homeTeam.GoalsReceived += goalsArgs[1];
            awayTeam.GoalsScored += goalsArgs[1];
            awayTeam.GoalsReceived += goalsArgs[0];

            if (result.Trim() == "1")
            {
                homeTeam.Wins++;
                homeTeam.Points += WinPoints;
                awayTeam.Loses++;
            }
            else if (result.Trim() == "2")
            {
                homeTeam.Loses++;
                awayTeam.Wins++;
                awayTeam.Points += WinPoints;
            }
            else if (result.Trim() == "x")
            {
                homeTeam.Draws++;
                homeTeam.Points += DrawPoints;
                awayTeam.Draws++;
                awayTeam.Points += DrawPoints;
            }

            this.db.Teams.Update(homeTeam);
            this.db.Teams.Update(awayTeam);
            await this.db.SaveChangesAsync();
            await this.UpdateTeamsPositions(group);
        }
        
        public async Task<Team> GetTeamByName(string name)
        {
            return await this.db
                .Teams
                .FirstOrDefaultAsync(t => t.Name == name.Trim());
        }

        public async Task<Team> GetTeamById(int teamId)
        {
            return await this.db
                .Teams
                .Include(t => t.Group)
                .FirstOrDefaultAsync(t => t.Id == teamId);
        }

        public async Task<ICollection<Team>> GetAllTeams()
        {
            return this.db.Teams.ToList();
        }

        public async Task Delete(Team team)
        {
            this.db.Teams.Remove(team);
            await this.db.SaveChangesAsync();
        }

        private async Task UpdateTeamsPositions(Group group)
        {
            var orderedTeams = group
                .Teams
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.GoalsScored - t.GoalsReceived)
                .ThenByDescending(t => t.GoalsScored)
                .ThenByDescending(t => t.Wins)
                .ToList();

            for (int i = 0; i < group.Teams.Count; i++)
            {
                var team = orderedTeams[i];
                team.Position = i + 1;

                this.db.Teams.Update(team);
            }

            await this.db.SaveChangesAsync();
        }
    }
}
