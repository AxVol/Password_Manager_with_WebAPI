using WebApi.Domain.Entity;
using WebApi.Domain.Response;
using WebApi.Domain.ViewModels.Password;

namespace WebApi.Service.Interfaces
{
    public interface IPasswordService
    {
        Task<IResponse<Password>> GetAll(PasswordViewModel model);
        Task<IResponse<Password>> Create(PasswordViewModel model);
        Task<IResponse<Password>> Update(PasswordViewModel model);
        Task<IResponse<Password>> Delete(PasswordViewModel model);
    }
}
