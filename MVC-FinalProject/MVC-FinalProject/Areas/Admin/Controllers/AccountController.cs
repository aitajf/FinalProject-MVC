using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_FinalProject.Models.Account;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    [Area("Admin")]
    public class AccountController : Controller
    {

        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _accountService.GetAllUsersAsync();
            return View(users);
        }


        [HttpGet]
        public async Task<IActionResult> AddRole(string username)
        {
            ViewBag.Roles = new SelectList(await _accountService.GetAllRolesAsync());
            return View(new Role { Username = username });
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(Role model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.RoleName))
            {
                ModelState.AddModelError("", "Invalid data.");
                ViewBag.Roles = new SelectList(await _accountService.GetAllRolesAsync());
                return View(model);
            }

            var userRoles = await _accountService.GetUserRolesAsync(model.Username);
            if (userRoles.Contains(model.RoleName, StringComparer.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("", $"User already has the '{model.RoleName}' role.");
                ViewBag.Roles = new SelectList(await _accountService.GetAllRolesAsync());
                return View(model);
            }

            var result = await _accountService.AddRoleAsync(model.Username, model.RoleName);

            if (result.Contains("successfully", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result);
            ViewBag.Roles = new SelectList(await _accountService.GetAllRolesAsync());
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> RemoveRole(string username)
        {
            if (string.IsNullOrEmpty(username))
                return BadRequest("Username is required.");

            var roles = await _accountService.GetUserRolesAsync(username);
            if (roles == null || !roles.Any())
            {
                // İstifadəçinin heç bir rolu yoxdursa xəbərdar edə bilərik
                ViewBag.Message = "This user has no roles to remove.";
                return View(new Role { Username = username });
            }

            ViewBag.Roles = new SelectList(roles);
            return View(new Role { Username = username });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(Role model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.RoleName))
            {
                ModelState.AddModelError("", "Invalid data.");
                ViewBag.Roles = new SelectList(await _accountService.GetUserRolesAsync(model.Username));
                return View(model);
            }

            var result = await _accountService.RemoveRoleAsync(model.Username, model.RoleName);

            if (result.Contains("successfully", StringComparison.OrdinalIgnoreCase))
                return RedirectToAction("Index");

            ModelState.AddModelError("", result);
            ViewBag.Roles = new SelectList(await _accountService.GetUserRolesAsync(model.Username));
            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> SendMessage()
        {
            var emails = await _accountService.GetAdminsEmailsAsync();
            ViewBag.AdminEmails = new SelectList(emails);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(AdminMessage request)
        {
            if(!ModelState.IsValid) return View(request);
            var emails = await _accountService.GetAdminsEmailsAsync();
            ViewBag.AdminEmails = new SelectList(emails);

            var result = await _accountService.SendMessageToAdminAsync(request);
            ViewBag.Message = result;

            return View(request);
        }

    }
}
