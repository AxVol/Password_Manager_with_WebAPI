using Desktop_client.Models;
using Desktop_client.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Desktop_client.Services.Implementations
{
    public class PasswordService : IPasswordService
    {
        public Task<Password> Create(PasswordSendModel model)
        {
            throw new NotImplementedException();
        }

        public Task Delete(PasswordSendModel model)
        {
            throw new NotImplementedException();
        }

        public Task<string> GenerateStrongPassword()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Password>> GetAll(PasswordSendModel model)
        {
            throw new NotImplementedException();
        }

        public Task<Password> Update(PasswordSendModel model)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateUserToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
