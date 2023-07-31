using Microsoft.Extensions.Logging;
using Moq;
using WebApi.DAL.Interfaces;
using WebApi.Domain.Entity;

namespace WebApi.Tests.Mock
{
    public static class MockFactory<T>
    {
        public static ILogger<T> GetLogger()
        {
            var mock = new Mock<ILogger<T>>();

            return mock.Object;
        }

        public static IRepository<User> GetUserRepository()
        {
            IEnumerable<User> users = CreateUserList();
            var mock = new Mock<IRepository<User>>();

            mock.Setup(m => m.Create(It.IsAny<User>())).Returns(Task.CompletedTask);
            mock.Setup(m => m.Delete(It.IsAny<User>())).Returns(Task.CompletedTask);
            mock.Setup(m => m.Update(It.IsAny<User>())).Returns(Task.CompletedTask);
            mock.Setup(m => m.GetAll()).Returns(users);

            return mock.Object;
        }

        public static IRepository<Password> GetPasswordRepository()
        {
            IEnumerable<Password> passwords = CreatePasswordList();
            var mock = new Mock<IRepository<Password>>();

            mock.Setup(m => m.Create(It.IsAny<Password>())).Returns(Task.CompletedTask);
            mock.Setup(m => m.Delete(It.IsAny<Password>())).Returns(Task.CompletedTask);
            mock.Setup(m => m.Update(It.IsAny<Password>())).Returns(Task.CompletedTask);
            mock.Setup(m => m.GetAll()).Returns(passwords);

            return mock.Object;
        }

        private static IEnumerable<User> CreateUserList()
        {
            List<User> users = new List<User>()
            {
                new User()
                {
                    Id = 1,
                    Login = "login1",
                    Email = "login1@mail.ru",
                    Password = "B5MjpJwwDc2N/+FGDe3gLA==", //login1
                    SecretToken = "ZGUwNTAyZDItZmFjZC00N2M3LWFkZTQtOTYyMWQ1NTFlMzk4"
                },
                new User()
                {
                    Id = 2,
                    Login = "login2",
                    Email = "login2@mail.ru",
                    Password = "OzjCI80HZ8Xm9Ap/uGFZtA==", //login2
                    SecretToken = "NGJkM2JlMDQtMzQ4MC00YWJlLTg0ZjgtZjhjOTRkNDgzMzQ5"
                },
                new User()
                {
                    Id = 3,
                    Login = "login3",
                    Email = "login3@mail.ru",
                    Password = "qgi+KxWmK/H7PbiAIqd/kg==", //login3
                    SecretToken = "ZDI0NmNiMDAtZTdhMi00ODYwLWI4YWQtYTc0YjU0NzIyMzQ4"
                }
            };

            return users;
        }

        private static IEnumerable<Password> CreatePasswordList()
        {
            List<Password> passwords = new List<Password>()
            {
                new Password()
                {
                    Id = 1,
                    User = new User()
                    {
                        Id = 1,
                        Login = "login1",
                        Email = "login1@mail.ru",
                        Password = "B5MjpJwwDc2N/+FGDe3gLA==", //login1
                        SecretToken = "ZGUwNTAyZDItZmFjZC00N2M3LWFkZTQtOTYyMWQ1NTFlMzk4"
                    },
                    PassWord = "eKBXlrliDRm1fmhAF8TlOw==", //123
                    PassService = "faceebook"
                },
                new Password()
                {
                    Id = 2,
                    User = new User()
                    {
                        Id = 2,
                        Login = "login2",
                        Email = "login2@mail.ru",
                        Password = "OzjCI80HZ8Xm9Ap/uGFZtA==", //login2
                        SecretToken = "NGJkM2JlMDQtMzQ4MC00YWJlLTg0ZjgtZjhjOTRkNDgzMzQ5"
                    },
                    PassWord = "vrcR5dYkzSJIgvSW6W/3JQ==", //crack
                    PassService = "faceebook"
                },
                new Password()
                {
                    Id = 3,
                    User = new User()
                    {
                        Id = 2,
                        Login = "login2",
                        Email = "login2@mail.ru",
                        Password = "OzjCI80HZ8Xm9Ap/uGFZtA==", //login2
                        SecretToken = "NGJkM2JlMDQtMzQ4MC00YWJlLTg0ZjgtZjhjOTRkNDgzMzQ5"
                    },
                    PassWord = "On2uylG9BrQrh077uVZP6g==", //best
                    PassService = "youtube"
                },
            };

            return passwords;
        }
    }
}
