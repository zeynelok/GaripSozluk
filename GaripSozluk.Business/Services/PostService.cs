using GaripSozluk.Business.Interfaces;
using GaripSozluk.Common.ViewModels;
using GaripSozluk.Data.Domain;
using GaripSozluk.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace GaripSozluk.Business.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<User> _signInManager;
        private readonly ICommentService _commentService;
        public PostService(IPostRepository postRepository, IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager, ICommentService commentService)
        {
            _postRepository = postRepository;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _commentService = commentService;
        }

        public List<PostRowVM> GetAllByCategoryId(int selectedCategoryId)
        {
            
            var list = new List<PostRowVM>();
            _postRepository.GetAllByCategoryId(selectedCategoryId).ToList().ForEach(x =>
            {
                var item = new PostRowVM();
                item.postId = x.Id;
                item.Title = x.Title;
                item.CommentCount = _commentService.GetAllByPostId(x.Id).Count();
                list.Add(item);
            });
            return list;
        }
        //public List<CommentCountVM> GetAllByCategoryIdCommentCount(int selectedCategoryId)
        //{
        //    var comment = new List<CommentCountVM>();


        //    var asd = _postRepository.GetAllByCategoryId(selectedCategoryId);
        //    foreach (var item in asd)
        //    {
        //        comment.Add(new CommentCountVM()

        //        {
        //            commentCount = _commentService.GetAllByPostId(item.Id).Count(),
        //            postId = item.Id,
        //            Title = item.Title

        //        });
        //    }
        //    return comment;
        //}

        public Post Get(Expression<Func<Post, bool>> expression)
        {
            return _postRepository.Get(expression);
        }

        public ServiceStatus AddPost(PostVM model)
        {
            var serviceStatus = new ServiceStatus();

            var httpUser = _httpContextAccessor.HttpContext.User;

            var claims = int.Parse(httpUser.Claims.ToList().Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);

            //var user =_signInManager.UserManager.GetUserAsync(httpUser).Result;
            var post = new Post();
            post.Title = model.Title;
            post.CreateDate = DateTime.Now;
            post.UserId = claims;
            post.PostCategoryId = model.PostCategoryId;
            post.ViewCount =1;
            _postRepository.Add(post);

            try
            {
                _postRepository.SaveChanges();
                serviceStatus.Status = true;
                return serviceStatus;

            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                throw;
            }

        }

        public PostRowVM GetPostById(int id, int currentPage = 1)
        {
            //var httpUser = _httpContextAccessor.HttpContext.User;
            //var userId = int.Parse(httpUser.Claims.ToList().Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            //var blockedUsers=_
            var postCount = _commentService.GetAllByPostId(id).Count();
            var postSize = 8;
            var pageCount = (postCount / postSize) + (postCount % postSize > 0 ? 1 : 0);
            var model = new PostRowVM();
            model.CurrentPage = currentPage;
            model.PageCount = pageCount;
            model.PreviousPage = (currentPage - 1) > 0 ? currentPage - 1 : pageCount;
            model.NextPage = currentPage + 1 > pageCount ? 1 : currentPage + 1;

            var post = _postRepository.Get(x => x.Id == id);

            if (post != null)
            {
                (post.ViewCount) = (post.ViewCount) + 1;
                model.ViewCount = (post.ViewCount);
                model.Title = post.Title;
                model.postId = post.Id;
                model.CreateDate = post.CreateDate;
                model.Comments = _commentService.GetAllByPostId(post.Id).Skip((currentPage - 1) * postSize).Take(postSize).ToList();
            }

            _postRepository.SaveChanges();
            return model;
        }

        public SearchVM SearchPost(SearchVM model)

        {
            //model.posts = _postRepository.GetAll(x => x.Title.Contains(model.text)).ToList();

            var query = _postRepository.GetAll().Where(x => true);
            if (!string.IsNullOrEmpty(model.text))
            {
                query = query.Where(x => x.Title.Contains(model.text));
            }
            if (model.startDate.HasValue || model.endDate.HasValue)
            {
                if (model.startDate.HasValue && model.endDate == null)
                {
                    query = query.Where(x => x.CreateDate > model.startDate);
                }
                else if (model.startDate == null && model.endDate.HasValue)
                {
                    query = query.Where(x => x.CreateDate > model.endDate);
                }
                else
                {
                    query = query.Where(x => (x.CreateDate >= model.startDate && x.CreateDate <= model.endDate));
                }

            }

            if (model.ranking == 1)
            {
                var Query = query.OrderByDescending(x => x.CreateDate);

                model.posts = Query.ToList();

            }
            else
            {
                var Query = query.OrderBy(x => x.CreateDate);
                model.posts = Query.ToList();
            }

            return model;
        }

        //public SearchVM DetailSearchPost(SearchVM model)
        //{
        //    var query = _postRepository.GetAll().Where(x => true);
        //    if (!string.IsNullOrEmpty(model.text))
        //    {
        //        query = query.Where(x => x.Title.Contains(model.text));
        //    }
        //    if (model.startDate.HasValue || model.endDate.HasValue)
        //    {
        //        if (model.startDate.HasValue && model.endDate == null)
        //        {
        //            query = query.Where(x => x.CreateDate > model.startDate);
        //        }
        //        else if (model.startDate == null && model.endDate.HasValue)
        //        {
        //            query = query.Where(x => x.CreateDate > model.endDate);
        //        }
        //        else
        //        {
        //            query = query.Where(x => (x.CreateDate >= model.startDate && x.CreateDate <= model.endDate));
        //        }

        //    }

        //    if (model.ranking == 1)
        //    {
        //        var Query = query.OrderByDescending(x => x.CreateDate);

        //        model.posts = Query.ToList();

        //    }
        //    else
        //    {
        //        var Query = query.OrderBy(x => x.CreateDate);
        //        model.posts = Query.ToList();
        //    }

        //    return model;
        //}
    }
}
