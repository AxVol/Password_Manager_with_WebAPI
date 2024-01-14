using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApi.DAL.Interfaces;
using WebApi.Domain.Entity;
using WebApi.Domain.Response;
using WebApi.Domain.Response.Password;
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

        public async Task<IResponse<PasswordResponse>> Create(PasswordViewModel model, string token)
        {
            try
            {
                JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                List<Claim> claims = new List<Claim>(jwt.Claims);
                string userLogin = claims[0].Value;
                User user = userRepository.GetAll().First(u => u.Login == userLogin);
                string cypherPass = cryptography.EncryptPassword(model.Password, user.Id);

                Password password = new Password()
                {
                    User = user,
                    LoginService = model.Login,
                    PassWord = cypherPass,
                    PassService = model.Service
                };
                PasswordResponse passwordResponse = new PasswordResponse()
                {
                    Id = model.Id
                };

                await passwordRepository.Create(password);

                return new Response<PasswordResponse>()
                {
                    Description = "Пароль успешно добавлен",
                    Status = Domain.Enum.RequestStatus.Success,
                    Value = passwordResponse
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                return new Response<PasswordResponse>()
                {
                    Description = "Непредвиденная ошибка",
                    Status = Domain.Enum.RequestStatus.Failed
                };
            }
        }

        public async Task<IResponse<PasswordResponse>> Delete(PasswordViewModel model, string token)
        {
            JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            List<Claim> claims = new List<Claim>(jwt.Claims);
            string userLogin = claims[0].Value;
            Password? password = passwordRepository.GetAll()
                .FirstOrDefault(p => p.Id == model.Id && p.User.Login == userLogin);

            if (password == null)
            {
                return new Response<PasswordResponse>()
                {
                    Description = "Попытка удалить несуществующий пароль",
                    Status = Domain.Enum.RequestStatus.Failed
                };
            }

            await passwordRepository.Delete(password);

            return new Response<PasswordResponse>()
            {
                Description = "Пароль успешно удален",
                Status = Domain.Enum.RequestStatus.Success
            };
        }

        public async Task<IResponse<PasswordResponse>> GetAll(string token)
        {
            return await Task.Run(() =>
            {
                try
                {
                    JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                    List<Claim> claims = new List<Claim>(jwt.Claims);
                    string userLogin = claims[0].Value;

                    IEnumerable<Password> passwords = passwordRepository.GetAll().
                        Where(p => p.User.Login == userLogin);
                    List<PasswordResponse> responsePasswords = new List<PasswordResponse>();

                    if (!passwords.Any())
                    {
                        return new Response<PasswordResponse>()
                        {
                            Description = "Пользователь не найден или у него нету паролей",
                            Status = Domain.Enum.RequestStatus.Failed
                        };
                    }

                    foreach (Password password in passwords)
                    {
                        string pass = cryptography.DecryptPassword(password.PassWord, password.User.Id);
                        password.PassWord = pass;

                        PasswordResponse responseModel = new PasswordResponse()
                        {
                            Id = password.Id,
                            Login = password.LoginService,
                            Password = password.PassWord,
                            Service = password.PassService,
                        };
                        responsePasswords.Add(responseModel);
                    }

                    return new Response<PasswordResponse>()
                    {
                        Status = Domain.Enum.RequestStatus.Success,
                        Values = responsePasswords
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);

                    return new Response<PasswordResponse>()
                    {
                        Description = "Не предвиденная ошибка",
                        Status = Domain.Enum.RequestStatus.Failed
                    };
                }
            });
        }

        public async Task<IResponse<PasswordResponse>> Update(PasswordViewModel model, string token)
        {
            JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            List<Claim> claims = new List<Claim>(jwt.Claims);
            string userLogin = claims[0].Value;
            Password? password = passwordRepository.GetAll().
                FirstOrDefault(p => p.Id == model.Id && p.User.Login == userLogin);

            if (password == null)
            {
                return new Response<PasswordResponse>()
                {
                    Description = "Попытка изменить несуществующий пароль",
                    Status = Domain.Enum.RequestStatus.Failed
                };
            }

            string cypherPass = cryptography.EncryptPassword(model.Password, password.User.Id);
            password.LoginService = model.Login;
            password.PassWord = cypherPass;
            password.PassService = model.Service;

            await passwordRepository.Update(password);

            PasswordResponse passwordResponse = new PasswordResponse()
            {
                Id = model.Id,
                Login = model.Login,
                Password = model.Password,
                Service = model.Service
            };

            return new Response<PasswordResponse>()
            {
                Description = "Пароль успешно изменен",
                Status = Domain.Enum.RequestStatus.Success,
                Value = passwordResponse
            };
        }
    }
}
