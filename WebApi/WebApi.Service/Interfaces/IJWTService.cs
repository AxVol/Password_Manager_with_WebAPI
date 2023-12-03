using WebApi.Domain.Entity;

namespace WebApi.Service.Interfaces
{
    public interface IJWTService
    {
        public Task<string> GetToken(User user);
    }
}
