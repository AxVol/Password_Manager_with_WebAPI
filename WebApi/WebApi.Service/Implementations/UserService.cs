using Microsoft.Extensions.Logging;
using System.Text;
using WebApi.DAL.Interfaces;
using WebApi.Domain.Entity;
using WebApi.Domain.Response;
using WebApi.Domain.ViewModels.User;
using WebApi.Service.Implementations;
using WebApi.Service.Interfaces;

namespace WebApi.Service
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> repository;
        private readonly ILogger<UserService> logger;
        private readonly ICryptography cryptography;

        public UserService(IRepository<User> rep, ILogger<UserService> log)
        {
            repository = rep;
            logger = log;
            cryptography = new Cryptography();
        }

        public async Task<IResponse<User>> Login(LoginViewModel model)
        {
            IResponse<User> response = new Response<User>()
            {
                Description = "Неверен логин или пароль",
                Status = Domain.Enum.RequestStatus.Failed,
                Value = null
            };

            string passwordHash = cryptography.GetPasswordHash(model.Password);
            IEnumerable<User> users = repository.GetAll();
            User? user = users.FirstOrDefault(u => u.Email == model.Login || u.Login == model.Login);

            if (user == null)
            {
                return response;
            }
            else
            {
                if (passwordHash != user.Password)
                    return response;
            }

            return new Response<User>()
            {
                Description = "Успех",
                Status = Domain.Enum.RequestStatus.Success,
                Value = user
            };
        }

        public async Task<IResponse<User>> Register(RegisterViewModel model)
        {
            try
            {
                string hashPassword = cryptography.GetPasswordHash(model.Password);
                IEnumerable<User> users = repository.GetAll();
                User? user = users.FirstOrDefault(u => u.Email == model.Email || u.Login == model.Login);

                if (user != null)
                {
                    return new Response<User>()
                    {
                        Description = "Такой пользователь уже существует",
                        Status = Domain.Enum.RequestStatus.Exists,
                        Value = user
                    };
                }

                user = new User()
                {
                    Login = model.Login,
                    Email = model.Email,
                    Password = hashPassword,
                    SecretToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()))
                };

                await repository.Create(user);

                return new Response<User>()
                {
                    Description = "Пользователь успешно создан",
                    Status = Domain.Enum.RequestStatus.Success,
                    Value = user
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                return new Response<User>()
                {
                    Description = "Непредвиденная ошибка",
                    Status = Domain.Enum.RequestStatus.Failed
                };
            }
        }

        public Task<IResponse<string>> UpdateToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
