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
            long vector = 1;
            string encryptEqual = "a7h2woGxZTUB+hsVNjCpOA==";

            // Act
            string encrypt = cryptography.EncryptPassword(password, vector);

            // Assert
            Assert.Equal(encryptEqual, encrypt);
        }

        [Fact]
        public void DecryptTest()
        {
            // Arrage
            Cryptography cryptography = new Cryptography();
            string passwordCheck = "123";
            long vector = 1;
            string encrypt = "a7h2woGxZTUB+hsVNjCpOA==";

            // Act
            string password = cryptography.DecryptPassword(encrypt, vector);

            // Assert
            Assert.Equal(passwordCheck, password);
        }

        [Fact]
        public void EncryptIvFailTest()
        {
            // Arrage
            Cryptography cryptography = new Cryptography();
            string password = "123";
            long vector = 2;
            string encryptEqual = "ewWaV4IUG6Ml1unXXCEAxQ==";

            // Act
            string encrypt = cryptography.EncryptPassword(password, vector);

            // Assert
            Assert.False(encryptEqual == encrypt);
        }

        [Fact]
        public void EncryptKeyFailTest()
        {
            // Arrage
            Cryptography cryptography = new Cryptography();
            string password = "sadasd";
            long vector = 1;
            string encryptEqual = "t6vFBOOHTK5hqlyL+a0tlw==";

            // Act
            string encrypt = cryptography.EncryptPassword(password, vector);

            // Assert
            Assert.False(encryptEqual == encrypt);
        }
    }
}