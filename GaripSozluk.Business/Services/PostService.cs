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
        private readonly IBlockedUserService _blockedUserService;
        private readonly IRatingRepository _ratingRepository;
        public PostService(IPostRepository postRepository, IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager, ICommentService commentService, IBlockedUserService blockedUserService, IRatingRepository ratingRepository)
        {
            _postRepository = postRepository;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _commentService = commentService;
            _blockedUserService = blockedUserService;
            _ratingRepository = ratingRepository;
        }

        //Seçili kategoriye göre postları çekme
        public List<PostRowVM> GetAllByCategoryId(int selectedCategoryId)
        {
            var list = new List<PostRowVM>();
            _postRepository.GetAllByCategoryId(selectedCategoryId).ToList()
               .ForEach(x =>
            {
                var item = new PostRowVM();
                item.postId = x.Id;
                item.Title = x.Title;
                item.CommentCount = _commentService.GetAllByPostId(x.Id).Count();
                list.Add(item);
            });
            return list;
        }

        public Post Get(Expression<Func<Post, bool>> expression)
        {
            return _postRepository.Get(expression);
        }

        // Post Ekleme
        public ServiceStatus AddPost(PostVM model)
        {
            var serviceStatus = new ServiceStatus();

            var httpUser = _httpContextAccessor.HttpContext.User;

            var claims = int.Parse(httpUser.Claims.ToList().Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);

            var post = new Post();
            post.Title = model.Title;
            post.CreateDate = DateTime.Now;
            post.UserId = claims;
            post.PostCategoryId = model.PostCategoryId;
            post.ViewCount = 1;
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

        // Id ye göre 1 adet post ve onun yorumlarını çek ve sayfala
        public PostRowVM GetPostById(int id, int currentPage = 1)
        {

            var commentCount = _commentService.GetAllByPostId(id).Count();
            var commentSize = 8;
            var pageCount = (commentCount / commentSize) + (commentCount % commentSize > 0 ? 1 : 0);
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
                model.LikeCount = _ratingRepository.GetAll(x => x.Isliked == true && x.PostId == post.Id).Count();
                model.DislikeCount = _ratingRepository.GetAll(x => x.IsDisliked == true && x.PostId == post.Id).Count();
                model.Comments = _commentService.GetAllByPostId(post.Id).Skip((currentPage - 1) * commentSize).Take(commentSize).ToList();
            }

            _postRepository.SaveChanges();
            return model;
        }

        // Aramalar 
        public SearchVM SearchPost(SearchVM model)
        {
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

        public int GetRandomPost()
        {
            List<int> postIds = new List<int>();

            var posts = _postRepository.GetAll();
            foreach (var item in posts)
            {
                postIds.Add(item.Id);
            }
            var postCount = posts.Count();
            var rand = new Random();

            return postIds[rand.Next(0, postCount)];

        }


        public void PostRating(int ratingPostId, string type)
        {
            var httpUser = _httpContextAccessor.HttpContext.User;
            var userId = int.Parse(httpUser.Claims.ToList().Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var rating = _ratingRepository.Get(x => x.UserId == userId && x.PostId == ratingPostId);

            if (type == "like") // type like mı
            {
                if (rating != null) //veritabanında kayıt var
                {

                    if (rating.Isliked != null && rating.Isliked == true)
                    {
                        _ratingRepository.Remove(rating);
                    }
                    else
                    {
                        rating.Isliked = true;
                        rating.IsDisliked = null;
                        rating.UpdateDate = DateTime.Now;
                    }
                }
                else  //veritabanında kayıt yok
                {
                    rating = new Rating();
                    rating.PostId = ratingPostId;
                    rating.CreateDate = DateTime.Now;
                    rating.UserId = userId;
                    rating.Isliked = true;
                    _ratingRepository.Add(rating);

                }
            }
            else //type dislike 
            {
                if (rating != null) //veritabanında kayıt var
                {

                    if (rating.IsDisliked != null && rating.IsDisliked == true)
                    {
                        _ratingRepository.Remove(rating);
                    }
                    else
                    {
                        rating.Isliked = null;
                        rating.IsDisliked = true;
                        rating.UpdateDate = DateTime.Now;
                    }
                }
                else  //veritabanında kayıt yok
                {
                    rating = new Rating();
                    rating.PostId = ratingPostId;
                    rating.CreateDate = DateTime.Now;
                    rating.UserId = userId;
                    rating.IsDisliked = true;
                    _ratingRepository.Add(rating);

                }
            }

            try
            {
                _ratingRepository.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}
