using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaripSozluk.Business.Interfaces;
using GaripSozluk.Business.Services;
using GaripSozluk.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GaripSozluk.WebApp.Controllers
{
    public class PostCategoryController : Controller
    {
        private readonly IPostCategoryService _postCategoryService;
        public PostCategoryController(IPostCategoryService postCategoryService)
        {
            _postCategoryService = postCategoryService;
        }
        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult PostCategoryList()
        //{
        
        //  var result=  _postCategoryService.PostCategoryList().ToList();
        //    ViewBag.PostCategory = result;
        //    return View();
        //}


        public IActionResult AddPostCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPostCategory(PostCategoryVM model)
        {

            return View();
        }

    }
}
