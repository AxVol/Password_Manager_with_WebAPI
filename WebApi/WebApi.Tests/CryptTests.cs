using WebApi.Service.Implementations;
using Xunit;

namespace WebApi.Tests
{
    public class CryptTests
    {
        [Fact]
        public void EncryptTest()
        {
            // Arrage
            Cryptography cryptography = new Cryptography();
            string password = "123";
            string encryptEqual = "ewWaV4IUG6Ml1unXXCEAxQ==";

            // Act
            string encrypt = cryptography.EncryptPassword(password);

            // Assert
            Assert.Equal(encryptEqual, encrypt);
        }

        [Fact]
        public void DecryptTest()
        {
            // Arrage
            Cryptography cryptography = new Cryptography();
            string passwordCheck = "123";
            string encrypt = "ewWaV4IUG6Ml1unXXCEAxQ==";

            // Act
            string password = cryptography.DecryptPassword(encrypt);

            // Assert
            Assert.Equal(passwordCheck, password);
        }
    }
}