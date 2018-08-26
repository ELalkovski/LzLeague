namespace LzLeague.Tests.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using LzLeague.Data;
    using LzLeague.Models;
    using LzLeague.Services.Admin;
    using LzLeague.Tests.Mocks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MatchesServiceTests
    {
        private LzLeagueContext dbContext;

        [TestMethod]
        public async Task AddSingleMatch_Test()
        {
            // Arrange
            var matchesService = new MatchService(this.dbContext);

            // Act 
            await matchesService.CreateMatch(new Match
            {
                HomeTeam = "Manchester United",
                AwayTeam = "CSKA Moscow",
                BeginTime = new TimeSpan(DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, DateTime.UtcNow.Second),
                Date = DateTime.UtcNow
            });

            // Assert
            Assert.AreEqual(1, this.dbContext.Matches.Count());
        }

        [TestMethod]
        public async Task AddMultipleMatches_Test()
        {
            // Arrange
            var matchesService = new MatchService(this.dbContext);

            // Act 
            await matchesService.CreateMatch(new Match
            {
                HomeTeam = "Manchester United",
                AwayTeam = "CSKA Moscow",
                BeginTime = new TimeSpan(DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, DateTime.UtcNow.Second),
                Date = DateTime.UtcNow
            });
            await matchesService.CreateMatch(new Match
            {
                HomeTeam = "Basel",
                AwayTeam = "CSKA Moscow",
                BeginTime = new TimeSpan(DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, DateTime.UtcNow.Second),
                Date = DateTime.UtcNow
            });
            await matchesService.CreateMatch(new Match
            {
                HomeTeam = "Manchester United",
                AwayTeam = "Basel",
                BeginTime = new TimeSpan(DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, DateTime.UtcNow.Second),
                Date = DateTime.UtcNow
            });

            // Assert
            Assert.AreEqual(3, this.dbContext.Matches.Count());
        }

        [TestMethod]
        public async Task GetSingleMatchById_Test()
        {
            // Arrange
            var matchesService = new MatchService(this.dbContext);

            this.dbContext.Matches.Add(new Match
            {
                HomeTeam = "Manchester United",
                AwayTeam = "CSKA Moscow",
                BeginTime = new TimeSpan(DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, DateTime.UtcNow.Second),
                Date = DateTime.UtcNow
            });
            await this.dbContext.SaveChangesAsync();

            // Act 
            var match = await matchesService.GetMatch(1);

            // Assert
            Assert.AreEqual("Manchester United", match.HomeTeam);
        }

        [TestMethod]
        public async Task GetAllMatches_Test()
        {
            // Arrange
            var matchesService = new MatchService(this.dbContext);

            this.dbContext.Matches.Add(new Match
            {
                HomeTeam = "Manchester United",
                AwayTeam = "CSKA Moscow",
                BeginTime = new TimeSpan(DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, DateTime.UtcNow.Second),
                Date = DateTime.UtcNow
            });
            this.dbContext.Matches.Add(new Match
            {
                HomeTeam = "Basel",
                AwayTeam = "CSKA Moscow",
                BeginTime = new TimeSpan(DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, DateTime.UtcNow.Second),
                Date = DateTime.UtcNow
            });
            this.dbContext.Matches.Add(new Match
            {
                HomeTeam = "Manchester United",
                AwayTeam = "Basel",
                BeginTime = new TimeSpan(DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, DateTime.UtcNow.Second),
                Date = DateTime.UtcNow
            });
            await this.dbContext.SaveChangesAsync();

            // Act 
            var matches = matchesService.GetAllMatches();

            // Assert
            Assert.AreEqual(3, matches.Count);
        }

        [TestMethod]
        public async Task DeleteMatch_Test()
        {
            // Arrange
            var matchesService = new MatchService(this.dbContext);

            this.dbContext.Matches.Add(new Match
            {
                HomeTeam = "Manchester United",
                AwayTeam = "CSKA Moscow",
                BeginTime = new TimeSpan(DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, DateTime.UtcNow.Second),
                Date = DateTime.UtcNow
            });

            await this.dbContext.SaveChangesAsync();
            var match = this.dbContext.Matches.FirstOrDefault(m => m.HomeTeam == "Manchester United" && m.AwayTeam == "CSKA Moscow");

            // Act 
            await matchesService.Delete(match);

            // Assert
            Assert.AreEqual(false, this.dbContext.Matches.Any(m => m.HomeTeam == "Manchester United" && m.AwayTeam == "CSKA Moscow"));
        }

        [TestInitialize]
        public void InitializeDbContext()
        {
            this.dbContext = MockDbContext.GetContext();
        }
    }
}
