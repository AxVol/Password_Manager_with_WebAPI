using WebApi.Domain.Response;
using WebApi.Domain.Response.Password;
using WebApi.Domain.ViewModels.Password;

namespace WebApi.Service.Interfaces
{
    public interface IPasswordService
    {
        Task<IResponse<PasswordResponse>> GetAll(string token);
        Task<IResponse<PasswordResponse>> Create(PasswordViewModel model, string token);
        Task<IResponse<PasswordResponse>> Update(PasswordViewModel model, string token);
        Task<IResponse<PasswordResponse>> Delete(PasswordViewModel model, string token);
    }
}
