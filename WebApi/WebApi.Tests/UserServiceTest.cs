using Microsoft.Extensions.Logging;
using WebApi.DAL.Interfaces;
using WebApi.Domain.Entity;
using WebApi.Domain.ViewModels.User;
using WebApi.Service;
using WebApi.Service.Implementations;
using WebApi.Service.Interfaces;
using WebApi.Tests.Mock;
using Xunit;

namespace WebApi.Tests
{
    public class UserServiceTest
    {
        private readonly IUserService userService;

        public UserServiceTest()
        {
            ICryptography cryptography = new Cryptography();
            ILogger<UserService> logger = MockFactory<UserService>.GetLogger();
            IRepository<User> userRepository = MockFactory<UserService>.GetUserRepository();
            IRepository<BlockedUser> blockedUserRepository = MockFactory<UserService>.GetBlockedUserRepository();

            userService = new UserService(userRepository, logger, cryptography, blockedUserRepository);
        }

        [Fact]
        public async void Blocked_user_login_test()
        {
            // Arrage
            LoginViewModel viewModel = new LoginViewModel()
            {
                Login = "login3",
                Password = "login3"
            };

            // Act
            var response = await userService.Login(viewModel);

            // Assert
            Assert.Equal(Domain.Enum.RequestStatus.Failed, response.Status);
            Assert.Equal("Вы были заблокированы", response.Description);
        }

        [Fact]
        public async void Login_test()
        {
            // Arrage
            LoginViewModel viewModel = new LoginViewModel()
            {
                Login = "login1",
                Password = "login1"
            };

            // Act
            var response = await userService.Login(viewModel);

            // Assert
            Assert.Equal(Domain.Enum.RequestStatus.Success, response.Status);
        }

        [Fact]
        public async void Login_for_email_test()
        {
            // Arrage
            LoginViewModel viewModel = new LoginViewModel()
            {
                Login = "login1@mail.ru",
                Password = "login1"
            };

            // Act
            var response = await userService.Login(viewModel);

            // Assert
            Assert.Equal(Domain.Enum.RequestStatus.Success, response.Status);
        }

        [Fact]
        public async void Wrong_data_to_login_test()
        {
            // Arrage
            LoginViewModel wrongLogin = new LoginViewModel()
            {
                Login = "dasda",
                Password = "login1",
            };

            LoginViewModel wrongPassword = new LoginViewModel()
            {
                Login = "login1@mail.ru",
                Password = "test"
            };

            // Act
            var responseLogin = await userService.Login(wrongLogin);
            var responsePassword = await userService.Login(wrongPassword);

            // Assert
            Assert.Equal(Domain.Enum.RequestStatus.Failed, responseLogin.Status);
            Assert.Equal(Domain.Enum.RequestStatus.Failed, responsePassword.Status);
        }

        [Fact]
        public async void Not_exists_user_login_test()
        {
            // Arrage
            LoginViewModel viewModel = new LoginViewModel()
            {
                Login = "sadad",
                Password = "adsasd"
            };

            // Act
            var response = await userService.Login(viewModel);

            // Assert
            Assert.Equal(Domain.Enum.RequestStatus.Failed, response.Status);
        }

        [Fact]
        public async void Register_user_test()
        {
            // Arrage
            RegisterViewModel viewModel = new RegisterViewModel()
            {
                Email = "test@mail.ru",
                Login = "test",
                Password = "test",
            };

            // Act
            var response = await userService.Register(viewModel);

            // Assert
            Assert.Equal(Domain.Enum.RequestStatus.Success, response.Status);
        }

        [Fact]
        public async void Register_exists_user_login_test()
        {
            // Arrage
            RegisterViewModel viewModel = new RegisterViewModel()
            {
                Email = "test@mail.ru",
                Login = "login1",
                Password = "test",
            };

            // Act
            var response = await userService.Register(viewModel);

            // Assert
            Assert.Equal(Domain.Enum.RequestStatus.Exists, response.Status);
        }

        [Fact]
        public async void Register_exists_user_email_test()
        {
            // Arrage
            RegisterViewModel viewModel = new RegisterViewModel()
            {
                Email = "login1@mail.ru",
                Login = "test",
                Password = "test",
            };

            // Act
            var response = await userService.Register(viewModel);

            // Assert
            Assert.Equal(Domain.Enum.RequestStatus.Exists, response.Status);
        }

        [Fact]
        public async void Update_token_test()
        {
            // Arrage
            string token = "ZGUwNTAyZDItZmFjZC00N2M3LWFkZTQtOTYyMWQ1NTFlMzk4";

            // Act
            var response = await userService.UpdateToken(token);

            // Assert
            Assert.Equal(Domain.Enum.RequestStatus.Success, response.Status);
        }
    }
}
