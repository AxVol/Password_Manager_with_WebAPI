using WebApi.Domain.Entity;
using WebApi.Domain.Response;
using WebApi.Domain.ViewModels.User;

namespace WebApi.Service
{
    public interface IUserService
    {
        Task<IResponse<User>> Register(RegisterViewModel model);
        Task<IResponse<User>> Login(LoginViewModel model);
        Task BlockAccount(long id);
    }
}
