using GaripSozluk.Business.Interfaces;
using GaripSozluk.Common.ViewModels;
using GaripSozluk.Data.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaripSozluk.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly ILogger _logger;
        public AuthService(UserManager<User> userManager, ILogger<AuthService> logger, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;

        }

        //Kullanıcı Kayıt
        public ServiceStatus Register(RegisterVM model)
        {
            var serviceStatus = new ServiceStatus();

            var user = new User() { UserName = model.UserName, Email = model.Email, BirthDate = model.BirthDate, CreateDate = DateTime.Now };
            var result = _userManager.CreateAsync(user, model.Password).Result;
            if (result.Succeeded)
            {
                _logger.LogInformation("Kullanıcı başarıyla oluştu.");
                serviceStatus.Status = true;
            }
            else
            {
                serviceStatus.Status = false;
            }
            return serviceStatus;
        }


        //Kullanıcı Giriş
        public ServiceStatus Login(LoginVM model)
        {
            var serviceStatus = new ServiceStatus();

            var result = _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.IsPersistent, false).Result;
            if (result.Succeeded)
            {
                _logger.LogInformation("Kullanıcı girişi başarılı");
                serviceStatus.Status = true;
            }
            else
            {
                serviceStatus.Status = false;

            }
            return serviceStatus;
        }

        //Çıkış
        public void LogOut()
        {
            _signInManager.SignOutAsync().Wait();
        }
    }
}
