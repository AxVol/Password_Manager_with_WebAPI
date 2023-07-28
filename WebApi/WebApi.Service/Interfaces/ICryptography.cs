namespace WebApi.Service.Interfaces
{
    public interface ICryptography
    {
        string GetPasswordHash(string password);
        string CypherPassword(string password);
    }
}
