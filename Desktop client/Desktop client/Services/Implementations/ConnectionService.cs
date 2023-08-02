using Desktop_client.Models;
using Desktop_client.Services.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Desktop_client.Services.Implementations
{
    public class ConnectionService : IConnectionService
    {
        private readonly HttpClient httpClient;
        private IUserManager userManager;

        public ConnectionService(IHttpClientFactory httpFactory, IUserManager manager) 
        {
            httpClient = httpFactory.CreateClient();
            userManager = manager;
        }

        public async Task<string> Login(string login, string password)
        {
            LoginModel loginModel = new LoginModel()
            {
                Login = login,
                Password = password
            };

            JsonContent json = JsonContent.Create(loginModel);
            using var response = await httpClient.PostAsync("https://localhost:7125/api/User/Authentication", json);

            try
            {
                User? user = await response.Content.ReadFromJsonAsync<User>();

                if (user != null)
                {
                    userManager.user = user;

                    return "Успешно";
                }

                return "Не предвиденная ошибка";
            }
            catch(Exception ex)
            {
                return await response.Content.ReadFromJsonAsync<string>();
            }
        }

        public async Task Register(User user)
        {
            throw new NotImplementedException();
        }
    }
}
