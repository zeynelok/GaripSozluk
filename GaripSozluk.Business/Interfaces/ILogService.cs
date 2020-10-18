using GaripSozluk.Common.ViewModels;
using GaripSozluk.Data.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaripSozluk.Business.Interfaces
{
    public interface ILogService
    {
        LogRowVM GetAllLogRowVM(LogRowVM logRowVM);
        void AddLog(HttpContext context);
    }
}
