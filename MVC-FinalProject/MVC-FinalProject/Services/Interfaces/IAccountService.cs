using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Account;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface IAccountService
    {
        Task<HttpResponseMessage> Register(Register model);
        Task<HttpResponseMessage> Login(Login model);
        Task<string> ResetPasswordAsync(UserPassword model);
        Task<string> ForgetPasswordAsync(string email);
    }
}
