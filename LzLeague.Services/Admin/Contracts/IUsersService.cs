namespace LzLeague.Services.Admin.Contracts
{
    using System.Collections.Generic;
    using LzLeague.Models;

    public interface IUsersService
    {
        ICollection<ApplicationUser> GetUsers();
    }
}
