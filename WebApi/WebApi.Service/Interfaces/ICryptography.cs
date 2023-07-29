namespace WebApi.Service.Interfaces
{
    public interface ICryptography
    {
        string GetPasswordHash(string password);
        string EncryptPassword(string password);
        string DecryptPassword(string cryptedPassword);
    }
}
