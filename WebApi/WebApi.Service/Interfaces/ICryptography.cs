﻿namespace WebApi.Service.Interfaces
{
    public interface ICryptography
    {
        string GetPasswordHash(string password, string salt);
        string EncryptPassword(string password, long userId);
        string DecryptPassword(string cryptedPassword, long userId);
    }
}
