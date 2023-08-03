using Desktop_client.Models;
using Desktop_client.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Desktop_client.Services.Implementations
{
    public class PasswordService : IPasswordService
    {
        private readonly HttpClient httpClient;

        public PasswordService(IHttpClientFactory httpFactory) 
        {
            httpClient = httpFactory.CreateClient();
        }

        public async Task Create(PasswordSendModel model)
        {
            JsonContent json = JsonContent.Create(model);

            await httpClient.PostAsync("https://localhost:7125/api/Passwords/CreatePass", json);
        }

        public async Task Delete(PasswordSendModel model)
        {
            JsonContent json = JsonContent.Create(model);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                Content = json,
                Method = HttpMethod.Delete,
                RequestUri = new Uri("https://localhost:7125/api/Passwords/DeletePass")
            };

            await httpClient.SendAsync(request);
        }

        public async Task<string> GenerateStrongPassword()
        {
            string password = string.Empty;
            Random r = new Random();

            while (password.Length < 16)
            {
                char c = (char)r.Next(33, 125);

                if (char.IsLetterOrDigit(c))
                    password += c;
            }

            return password;
        }

        public Task<IEnumerable<Password>> GetAll(PasswordSendModel model)
        {
            throw new NotImplementedException();
        }

        public async Task Update(PasswordSendModel model)
        {
            JsonContent json = JsonContent.Create(model);

            await httpClient.PutAsync("https://localhost:7125/api/Passwords/UpdatePass", json);
        }

        public Task<string> UpdateUserToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
