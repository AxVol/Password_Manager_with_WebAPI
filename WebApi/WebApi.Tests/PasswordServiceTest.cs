using Microsoft.Extensions.Logging;
using WebApi.DAL.Interfaces;
using WebApi.Domain.Entity;
using WebApi.Domain.ViewModels.Password;
using WebApi.Service.Implementations;
using WebApi.Service.Interfaces;
using WebApi.Tests.Mock;
using Xunit;

namespace WebApi.Tests
{
    public class PasswordServiceTest
    {
        private readonly IPasswordService passwordService;

        public PasswordServiceTest()
        {
            ICryptography cryptography = new Cryptography();
            ILogger<PasswordService> logger = MockFactory<PasswordService>.GetLogger();
            IRepository<User> userRepository = MockFactory<PasswordService>.GetUserRepository();
            IRepository<Password> passwordRepository = MockFactory<PasswordService>.GetPasswordRepository();

            passwordService = new PasswordService(passwordRepository, userRepository, logger, cryptography);
        }

        [Fact]
        public async void Create_test()
        {
            // Arrage
            PasswordViewModel passwordViewModel = new PasswordViewModel()
            {
                Login = "login1",
                Password = "first",
                Service = "google",
            };

            // Act
            var response = await passwordService.Create(passwordViewModel);

            // Assert
            Assert.Equal(passwordViewModel.Password, response.Value.Password);
        }

        [Fact]
        public async void Create_with_not_exists_user_test()
        {
            // Arrage
            PasswordViewModel passwordViewModel = new PasswordViewModel()
            {
                Login = "login1",
                Password = "first",
                Service = "google",
            };

            // Act
            var response = await passwordService.Create(passwordViewModel);

            // Assert
            Assert.Equal(Domain.Enum.RequestStatus.Failed, response.Status);
        }

        [Fact]
        public async void Create_with_empty_parametr_test()
        {
            // Arrage
            PasswordViewModel passwordViewModel = new PasswordViewModel();

            // Act
            var response = await passwordService.Create(passwordViewModel);

            // Assert
            Assert.Equal(Domain.Enum.RequestStatus.Failed, response.Status);
        }

        [Fact]
        public async void GetAll_not_exists_user_test()
        {
            // Arrage
            string secretToken = "ZGUwNTAyZDItZmFjZC00N2SDADkZTDSDAQtODSAASTYyMWQ1NTFlMzk4";

            // Act
            var responseNotExists = await passwordService.GetAll(secretToken);

            // Assert
            Assert.Equal(Domain.Enum.RequestStatus.Failed, responseNotExists.Status);
        }

        [Fact]
        public async void GetAll_empty_Password_test()
        {
            // Arrage
            string secretToken = "ZDI0NmNiMDAtZTdhMi00ODYwLWI4YWQtYTc0YjU0NzIyMzQ4";

            // Act
            var responseEmptyPassword = await passwordService.GetAll(secretToken);

            // Assert
            Assert.Equal(Domain.Enum.RequestStatus.Failed, responseEmptyPassword.Status);
        }

        [Fact]
        public async void Update_test()
        {
            // Arrage
            PasswordViewModel passwordViewModel = new PasswordViewModel()
            {
                Id = 1,
                Login = "login1",
                Password = "first",
                Service = "google",
            };

            // Act
            var response = await passwordService.Update(passwordViewModel);

            // Assert
            Assert.Equal(Domain.Enum.RequestStatus.Success, response.Status);
        }

        [Fact]
        public async void Update_not_exists_password_test()
        {
            // Arrage
            PasswordViewModel passwordViewModel = new PasswordViewModel()
            {
                Id = 33,
                Login = "login1",
                Password = "first",
                Service = "google",
            };

            // Act
            var response = await passwordService.Update(passwordViewModel);

            // Assert
            Assert.Equal(Domain.Enum.RequestStatus.Failed, response.Status);
        }

        [Fact]
        public async void Update_not_owner_update_password()
        {
            // Arrage
            PasswordViewModel passwordViewModel = new PasswordViewModel()
            {
                Id = 1,
                Login = "login1",
                Password = "first",
                Service = "google",
            };

            // Act
            var response = await passwordService.Update(passwordViewModel);

            // Assert
            Assert.Equal(Domain.Enum.RequestStatus.Failed, response.Status);
        }

        [Fact]
        public async void Delete_test()
        {
            // Arrage
            PasswordViewModel passwordViewModel = new PasswordViewModel()
            {
                Id = 1,
            };

            // Act
            var response = await passwordService.Delete(passwordViewModel);

            // Assert
            Assert.Equal(Domain.Enum.RequestStatus.Success, response.Status);

        }

        [Fact]
        public async void Delete_not_exists_password()
        {
            // Arrage
            PasswordViewModel passwordViewModel = new PasswordViewModel()
            {
                Id = 33,
            };

            // Act
            var response = await passwordService.Delete(passwordViewModel);

            // Assert
            Assert.Equal(Domain.Enum.RequestStatus.Failed, response.Status);
        }

        [Fact]
        public async void Delete_not_owner_update_password()
        {
            // Arrage
            PasswordViewModel passwordViewModel = new PasswordViewModel()
            {
                Id = 1,
            };

            // Act
            var response = await passwordService.Delete(passwordViewModel);

            // Assert
            Assert.Equal(Domain.Enum.RequestStatus.Failed, response.Status);
        }
    }
}
