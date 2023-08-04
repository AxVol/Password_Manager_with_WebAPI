using Desktop_client.Models;
using Desktop_client.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text;

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

        public async Task<ObservableCollection<Password>> GetAll(string token)
        {
            using var response = await httpClient.GetAsync
                ($"https://localhost:7125/api/Passwords/GetUserPass?secretToken={token}");

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                return new ObservableCollection<Password>();

            ObservableCollection<Password>? passwords = await response.Content.ReadFromJsonAsync<ObservableCollection<Password>>();

            return passwords;
        }

        public async Task Update(PasswordSendModel model)
        {
            JsonContent json = JsonContent.Create(model);

            await httpClient.PutAsync("https://localhost:7125/api/Passwords/UpdatePass", json);
        }

        public async Task<string> UpdateUserToken(string token)
        {
            using var response = await httpClient.PutAsync($"https://localhost:7125/api/User/UpdateToken?secretToken={token}", null);   
            string backToken = await response.Content.ReadAsStringAsync(); //{"value":"NTUwY2ExODUtODMwZi00Mjc3LWJmYjAtYTc3ZGZiMjY2ZDdh"}

            var sb = new StringBuilder(backToken);
            sb.Remove(0, 10);
            sb.Replace('\\', ' ');
            sb.Replace('"', ' ');
            sb.Replace('}', ' ');

            return sb.ToString(); 
        }
    }
}
