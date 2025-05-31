using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Account;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface IAccountService
    {
        Task<HttpResponseMessage> Register(Register model);
        Task<HttpResponseMessage> Login(Login model);
    }
}
