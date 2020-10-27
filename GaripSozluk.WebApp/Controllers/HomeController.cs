using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GaripSozluk.WebApp.Models;
using GaripSozluk.Business.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using GaripSozluk.Common.ViewModels;
using Microsoft.Extensions.Localization;

namespace GaripSozluk.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService _postService;
        private readonly IPostCategoryService _postCategoryService;
        private readonly ICommentService _commentService;
        private readonly IApiService _apiService;
        private readonly ILogService _logService;
        private readonly IStringLocalizer<HomeController> _localizer;
        public HomeController(ILogger<HomeController> logger, IPostService postService, IPostCategoryService postCategoryService, ICommentService commentService,IApiService apiService,ILogService logService, IStringLocalizer<HomeController> stringLocalizer)
        {
            _logger = logger;
            _postService = postService;
            _postCategoryService = postCategoryService;
            _commentService = commentService;
            _apiService = apiService;
            _logService = logService;
            _localizer = stringLocalizer;
        }

        // Ana Sayfa
        public IActionResult Index(string searchText, int selectedCategoryId=1,  int? postId=null,int currentPage=1)
        {
            //ViewData["Title"] = _localizer.GetString("Title");


            //Todo: Viewbag kısımları mümkün oldukça model içine koyup öyle dönelim. Mesela PostRowVM içine viewbag ile view tarafına döneceğin verileri de dahil etmek iyi bir çözüm olur.
            ViewBag.PostCategory = _postCategoryService.PostCategoryList(selectedCategoryId);          
            ViewBag.PostCategoryName = _postCategoryService.GetPostCategory(selectedCategoryId);
            ViewBag.Post = _postService.GetAllByCategoryId(selectedCategoryId).ToList();
                        
            // postıd hasvalue ise postu ve yorumlarını çek
            if (postId.HasValue)
            {
                //Todo: Değişken isimleri içerisinde tutacağı verilerin ne olduğunu ifade edecek şekilde tanımlarsan senin açından daha iyi olur. Bugün belki anlıyorsun ama bundan 1 ay sonra projeyi açtığında ve bu metot içinde birden fazla aynı servisi çağırıyorsan işler karışık bir hale dönebilir. Konuyla ilgili proje ilerlediğinde sıkıntı yaşamamak için bazı temel prensiplerin yer aldığı "Clean Code" (temiz kod) diye bir terim var. Bunu araştır proje büyüdükçe işlerini kolaylaştıracak.
                //https://medium.com/@busrauzun/clean-code-kitabindan-notlar-1-temiz-kod-derken-44e6f7a27eb0
                var postAndComments = new PostRowVM();
                if (selectedCategoryId==5)
                {
                    //_apiService.GetApi(searchText);
                     postAndComments = _postService.GetPostById(searchText, postId.Value, currentPage);
                }
                else
                {
                    postAndComments = _postService.GetPostById("", postId.Value, currentPage);
                }
              
                return View(postAndComments);
            }
            else
            {
                var postAndComments = _postService.GetPostById("",1, currentPage);
                return View(postAndComments);
            }
        }

        // Arama
        public IActionResult SearchPost(SearchVM model)
        {
            var search = _postService.SearchPost(model);
            return View(search);
        }

        //Detaylı arama
        public IActionResult DetailSearchPost(SearchVM model)
        {
            if (ModelState.IsValid)
            {
                var search = _postService.SearchPost(model);
                return View(search);
            }

            return View();
        }

        //Logları Çekme
        public IActionResult GetLog()
        {
           var getLog= _logService.GetAllLogRowVM(null);
            return View(getLog);
        }

        //Filtreyle beraber logları çekme
        [HttpPost]
        public IActionResult GetLog(LogRowVM logRowVM)
        {
            var getLog = _logService.GetAllLogRowVM(logRowVM);
            return View(getLog);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
