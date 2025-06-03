using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Models.Account;
using MVC_FinalProject.Services.Interfaces;
using Register = MVC_FinalProject.Models.Account.Register;
using Login = MVC_FinalProject.Models.Account.Login;
using System.Text;

namespace MVC_FinalProject.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;
        public AccountService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<HttpResponseMessage> Register(Register model)
        {
            return await _httpClient.PostAsJsonAsync($"{Urls.AccountUrl}Register", model);
        }
        public async Task<HttpResponseMessage> Login(Login model)
        {
            return await _httpClient.PostAsJsonAsync($"{Urls.AccountUrl}Login", model);                  
        }


        public async Task<string> ForgetPasswordAsync(string email)
        {
            if (string.IsNullOrEmpty(email)) return "Email cannot be empty.";

            var content = new StringContent($"\"{email}\"", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Urls.AccountUrl}ForgetPassword", content);

            return await response.Content.ReadAsStringAsync();
        }


        public async Task<string> ResetPasswordAsync(UserPassword model)
        {
            if (model == null) return "Invalid request.";

            var response = await _httpClient.PostAsJsonAsync($"{Urls.AccountUrl}ResetPassword", model);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
