using Microsoft.Extensions.Logging;
using WebApi.DAL.Interfaces;
using WebApi.Domain.Entity;
using WebApi.Domain.Response;
using WebApi.Domain.ViewModels.Password;
using WebApi.Service.Interfaces;

namespace WebApi.Service.Implementations
{
    public class PasswordService : IPasswordService
    {
        private readonly IRepository<Password> passwordRepository;
        private readonly IRepository<User> userRepository;
        private readonly ILogger<PasswordService> logger;
        private readonly ICryptography cryptography;
        
        public PasswordService(IRepository<Password> rep, IRepository<User> userRep, ILogger<PasswordService> log,
            ICryptography crypt) 
        {
            passwordRepository = rep;
            userRepository = userRep;
            logger = log;
            cryptography = crypt;
        }

        public async Task<IResponse<Password>> Create(PasswordViewModel model)
        {
            try
            {
                User user = userRepository.GetAll().First(u => u.SecretToken == model.SecretToken);
                string cypherPass = cryptography.EncryptPassword(model.Password);

                Password password = new Password()
                {
                    User = user,
                    PassWord = cypherPass,
                    PassService = model.Service
                };

                await passwordRepository.Create(password);

                password.PassWord = model.Password;

                IResponse<Password> response = new Response<Password>()
                {
                    Description = "Пароль успешно добавлен",
                    Status = Domain.Enum.RequestStatus.Success,
                    Value = password
                };

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                return new Response<Password>()
                {
                    Description = "Непредвиденная ошибка",
                    Status = Domain.Enum.RequestStatus.Failed
                };
            }
        }

        public Task<IResponse<Password>> Delete(PasswordViewModel model)
        {
            Password password = passwordRepository.

            passwordRepository.Delete(password);
        }

        public Task<IResponse<Password>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<Password>> Update(PasswordViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
