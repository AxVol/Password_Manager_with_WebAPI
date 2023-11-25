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

        public static IRepository<BlockedUser> GetBlockedUserRepository()
        {
            IEnumerable<BlockedUser> blockedUsers = CreateBlockedUserList();
            var mock = new Mock<IRepository<BlockedUser>>();

            mock.Setup(m => m.Create(It.IsAny<BlockedUser>())).Returns(Task.CompletedTask);
            mock.Setup(m => m.Delete(It.IsAny<BlockedUser>())).Returns(Task.CompletedTask);
            mock.Setup(m => m.GetAll()).Returns(blockedUsers);

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
                    Password = "DB4C19D4EAA91BA92392484C439CBC1365BE17C57397641ABBEE9E9AEB35E579BACA35B6514CC3B9016D5AE0C75AC259E374A7B4C52E409D718AF1AF91ED01CE", //login1
                    SecretToken = "ZGUwNTAyZDItZmFjZC00N2M3LWFkZTQtOTYyMWQ1NTFlMzk4"
                },
                new User()
                {
                    Id = 2,
                    Login = "login2",
                    Email = "login2@mail.ru",
                    Password = "F6B7985E8CDFBC2DDB597051437DD0E4421AD3386AA9629F961EDF127E2249681F6379CBFDCC7DD92031ED5A029B7CCF30720D1C0D88229D4DFC360E38C3777F", //login2
                    SecretToken = "NGJkM2JlMDQtMzQ4MC00YWJlLTg0ZjgtZjhjOTRkNDgzMzQ5"
                },
                new User()
                {
                    Id = 3,
                    Login = "login3",
                    Email = "login3@mail.ru",
                    Password = "8D463DBAB5A43E40D1D8213D01595413950917C46728AA9419E3FAC3DF1B74425BF03D2EEB7463A29A08EF8D77087834BA70F6ADEB6C3724DE8414D542C18CB1", //login3
                    SecretToken = "ZDI0NmNiMDAtZTdhMi00ODYwLWI4YWQtYTc0YjU0NzIyMzQ4"
                }
            };

            return users;
        }

        private static IEnumerable<BlockedUser> CreateBlockedUserList()
        {
            List<User> users = CreateUserList().ToList();
            List<BlockedUser> blockedUsers = new List<BlockedUser>()
            {
                new BlockedUser()
                {
                    Id = 1,
                    User = users[2],
                    UnbanDate = DateTime.Now.AddMinutes(30),
                }
            };

            return blockedUsers;
        }

        private static IEnumerable<Password> CreatePasswordList()
        {
            List<User> users = CreateUserList().ToList();
            List<Password> passwords = new List<Password>()
            {
                new Password()
                {
                    Id = 1,
                    User = users[0],
                    LoginService = "login1",
                    PassWord = "eKBXlrliDRm1fmhAF8TlOw==", //123
                    PassService = "faceebook"
                },
                new Password()
                {
                    Id = 2,
                    User = users[1],
                    LoginService = "login2",
                    PassWord = "vrcR5dYkzSJIgvSW6W/3JQ==", //crack
                    PassService = "faceebook"
                },
                new Password()
                {
                    Id = 3,
                    User = users[1],
                    LoginService = "login2",
                    PassWord = "On2uylG9BrQrh077uVZP6g==", //best
                    PassService = "youtube"
                },
            };

            return passwords;
        }
    }
}
