using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaripSozluk.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GaripSozluk.WebApp.Controllers
{
    public class ApiController : Controller
    {
        private readonly IApiService _apiService;
        public ApiController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult GetApi()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetApi(string searchText,int option)
        {
           var books= _apiService.GetApi(searchText,option);   
            return View(books);
        }
    }
}
