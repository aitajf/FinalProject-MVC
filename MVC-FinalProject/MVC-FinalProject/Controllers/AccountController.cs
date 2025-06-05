
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Account;
using MVC_FinalProject.Services.Interfaces;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MVC_FinalProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new Login());
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(Login model)
        //{
        //    if (!ModelState.IsValid) return View(model);

        //    var response = await _accountService.Login(model);
        //    var content = await response.Content.ReadAsStringAsync();

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        ModelState.AddModelError(string.Empty, "Login failed. Please check your credentials.");
        //        return View(model);
        //    }

        //    var loginResponse = JsonSerializer.Deserialize<LoginResponse>(content, new JsonSerializerOptions
        //    {
        //        PropertyNameCaseInsensitive = true
        //    });

        //    if (loginResponse != null && loginResponse.Success)
        //    {


        //        HttpContext.Session.SetString("AuthToken", loginResponse.Token);
        //        HttpContext.Session.SetString("UserName", loginResponse.UserName ?? "");




        //        var claims = new List<Claim>
        //        {
        //             new Claim(ClaimTypes.Name, loginResponse.UserName ?? ""),


        //             new Claim("access_token", loginResponse.Token ?? "") // 🔥 TOKEN Saved!
        //        };

        //        if (loginResponse.Roles != null && loginResponse.Roles.Any())
        //        {
        //            foreach (var role in loginResponse.Roles)
        //            {
        //                claims.Add(new Claim(ClaimTypes.Role, role));
        //            }
        //        }

        //        var identity = new ClaimsIdentity(claims, "MyCookieAuth");
        //        var principal = new ClaimsPrincipal(identity);
        //        await HttpContext.SignInAsync("MyCookieAuth", principal);


        //        await HttpContext.SignInAsync("MyCookieAuth", principal, new AuthenticationProperties
        //        {
        //            IsPersistent = true, // 🔥 Cookie saved!
        //            ExpiresUtc = DateTime.UtcNow.AddHours(2) // 🔥 2 hours session !
        //        });

        //        return RedirectToAction("Index", "Home");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError(string.Empty, loginResponse?.Error ?? "Login failed.");
        //        return View(model);
        //    }
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model)
        {
            if (!ModelState.IsValid) return View(model);

            var response = await _accountService.Login(model);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Login failed. Please check your credentials.");
                return View(model);
            }

            var loginResponse = JsonSerializer.Deserialize<LoginResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (loginResponse != null && loginResponse.Success)
            {
                HttpContext.Session.SetString("AuthToken", loginResponse.Token);
                HttpContext.Session.SetString("UserName", loginResponse.UserName ?? "");

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, loginResponse.UserName ?? ""),
            new Claim("access_token", loginResponse.Token ?? "")
        };

                if (loginResponse.Roles != null && loginResponse.Roles.Any())
                {
                    foreach (var role in loginResponse.Roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                }

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyCookieAuth", principal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours(2)
                });

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register request)
        {
            if (!ModelState.IsValid) return View(request);
            var responseMessage = await _accountService.Register(request);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("RegisterConfirmation");
            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                ModelState.AddModelError("UserName", "Username already exists. Please choose a different username.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "An error occurred while registering the account.");
            }

            return View(request);
        }

        public IActionResult RegisterConfirmation()
        {
            return View();
        }



        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var responseMessage = await _accountService.ForgetPasswordAsync(email);

            if (responseMessage == "Email cannot be empty.")
            {
                ModelState.AddModelError("", responseMessage);
                return View();
            }

            TempData["Message"] = responseMessage;
            return RedirectToAction("ForgetPasswordConfirmation");
        }

        public IActionResult ForgetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            var model = new UserPassword
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(UserPassword userPasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View(userPasswordVM);
            }

            var responseMessage = await _accountService.ResetPasswordAsync(userPasswordVM);

            if (responseMessage == "Invalid request.")
            {
                ModelState.AddModelError("", responseMessage);
                return View(userPasswordVM);
            }

            TempData["Message"] = responseMessage;
            return RedirectToAction("ResetPasswordConfirmation");
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}
