using MVC_FinalProject.Helpers.Constants;
using System.Reflection;
using MVC_FinalProject.Models.Account;
using MVC_FinalProject.Services.Interfaces;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Register = MVC_FinalProject.Models.Account.Register;
using Login = MVC_FinalProject.Models.Account.Login;

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
    }
}
