namespace LzLeague.Services.Admin
{
    using System.Collections.Generic;
    using System.Linq;
    using LzLeague.Data;
    using LzLeague.Models;
    using LzLeague.Services.Admin.Contracts;
    using Microsoft.EntityFrameworkCore;

    public class UsersService : IUsersService
    {
        private readonly LzLeagueContext db;

        public UsersService(LzLeagueContext db)
        {
            this.db = db;
        }

        public ICollection<ApplicationUser> GetUsers()
        {
            return this.db
                .Users
                .Include(u => u.Prediction)
                .ToList();
        }
    }
}
