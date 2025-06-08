
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
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

        //    //if (!response.IsSuccessStatusCode)
        //    //{
        //    //    ModelState.AddModelError(string.Empty, "Login failed. Please check your credentials.");
        //    //    return View(model);
        //    //}




        //    var loginResponse = JsonSerializer.Deserialize<LoginResponse>(content, new JsonSerializerOptions
        //    {
        //        PropertyNameCaseInsensitive = true
        //    });

        //    if (loginResponse != null && loginResponse.Success)
        //    {
        //        HttpContext.Session.SetString("AuthToken", loginResponse.Token);
        //        HttpContext.Session.SetString("UserName", loginResponse.UserName ?? "");

        //         var claims = new List<Claim>
        //         {
        //            new Claim(ClaimTypes.Name, loginResponse.UserName ?? ""),
        //            new Claim(ClaimTypes.NameIdentifier, loginResponse.UserId),
        //            new Claim("access_token", loginResponse.Token ?? "")
        //          };


        //        if (loginResponse.Roles != null && loginResponse.Roles.Any())
        //        {
        //            foreach (var role in loginResponse.Roles)
        //            {
        //                claims.Add(new Claim(ClaimTypes.Role, role));
        //            }
        //        }

        //        var identity = new ClaimsIdentity(claims, "MyCookieAuth");
        //        var principal = new ClaimsPrincipal(identity);

        //        await HttpContext.SignInAsync("MyCookieAuth", principal, new AuthenticationProperties
        //        {
        //            IsPersistent = true,
        //            ExpiresUtc = DateTime.UtcNow.AddHours(2)
        //        });

        //        return RedirectToAction("Index", "Home");
        //    }

        //    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        //    return View(model);
        //}





        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(Login model)
        //{
        //    // 1. Model validation
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    // 2. API çağırışı
        //    var response = await _accountService.Login(model);
        //    var content = await response.Content.ReadAsStringAsync();

        //    // 3. Login cavabını deserialize et
        //    var loginResponse = JsonSerializer.Deserialize<LoginResponse>(content, new JsonSerializerOptions
        //    {
        //        PropertyNameCaseInsensitive = true
        //    });

        //    // 4. API uğursuz cavab qaytardısa
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        if (loginResponse != null && !string.IsNullOrWhiteSpace(loginResponse.Error))
        //        {
        //            ModelState.AddModelError(string.Empty, loginResponse.Error);
        //        }
        //        else
        //        {
        //            ModelState.AddModelError(string.Empty, "Login failed. Please try again.");
        //        }

        //        return View(model);
        //    }

        //    // 5. Login uğurludursa
        //    if (loginResponse != null && loginResponse.Success)
        //    {
        //        HttpContext.Session.SetString("AuthToken", loginResponse.Token);
        //        HttpContext.Session.SetString("UserName", loginResponse.UserName ?? "");

        //        var claims = new List<Claim>
        //{
        //    new Claim(ClaimTypes.Name, loginResponse.UserName ?? ""),
        //    new Claim(ClaimTypes.NameIdentifier, loginResponse.UserId),
        //    new Claim("access_token", loginResponse.Token ?? "")
        //};

        //        if (loginResponse.Roles != null && loginResponse.Roles.Any())
        //        {
        //            foreach (var role in loginResponse.Roles)
        //            {
        //                claims.Add(new Claim(ClaimTypes.Role, role));
        //            }
        //        }

        //        var identity = new ClaimsIdentity(claims, "MyCookieAuth");
        //        var principal = new ClaimsPrincipal(identity);

        //        await HttpContext.SignInAsync("MyCookieAuth", principal, new AuthenticationProperties
        //        {
        //            IsPersistent = true,
        //            ExpiresUtc = DateTime.UtcNow.AddHours(2)
        //        });

        //        return RedirectToAction("Index", "Home");
        //    }

        //    // 6. Nəticə uğursuzdursa və loginResponse null-dursa
        //    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        //    return View(model);
        //}



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors });
            }

            var response = await _accountService.Login(model);
            var content = await response.Content.ReadAsStringAsync();

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
            new Claim(ClaimTypes.NameIdentifier, loginResponse.UserId),
            new Claim("access_token", loginResponse.Token ?? "")
        };

                if (loginResponse.Roles != null)
                {
                    foreach (var role in loginResponse.Roles)
                        claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyCookieAuth", principal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours(2)
                });

                return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
            }

            return Json(new
            {
                success = false,
                errors = new List<string> { loginResponse?.Error ?? "Invalid login attempt." }
            });
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(Register request)
        //{
        //    if (!ModelState.IsValid) return View(request);
        //    var responseMessage = await _accountService.Register(request);

        //    if (responseMessage.IsSuccessStatusCode)
        //    {
        //        return RedirectToAction("RegisterConfirmation");
        //    }
        //    else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Conflict)
        //    {
        //        ModelState.AddModelError("UserName", "Username already exists. Please choose a different username.");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError(string.Empty, "An error occurred while registering the account.");
        //    }

        //    return View(request);
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register request)
        {
            if (!ModelState.IsValid)
            {
                // Model validation errorlarını JSON formatında qaytarırıq
                var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                                       .ToDictionary(
                                           kvp => kvp.Key,
                                           kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                       );

                return BadRequest(new { errors });
            }

            var responseMessage = await _accountService.Register(request);

            if (responseMessage.IsSuccessStatusCode)
            {
                // Uğurlu qeydiyyat
                return Ok(new { success = true, redirectUrl = Url.Action("RegisterConfirmation", "Account") });
            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                // Username artıq mövcuddur
                return Conflict(new { errors = new { UserName = new[] { "Username already exists. Please choose a different username." } } });
            }
            else
            {
                // Ümumi səhv
                return BadRequest(new { errors = new { General = new[] { "An error occurred while registering the account." } } });
            }
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
