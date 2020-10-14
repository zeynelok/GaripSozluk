using GaripSozluk.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaripSozluk.Business.Interfaces
{
   public interface IAuthService
    {
        ServiceStatus Register(RegisterVM model);
        ServiceStatus Login(LoginVM model);
        public void LogOut();

    }
}
