using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Models.Account;
using MVC_FinalProject.Services.Interfaces;
using Register = MVC_FinalProject.Models.Account.Register;
using Login = MVC_FinalProject.Models.Account.Login;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
            return await _httpClient.PostAsJsonAsync($"{Urls.AccountClientUrl}Register", model);
        }
        public async Task<HttpResponseMessage> Login(Login model)
        {
            return await _httpClient.PostAsJsonAsync($"{Urls.AccountClientUrl}Login", model);
        }
        public async Task<string> ForgetPasswordAsync(string email)
        {
            if (string.IsNullOrEmpty(email)) return "Email cannot be empty.";

            var content = new StringContent($"\"{email}\"", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Urls.AccountClientUrl}ForgetPassword", content);

            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> ResetPasswordAsync(UserPassword model)
        {
            if (model == null) return "Invalid request.";

            var response = await _httpClient.PostAsJsonAsync($"{Urls.AccountClientUrl}ResetPassword", model);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> AddRoleAsync(string username, string roleName)
        {
            var response = await _httpClient.PostAsync(
                $"{Urls.AccountUrl}AddRole?username={username}&roleName={roleName}", null);

            return await response.Content.ReadAsStringAsync();
        }


        public async Task<string> RemoveRoleAsync(string username, string roleName)
        {


            var response = await _httpClient.PostAsync($"{Urls.AccountUrl}RemoveRole?username={username}&roleName={roleName}", null);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<List<UserRole>> GetAllUsersAsync()
        {
            var response = await _httpClient.GetAsync($"{Urls.AccountUrl}GetAllUsers");

            if (!response.IsSuccessStatusCode)
                return new List<UserRole>();

            return await response.Content.ReadFromJsonAsync<List<UserRole>>();
        }

        public async Task<List<string>> GetAllRolesAsync()
        {
            var response = await _httpClient.GetAsync($"{Urls.AccountUrl}GetAllRoles");

            if (!response.IsSuccessStatusCode)
                return new List<string>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<string>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<List<string>> GetUserRolesAsync(string username)
        {
            var response = await _httpClient.GetAsync($"{Urls.AccountUrl}GetUserRoles?username={username}");

            if (!response.IsSuccessStatusCode)
                return new List<string>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<string>>(json);
        }


        public async Task<string> SendMessageToAdminAsync(AdminMessage model)
        {
            var response = await _httpClient.PostAsJsonAsync($"{Urls.AccountUrl}SendMessageToAdmin", model);
            return await response.Content.ReadAsStringAsync();

        }

        public async Task<List<string>> GetAdminsEmailsAsync()
        {
            var response = await _httpClient.GetAsync($"{Urls.AccountUrl}GetAdminsEmails");

            if (!response.IsSuccessStatusCode)
                return new List<string>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<string>>(json);
        }

    }
}
