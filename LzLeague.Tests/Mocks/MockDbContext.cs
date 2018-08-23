namespace LzLeague.Tests.Mocks
{
    using System;
    using LzLeague.Data;
    using Microsoft.EntityFrameworkCore;

    public static class MockDbContext
    {
        public static LzLeagueContext GetContext()
        {
            var options = new DbContextOptionsBuilder<LzLeagueContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new LzLeagueContext(options);
        }
    }
}
