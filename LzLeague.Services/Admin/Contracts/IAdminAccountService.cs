namespace LzLeague.Services.Admin.Contracts
{
    using System.Threading.Tasks;
    using Common.AdminBindingModels;

    public interface IAdminAccountService
    {
        Task<bool> SignIn(LoginBindingModel model);
    }
}
