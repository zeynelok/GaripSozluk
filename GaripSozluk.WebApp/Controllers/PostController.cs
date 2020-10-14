using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaripSozluk.Business.Interfaces;
using GaripSozluk.Common.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace GaripSozluk.WebApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostCategoryService _postCategoryService;
        private readonly IPostService _postService;
        private readonly ILogger<PostController> _logger;
        public PostController(IPostService postService, ILogger<PostController> logger, IPostCategoryService postCategoryService)
        {
            _postCategoryService = postCategoryService;
            _postService = postService;
            _logger = logger;
        }
        //public IActionResult PostList(int id)
        //{

        //    var result = _postService.GetAllByCategoryId(id);
        //    ViewBag.Post = result;
        //    return View();
        //   // return Redirect(Url.Action("Index","Home"));
        //}
        [Authorize]
        public IActionResult AddPost(int selectedCategoryId = 1)
        {
            ViewBag.PostCategory = _postCategoryService.PostCategoryList(selectedCategoryId);
            ViewBag.PostCategoryName = _postCategoryService.GetPostCategory(selectedCategoryId);
            ViewBag.Post = _postService.GetAllByCategoryId(selectedCategoryId).ToList();

            return View();
        }

        [HttpPost]
        public IActionResult AddPost(PostVM model)
        {
            if (ModelState.IsValid)
            {
                var serviceStatus = _postService.AddPost(model);
                if (serviceStatus.Status)
                {
                    return Redirect(Url.Action("Index", "Home", new { selectedCategoryId = model.PostCategoryId }));
                }
            }

            ViewBag.PostCategory = _postCategoryService.PostCategoryList(model.PostCategoryId);
            ViewBag.PostCategoryName = _postCategoryService.GetPostCategory(model.PostCategoryId);
            ViewBag.Post = _postService.GetAllByCategoryId(model.PostCategoryId).ToList();

            return View(model);
            //return Redirect(Url.Action("AddPost","Post"));
        }

    }
}
