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
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger _logger;

        public AuthenticationController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<Role> roleManager, ILogger<AuthenticationController> logger, IAuthService authService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _authService = authService;
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                //var user = new User { UserName = model.UserName, Email = model.Email, BirthDate = model.BirthDate, CreateDate = DateTime.Now };
                //var result = _userManager.CreateAsync(user, model.Password).Result;
                //if (result.Succeeded)
                //{
                //    _logger.LogInformation("Kullanıcı başarıyla oluştu.");
                //    return Redirect(Url.Action("Login", "Authentication"));
                //}
                //else
                //{
                //    foreach (var error in result.Errors)
                //    {
                //        ModelState.AddModelError("", $"{error.Code} -> {error.Description}");
                //    }
                //}
                var serviceStatus = _authService.Register(model);

                if (serviceStatus.Status)
                {
                    return Redirect(Url.Action("Login", "Authentication"));

                }

            }

            return View(model);
        }

        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                //var result =  _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.IsPersistent, false).Result;
                //if (result.Succeeded)
                //{
                //    _logger.LogInformation("Kullanıcı girişi başarılı");
                //    return View(model);/*Redirect(Url.Action("Privacy", "Home"));*/
                //}
                //else
                //{

                //        ModelState.AddModelError("", "Kullanıcı adı ya da şifre hatalı");

                //}

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
            //_signInManager.SignOutAsync().Wait();
            _authService.LogOut();
            return Redirect(Url.Action("Login", "Authentication"));
        }
    }
}
