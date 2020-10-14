using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaripSozluk.Business.Interfaces;
using GaripSozluk.Common.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GaripSozluk.WebApp.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
       
        [Authorize]
        public IActionResult AddComment(int postId,int selectedCategoryId)
        {
            ViewBag.selectedCategoryId = selectedCategoryId;
            ViewBag.selectedPostId = postId;
            return View();
        }

        [HttpPost]
        public IActionResult AddComment(CommentVM model,int selectedCategoryId,int postId= 1)
        {
            if (ModelState.IsValid)
            {
                var serviceStatus = _commentService.AddComment(model,postId);
                if (serviceStatus.Status)
                {
                    return Redirect(Url.Action("Index", "Home",new { postId=postId, selectedCategoryId = selectedCategoryId}));
                }
            }
            return View();
        }
    }
}
