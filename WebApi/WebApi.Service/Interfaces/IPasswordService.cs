using WebApi.Domain.Response;
using WebApi.Domain.Response.Password;
using WebApi.Domain.ViewModels.Password;

namespace WebApi.Service.Interfaces
{
    public interface IPasswordService
    {
        Task<IResponse<PasswordResponse>> GetAll(PasswordViewModel model);
        Task<IResponse<PasswordResponse>> Create(PasswordViewModel model);
        Task<IResponse<PasswordResponse>> Update(PasswordViewModel model);
        Task<IResponse<PasswordResponse>> Delete(PasswordViewModel model);
    }
}
