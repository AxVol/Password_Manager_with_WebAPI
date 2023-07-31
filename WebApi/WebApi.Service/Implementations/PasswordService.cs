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

                return new Response<Password>()
                {
                    Description = "Пароль успешно добавлен",
                    Status = Domain.Enum.RequestStatus.Success,
                    Value = password
                };
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

        public async Task<IResponse<Password>> Delete(PasswordViewModel model)
        {
            Password? password = passwordRepository.GetAll().
                FirstOrDefault(p => p.Id == model.Id && p.User.SecretToken == model.SecretToken);

            if (password == null)
            {
                return new Response<Password>()
                {
                    Description = "Попытка удалить несуществующий пароль",
                    Status = Domain.Enum.RequestStatus.Failed
                };
            }

            await passwordRepository.Delete(password);

            return new Response<Password>()
            {
                Description = "Пароль успешно удален",
                Status = Domain.Enum.RequestStatus.Success
            };
        }

        public async Task<IResponse<Password>> GetAll(PasswordViewModel model)
        {
            try
            {
                IEnumerable<Password> passwords = passwordRepository.GetAll().
                    Where(p => p.User.SecretToken == model.SecretToken);

                if (!passwords.Any())
                {
                    return new Response<Password>()
                    {
                        Description = "Пользователь не найден или у него нету паролей",
                        Status = Domain.Enum.RequestStatus.Failed
                    };
                }

                foreach (Password password in passwords)
                {
                    //string pass = cryptography.DecryptPassword(password.PassWord);
                    password.PassWord = "123";
                }

                return new Response<Password>()
                {
                    Status = Domain.Enum.RequestStatus.Success,
                    Values = passwords
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                return new Response<Password>()
                {
                    Description = "Не предвиденная ошибка",
                    Status = Domain.Enum.RequestStatus.Failed
                };
            }
        }

        public async Task<IResponse<Password>> Update(PasswordViewModel model)
        {
            Password? password = passwordRepository.GetAll().
                FirstOrDefault(p => p.Id == model.Id && p.User.SecretToken == model.SecretToken);

            if (password == null)
            {
                return new Response<Password>()
                {
                    Description = "Попытка изменить несуществующий пароль",
                    Status = Domain.Enum.RequestStatus.Failed
                };
            }

            string cypherPass = cryptography.EncryptPassword(model.Password);
            password.PassWord = cypherPass;
            password.PassService = model.Service;

            await passwordRepository.Update(password);

            password.PassWord = model.Password;

            return new Response<Password>()
            {
                Description = "Пароль успешно изменен",
                Status = Domain.Enum.RequestStatus.Success,
                Value = password
            };
        }
    }
}
