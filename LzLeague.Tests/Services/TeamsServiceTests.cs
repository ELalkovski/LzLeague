namespace LzLeague.Tests.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using LzLeague.Data;
    using LzLeague.Models;
    using LzLeague.Services.Admin;
    using LzLeague.Tests.Mocks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TeamsServiceTests
    {
        private LzLeagueContext dbContext;

        [TestInitialize]
        public void InitializeDbContext()
        {
            this.dbContext = MockDbContext.GetContext();
        }

        [TestMethod]
        public async Task AddGroup_Test()
        {
            // Arrange
            var teamsService = new TeamsService(this.dbContext);

            // Act 
            await teamsService.CreateGroup(new Group { Name = "A" });
            var groups = this.dbContext.Groups;

            // Assert
            Assert.AreEqual(1, groups.Count());
        }

        [TestMethod]
        public async Task AddTeam_Test()
        {
            // Arrange
            var teamsService = new TeamsService(this.dbContext);
            this.dbContext.Groups.Add(new Group { Name = "E" });
            await this.dbContext.SaveChangesAsync();

            // Act 
            await teamsService.CreateTeam( new Team
            {
                Name = "Manchester United",
                GroupId = 1,
                ImageUrl = "test.jpg"
            });

            var teams = this.dbContext.Teams;

            // Assert
            Assert.AreEqual(1, teams.Count());
        }

        [TestMethod]
        public async Task GetGroupByName_Test()
        {
            // Arrange
            var teamsService = new TeamsService(this.dbContext);

            this.dbContext.Groups.Add(new Group { Name = "B" });
            this.dbContext.Groups.Add(new Group { Name = "C" });
            await this.dbContext.SaveChangesAsync();

            // Act
            var group = teamsService.GetGroupByName("B");

            // Assert
            Assert.AreNotEqual(null, group);
        }

        [TestMethod]
        public async Task GetGroupById_Test()
        {
            // Arrange
            var teamsService = new TeamsService(this.dbContext);

            this.dbContext.Groups.Add(new Group { Name = "D" });
            await this.dbContext.SaveChangesAsync();

            // Act
            var group = teamsService.GetGroupById(1);

            // Assert
            Assert.AreNotEqual(null, group);
        }
    }
}
