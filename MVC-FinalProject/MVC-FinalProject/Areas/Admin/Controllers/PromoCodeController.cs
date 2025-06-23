﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.PromoCode;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    [Area("Admin")]
    public class PromoCodeController : Controller
    {
        private readonly IPromoCodeService _promoCodeService;

        public PromoCodeController(IPromoCodeService promoCodeService)
        {
            _promoCodeService = promoCodeService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var promocodes = await _promoCodeService.GetAllAsync();
            return View(promocodes);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(PromoCodeCreate request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            await _promoCodeService.CreateAsync(request);

            TempData["SuccessMessage"] = "Promokod yaradıldı və istifadəçilərə göndərildi.";
            return RedirectToAction("Index");
        }

      

    }
}
