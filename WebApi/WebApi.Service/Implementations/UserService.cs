using Microsoft.Extensions.Logging;
using System.Text;
using WebApi.DAL.Interfaces;
using WebApi.Domain.Entity;
using WebApi.Domain.Response;
using WebApi.Domain.ViewModels.Password;
using WebApi.Domain.ViewModels.User;
using WebApi.Service.Interfaces;

namespace WebApi.Service
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> userRepository;
        private readonly IRepository<BlockedUser> blockedRepository;
        private readonly ILogger<UserService> logger;
        private readonly ICryptography cryptography;

        public UserService(IRepository<User> rep, ILogger<UserService> log, 
            ICryptography crypt, IRepository<BlockedUser> block)
        {
            userRepository = rep;
            logger = log;
            cryptography = crypt;
            blockedRepository = block;
        }

        public async Task<IResponse<User>> Login(LoginViewModel model)
        {
            return await Task.Run(() =>
            {
                IResponse<User> failedResponse = new Response<User>()
                {
                    Description = "Неверен логин или пароль",
                    Status = Domain.Enum.RequestStatus.Failed,
                    Value = null
                };

                User? user = userRepository.GetAll().FirstOrDefault(
                    u => u.Email == model.Login || u.Login == model.Login);

                if (user == null)
                {
                    return failedResponse;
                }

                if (IsBanned(user))
                {
                    failedResponse.Description = "Вы были заблокированы";

                    return failedResponse;
                }

                if (!IsCorrectPassword(model.Password, user))
                {
                    failedResponse.Description = "Неверный пароль";
                    failedResponse.Value = user;

                    return failedResponse;
                }

                return new Response<User>()
                {
                    Description = "Успех",
                    Status = Domain.Enum.RequestStatus.Success,
                    Value = user
                };
            });
        }

        public async Task<IResponse<User>> Register(RegisterViewModel model)
        {
            try
            {
                if (UserExists(model))
                {
                    return new Response<User>()
                    {
                        Description = "Такой пользователь уже существует",
                        Status = Domain.Enum.RequestStatus.Exists,
                        Value = null
                    };
                }

                string hashPassword = cryptography.GetPasswordHash(model.Password, model.Login);
                User user = new User()
                {
                    Login = model.Login,
                    Email = model.Email,
                    Password = hashPassword
                };

                await userRepository.Create(user);

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

        public async Task BlockAccount(long id)
        {
            User? user = userRepository.GetAll().FirstOrDefault(u => u.Id == id);

            if (user is null)
            {
                logger.LogError($"Error user to block is null");
                return;
            }

            BlockedUser blockedUser = new BlockedUser()
            {
                User = user,
                UnbanDate = DateTime.Now.AddMinutes(30),
            };

            await blockedRepository.Create(blockedUser);
        }

        private bool UserExists(RegisterViewModel model)
        {
            User? user = userRepository.GetAll().FirstOrDefault(
                u => u.Email == model.Email || u.Login == model.Login);

            if (user == null)
                return false;
            else
                return true;
        }

        private bool IsBanned(User user)
        {
            BlockedUser? blockedUser = blockedRepository.GetAll().FirstOrDefault(u => u.User.Id == user.Id);

            if (blockedUser is null)
            {
                return false;
            }

            return true;
        }

        private bool IsCorrectPassword(string password, User user)
        {
            string passwordHash = cryptography.GetPasswordHash(password, user.Login);

            if (passwordHash != user.Password)
            {
                return false;
            }

            return true;
        }
    }
}
