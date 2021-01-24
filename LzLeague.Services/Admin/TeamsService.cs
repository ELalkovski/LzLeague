namespace LzLeague.Services.Admin
{
    using System;
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
                    .OrderBy(t => t.Position)
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

        public async Task UpdateTeam(Team team)
        {
            this.db.Teams.Update(team);
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
                .Split(":", StringSplitOptions.RemoveEmptyEntries)
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

        public async Task EditTeamsStatistics(Match match, string score, string result)
        {
            var homeTeam = await this.GetTeamByName(match.HomeTeam);
            var awayTeam = await this.GetTeamByName(match.AwayTeam);
            var newGoalsArgs = score
                .Split(":", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            var prevGoalsArgs = match.Result
                .Split(":", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            var homeGoalsDifference = prevGoalsArgs[0] - newGoalsArgs[0];
            var awayGoalsDifference = prevGoalsArgs[1] - newGoalsArgs[1];

            homeTeam.GoalsScored -= homeGoalsDifference;
            awayTeam.GoalsScored -= awayGoalsDifference;
            homeTeam.GoalsReceived -= awayGoalsDifference;
            awayTeam.GoalsReceived -= homeGoalsDifference;

            if (match.WinnerSign != result)
            {
                if (match.WinnerSign == "1" && result == "x")
                {
                    homeTeam.Wins--;
                    homeTeam.Draws++;
                    homeTeam.Points -= WinPoints;
                    homeTeam.Points += DrawPoints;
                    awayTeam.Loses--;
                    awayTeam.Draws++;
                    awayTeam.Points += DrawPoints;
                }
                else if (match.WinnerSign == "2" && result == "x")
                {
                    awayTeam.Wins--;
                    awayTeam.Draws++;
                    awayTeam.Points -= WinPoints;
                    awayTeam.Points += DrawPoints;
                    homeTeam.Loses--;
                    homeTeam.Draws++;
                    homeTeam.Points += DrawPoints;
                }
                else if (match.WinnerSign == "1" && result == "2")
                {
                    homeTeam.Wins--;
                    homeTeam.Loses++;
                    homeTeam.Points -= WinPoints;
                    awayTeam.Wins++;
                    awayTeam.Loses--;
                    awayTeam.Points += WinPoints;
                }
                else if (match.WinnerSign == "2" && result == "1")
                {
                    awayTeam.Wins--;
                    awayTeam.Loses++;
                    awayTeam.Points -= WinPoints;
                    homeTeam.Wins++;
                    homeTeam.Loses--;
                    homeTeam.Points += WinPoints;
                }
                else if (match.WinnerSign == "x" && result == "1")
                {
                    homeTeam.Draws--;
                    homeTeam.Wins++;
                    homeTeam.Points -= DrawPoints;
                    homeTeam.Points += WinPoints;
                    awayTeam.Draws--;
                    awayTeam.Loses++;
                    awayTeam.Points -= DrawPoints;
                }
                else if (match.WinnerSign == "x" && result == "2")
                {
                    awayTeam.Draws--;
                    awayTeam.Wins++;
                    awayTeam.Points -= DrawPoints;
                    awayTeam.Points += WinPoints;
                    homeTeam.Draws--;
                    homeTeam.Loses++;
                    homeTeam.Points -= DrawPoints;
                }
            }

            this.db.Teams.Update(homeTeam);
            this.db.Teams.Update(awayTeam);
            await this.db.SaveChangesAsync();
            await this.UpdateTeamsPositions(match.Group);
        }


        public async Task<Team> GetTeamByName(string name)
        {
            return await this.db
                .Teams
                .Include(t => t.PlayedMatches)
                .FirstOrDefaultAsync(t => t.Name == name.Trim());
        }

        public async Task<Team> GetTeamById(int teamId)
        {
            return await this.db
                .Teams
                .AsNoTracking()
                .Include(t => t.Group)
                .ThenInclude(g => g.Teams)
                .AsNoTracking()
                .Include(t => t.PlayedMatches)
                .AsNoTracking()
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

        public async Task AddPoints(Team team, string statType)
        {
            switch (statType)
            {
                case "win":
                    team.Wins++;
                    team.Points += WinPoints;
                    break;
                case "draw":
                    team.Draws++;
                    team.Points += DrawPoints;
                    break;
                case "loss":
                    team.Loses++;
                    break;
            }

            this.db.Teams.Update(team);
            await this.db.SaveChangesAsync();
            await this.UpdateTeamsPositions(team.Group);
        }

        public async Task RemovePoints(Team team, string statType)
        {
            switch (statType)
            {
                case "win":
                    team.Wins--;
                    team.Points -= WinPoints;
                    break;
                case "draw":
                    team.Draws--;
                    team.Points -= DrawPoints;
                    break;
                case "loss":
                    team.Loses--;
                    break;
            }

            this.db.Teams.Update(team);
            await this.db.SaveChangesAsync();
            await this.UpdateTeamsPositions(team.Group);
        }

        public async Task Update(Team team)
        {
            this.db.Teams.Update(team);
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
