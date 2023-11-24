﻿using Desktop_client.Models;
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

        private int CountToBan { get; } = 3;
        private int TryedToLogin { get; set; } = 0;

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
            using var response = await httpClient.PostAsync("https://localhost:7125/api/User/Authentication", json);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Error? error = await response.Content.ReadFromJsonAsync<Error>();

                if (long.TryParse(error.Description, out long id))
                {
                    if (TryedToLogin == CountToBan)
                    {
                        await BlockAccount(id);

                        return "Вы были заблокированы за подозрительную активность";
                    }

                    TryedToLogin++;

                    return "Неверный логин или пароль";
                }

                return error.Description;
            }

            User? user = await response.Content.ReadFromJsonAsync<User>();

            if (user != null)
            {
                userManager.user = user;
                userManager.passwords = await passwordService.GetAll(userManager.user.SecretToken);

                return "Успешно";
            }

            return "Не предвиденная ошибка";
        }

        public async Task<string> Register(RegistrationModel model)
        {
            JsonContent json = JsonContent.Create(model);
            using var response = await httpClient.PostAsync("https://localhost:7125/api/User/Register", json);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                Error? error = await response.Content.ReadFromJsonAsync<Error>();

                return error.Description;
            }

            User? user = await response.Content.ReadFromJsonAsync<User>();

            if (user != null)
            {
                userManager.user = user;

                return "Успешно";
            }

            return "Не предвиденная ошибка";
        }

        private async Task BlockAccount(long id)
        {
            JsonContent json = JsonContent.Create(new { id });

            await httpClient.PostAsync("https://localhost:7125/api/User/BlockAccount", json);
        }
    }
}
