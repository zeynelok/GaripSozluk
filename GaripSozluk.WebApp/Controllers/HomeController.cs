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

namespace GaripSozluk.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService _postService;
        private readonly IPostCategoryService _postCategoryService;
        private readonly ICommentService _commentService;

        public HomeController(ILogger<HomeController> logger, IPostService postService, IPostCategoryService postCategoryService, ICommentService commentService)
        {
            _logger = logger;
            _postService = postService;
            _postCategoryService = postCategoryService;
            _commentService = commentService;
        }

        // Ana Sayfa
        public IActionResult Index(int selectedCategoryId=1, int? postId=null,int currentPage=1)
        {                 
            ViewBag.PostCategory = _postCategoryService.PostCategoryList(selectedCategoryId);          
            ViewBag.PostCategoryName = _postCategoryService.GetPostCategory(selectedCategoryId);
            ViewBag.Post = _postService.GetAllByCategoryId(selectedCategoryId).ToList();
                        
            // postıd hasvalue ise postu ve yorumlarını çek
            if (postId.HasValue)
            {
                var hebele = _postService.GetPostById(postId.Value,currentPage);
                return View(hebele);
            }
            else
            {
                var hebele = _postService.GetPostById(1, currentPage);
                return View(hebele);
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
