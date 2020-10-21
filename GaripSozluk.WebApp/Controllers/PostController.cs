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
      
        //Post Ekleme
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
        }

        //Random post çekme
        public IActionResult GetRandomPost()
        {

            var postId = _postService.GetRandomPost();

            return Redirect(Url.Action("Index", "Home", new { postId = postId }));
        }

        //Post beğenme
        [Authorize]
        public IActionResult PostRating(int ratingPostId, string type)
        {
            //Todo: rating eklenip eklenmeme durumunun veritabanı tarafında başarılı olup olmadığını kontrol etmek isteyebilirsin. bunun için PostRating metodu void yerine başarılı veya başarısız diye bir yanıt dönebilir.
            _postService.PostRating(ratingPostId, type);

            return Redirect(Url.Action("Index", "Home", new { postId = ratingPostId }));
        }

    }
}
