using GaripSozluk.Business.Interfaces;
using GaripSozluk.Common.ViewModels;
using GaripSozluk.Data.Domain;
using GaripSozluk.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GaripSozluk.Business.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;
        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }


        //Log Ekleme
        public void AddLog(HttpContext context)
        {
            var log = new Log();
            log.CreateDate = DateTime.Now;
            log.IPAddress = context.Connection.RemoteIpAddress.ToString();
            log.RequestMethod = context.Request.Method.ToString();
            log.RequestPath = context.Request.Path.ToString();
            log.ResponseStatusCode = context.Response.StatusCode;
            log.RoutePath = context.Request.QueryString.ToString();
            log.TraceIdentifier = context.TraceIdentifier.ToString();
            log.UserAgent = context.Request.Headers["User-Agent"].ToString();

            _logRepository.Add(log);
            _logRepository.SaveChanges();
        }

        //Logları Çekme
        public LogRowVM GetAllLogRowVM(LogRowVM logRowVM)
        {
            var model = new LogRowVM();
            model.logVMs = new List<LogVM>();
            var getLog = _logRepository.GetAll();
            foreach (var item in getLog)
            {
                var getLogVm = new LogVM();
                getLogVm.IPAddress = item.IPAddress;
                getLogVm.RequestMethod = item.RequestMethod;
                getLogVm.RequestPath = item.RequestPath;
                getLogVm.ResponseStatusCode = item.ResponseStatusCode;
                getLogVm.RoutePath = item.RoutePath;
                getLogVm.TraceIdentifier = item.TraceIdentifier;
                getLogVm.UserAgent = item.UserAgent;
                model.logVMs.Add(getLogVm);
            }
            if (logRowVM != null)
            {
                model.logFilterVMs = new List<LogFilterVM>();

                if (logRowVM.startDate.HasValue || logRowVM.endDate.HasValue)
                {
                    if (logRowVM.startDate.HasValue && logRowVM.endDate == null)
                    {
                        getLog = getLog.Where(x => x.CreateDate >= logRowVM.startDate);
                    }
                    else if (logRowVM.startDate == null && logRowVM.endDate.HasValue)
                    {
                        getLog = getLog.Where(x => x.CreateDate <= logRowVM.endDate);
                    }
                    else
                    {
                        getLog = getLog.Where(x => (x.CreateDate >= logRowVM.startDate && x.CreateDate <= logRowVM.endDate));
                    }
                }
                var requestPathGroup = getLog.ToList().GroupBy(x => x.RequestPath).OrderByDescending(x => x.Count()).Take(10);

                foreach (var item in requestPathGroup)
                {
                    var getLogVm = new LogFilterVM();

                    getLogVm.RequestPath = item.Key;
                    getLogVm.viewCount = item.Count();

                    model.logFilterVMs.Add(getLogVm);
                }

            }
            return model;
        }


        // verilen tarihteki günlük log kayıtlarının çekilmesi
        public List<LogVM> GetLogComments(DateTime dateTime)
        {
          var logVMs = new List<LogVM>();
            var getLog = _logRepository.GetAll(x=>x.CreateDate.Date==dateTime);
            foreach (var item in getLog)
            {
                var getLogVm = new LogVM();
                getLogVm.IPAddress = item.IPAddress;
                getLogVm.RequestMethod = item.RequestMethod;
                getLogVm.RequestPath = item.RequestPath;
                getLogVm.ResponseStatusCode = item.ResponseStatusCode;
                getLogVm.RoutePath = item.RoutePath;
                getLogVm.TraceIdentifier = item.TraceIdentifier;
                getLogVm.UserAgent = item.UserAgent;
                logVMs.Add(getLogVm);
            }
            return logVMs;
        }

        // verilen tarihteki günlük log kayıtlarının çekilmesi gruplanması ve sıralanması
        public List<LogFilterVM> GetLogCommentsFilter(DateTime dateTime)
        {
            var logFilterVMs = new List<LogFilterVM>();
            var getLog = _logRepository.GetAll(x => x.CreateDate.Date == dateTime);
            var requestPathGroup = getLog.ToList().GroupBy(x => x.RequestPath).OrderByDescending(x => x.Count()).Take(10);
            foreach (var item in requestPathGroup)
            {
                var logFilter = new LogFilterVM();
                logFilter.RequestPath = item.Key;
                logFilter.viewCount = item.Count();
                logFilterVMs.Add(logFilter);
            }
            return logFilterVMs;
        }
    }
}
