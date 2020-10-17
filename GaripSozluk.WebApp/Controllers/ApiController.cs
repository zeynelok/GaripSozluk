using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaripSozluk.Business.Interfaces;
using GaripSozluk.Common.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace GaripSozluk.WebApp.Controllers
{
    public class ApiController : Controller
    {
        private readonly IApiService _apiService;
        private readonly IPostService _postService;
        public ApiController(IApiService apiService,IPostService postService)
        {
            _apiService = apiService;
            _postService = postService;
        }

        public IActionResult GetApi()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetApi(string searchText, int option)
        {
            if (searchText != null && option != 0)
            {
                var books = _apiService.GetApi(searchText, option);
                return View(books);

            }
            ModelState.AddModelError("", "Boş alan kalmasın");
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddPostFromApi(string[] books)
        {
            var serviceStatus = _postService.AddPostFromApi(books);
            if (serviceStatus.Status==true)
            {
                return Json(new {x= "/Home/Index/?selectedCategoryId=5",message="Kayıtlar eklendi",status=true});
            }
         
            return Json(new {x="/Api/GetApi",message="Ekleme başarısız.",status=false});
        }
    }
}
