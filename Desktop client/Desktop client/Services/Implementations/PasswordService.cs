﻿using Desktop_client.Models;
using Desktop_client.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Desktop_client.Enums;
using System.Linq;

namespace Desktop_client.Services.Implementations
{
    public class PasswordService : IPasswordService
    {
        private readonly HttpClient httpClient;
        private readonly string url = "https://localhost:32769/api/Passwords";
        private readonly char[] specSymbols = new char[] { '?', '!', '@', '#', '$', '%', '&' };
        private readonly int lenChar = 9;
        private readonly int countSpecSymbols = 2;

        public PasswordService(IHttpClientFactory httpFactory) 
        {
            httpClient = httpFactory.CreateClient();
        }

        public async Task<string> Create(PasswordSendModel model, string token)
        {
            string status = "";
            JsonContent json = JsonContent.Create(model);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                Content = json,
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{url}/CreatePass"),
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
            };

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                Error error = await response.Content.ReadFromJsonAsync<Error>();
                status = error.Description;

                return status;
            }
                
            return status;
        }

        public async Task<string> Delete(PasswordSendModel model, string token)
        {
            string status = "";
            JsonContent json = JsonContent.Create(model);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                Content = json,
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{url}/DeletePass"),
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
            };

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                Error error = await response.Content.ReadFromJsonAsync<Error>();
                status = error.Description;

                return status;
            }

            return status;
        }

        public async Task<string> GenerateStrongPassword()
        {
            string password = string.Empty;
            Random random = new Random();

            await Task.Run(() =>
            {
                while (password.Length < lenChar)
                {
                    char c = (char)random.Next(33, 125);

                    if (char.IsLetterOrDigit(c))
                        password += c;
                }

                for (int i = 0; i < countSpecSymbols; i++)
                {
                    char specSymbol = specSymbols[random.Next(specSymbols.Length)];
                    int placeInPassword = random.Next(0, password.Length);

                    password = password.Insert(placeInPassword, specSymbol.ToString());
                }
            });
            return password;
        }

        public async Task<ObservableCollection<Password>> GetAll(string token)
        {
            HttpRequestMessage request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{url}/GetUserPass")
            };
            request.Headers.Add("Authorization", $"Bearer {token}");

            using var response = await httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                return new ObservableCollection<Password>();

            ObservableCollection<Password>? passwords = await response.Content.ReadFromJsonAsync<ObservableCollection<Password>>();

            return passwords;
        }

        public async Task<string> Update(PasswordSendModel model, string token)
        {
            string status = "";
            JsonContent json = JsonContent.Create(model);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                Content = json,
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{url}/UpdatePass"),
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
            };

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                Error error = await response.Content.ReadFromJsonAsync<Error>();
                status = error.Description;

                return status;
            }

            return status;
        }

        public async Task<PasswordQuality> GetPasswordQuality(string password)
        {
            bool hasDigit = password.Any(c => char.IsDigit(c));
            bool hasUpper = password.Any(c => char.IsUpper(c));
            bool hasSpecSymbols = password.IndexOfAny(specSymbols) != -1;

            if (password.Length < 8)
            {
                return PasswordQuality.Easy;
            }
            else
            {
                if (hasDigit && hasUpper && hasSpecSymbols)
                {
                    return PasswordQuality.Hard;
                }
                else if (hasDigit && hasUpper)
                {
                    return PasswordQuality.Medium;
                }
                else if (hasSpecSymbols)
                {
                    return PasswordQuality.Medium;
                }
                else
                {
                    return PasswordQuality.Easy;
                }
            }
        }
    }
}
