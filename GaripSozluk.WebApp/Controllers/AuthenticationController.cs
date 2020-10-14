using GaripSozluk.Business.Interfaces;
using GaripSozluk.Business.Services;
using GaripSozluk.Common.ViewModels;
using GaripSozluk.Data.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GaripSozluk.Controllers
{
    public class AuthenticationController : Controller
    {
        private IAuthService _authService;
        private readonly ILogger _logger;

        public AuthenticationController(ILogger<AuthenticationController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }


        // Kullanıcı EKleme
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {

                var serviceStatus = _authService.Register(model);

                if (serviceStatus.Status)
                {
                    return Redirect(Url.Action("Login", "Authentication"));

                }

            }

            return View(model);
        }


        // Kullanıcı Giriş
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var serviceStatus = _authService.Login(model);

                if (serviceStatus.Status)
                {
                    return Redirect(Url.Action("Index", "Home"));
                }
            }
            return View(model);
        }

        [Authorize]
        public IActionResult LogOut()
        {
            _authService.LogOut();
            return Redirect(Url.Action("Login", "Authentication"));
        }
    }
}
