using Desktop_client.Models;
using Desktop_client.Services.Interfaces;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Desktop_client.Services.Implementations
{
    public class ConnectionService : IConnectionService
    {
        private readonly HttpClient httpClient;
        private readonly IPasswordService passwordService;
        private readonly IUserManager userManager;
        private readonly int countToBan = 3;
        private readonly string url = "https://localhost:32769/api/User";

        private int tryedToLogin = 0;

        public ConnectionService(IHttpClientFactory httpFactory, IUserManager manager, IPasswordService pass) 
        {
            httpClient = httpFactory.CreateClient();
            userManager = manager;
            passwordService = pass;
        }

        public bool HasEthernet()
        {
            using var response = httpClient.GetAsync("https://www.google.com");

            try
            {
                HttpStatusCode result = response.Result.StatusCode;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> Login(string login, string password)
        {
            LoginModel loginModel = new LoginModel()
            {
                Login = login,
                Password = password
            };

            JsonContent json = JsonContent.Create(loginModel);
            using var response = await httpClient.PostAsync($"{url}/Authentication", json);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Error? error = await response.Content.ReadFromJsonAsync<Error>();

                if (long.TryParse(error.Description, out long id))
                {
                    if (tryedToLogin == countToBan)
                    {
                        await BlockAccount(id);

                        return "Вы были заблокированы за подозрительную активность";
                    }

                    tryedToLogin++;

                    return "Неверный логин или пароль";
                }

                return error.Description;
            }

            string? jwt = await response.Content.ReadFromJsonAsync<string>();

            if (jwt != null)
            {
                userManager.Token = jwt;
                userManager.Passwords = await passwordService.GetAll(userManager.Token);

                return "Успешно";
            }

            return "Не предвиденная ошибка";
        }

        public async Task<string> Register(RegistrationModel model)
        {
            JsonContent json = JsonContent.Create(model);
            using var response = await httpClient.PostAsync($"{url}/Register", json);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                Error? error = await response.Content.ReadFromJsonAsync<Error>();

                return error.Description;
            }

            string? jwt = await response.Content.ReadFromJsonAsync<string>();

            if (jwt != null)
            {
                userManager.Token = jwt;
                userManager.Passwords = await passwordService.GetAll(userManager.Token);

                return "Успешно";
            }

            return "Не предвиденная ошибка";
        }

        private async Task BlockAccount(long id)
        {
            JsonContent json = JsonContent.Create(new { id });

            await httpClient.PostAsync($"{url}/BlockAccount", json);
        }
    }
}
