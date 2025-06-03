using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.ViewModels;

namespace MVC_FinalProject.Controllers
{
    public class ContactController : Controller
    {
        private readonly IHelpSectionService _helpSectionService;
        public ContactController(IHelpSectionService helpSectionService)
        {
            _helpSectionService = helpSectionService;
        }
        public async Task<IActionResult> Index()
        {
            var helpSections = await _helpSectionService.GetAllAsync();

            ContactVM model = new()
            {
                HelpSections = helpSections
            };
            return View(model);
        }
    }
}
