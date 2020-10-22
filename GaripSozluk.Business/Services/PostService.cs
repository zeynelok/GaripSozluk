using GaripSozluk.Api.Models;
using GaripSozluk.Business.Interfaces;
using GaripSozluk.Common.ViewModels;
using GaripSozluk.Data.Domain;
using GaripSozluk.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly ICommentService _commentService;
        private readonly IRatingRepository _ratingRepository;
        private readonly IPostCategoryService _postCategoryService;
        private readonly IApiService _apiService;
        public PostService(IPostRepository postRepository, IHttpContextAccessor httpContextAccessor, ICommentService commentService, IRatingRepository ratingRepository, IPostCategoryService postCategoryService, IApiService apiService)
        {
            _postRepository = postRepository;
            _httpContextAccessor = httpContextAccessor;
            _commentService = commentService;
            _ratingRepository = ratingRepository;
            _postCategoryService = postCategoryService;
            _apiService = apiService;
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



        // Post Ekleme
        public ServiceStatus AddPost(PostVM model)
        {
            var serviceStatus = new ServiceStatus();
            var httpUser = _httpContextAccessor.HttpContext.User;

            var claims = int.Parse(httpUser.Claims.ToList().Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            model.UserId = claims;
            if (model.Comment!=null)
            {
               serviceStatus= _postRepository.AddPost(model);
                return serviceStatus;
            }
            else
            {
                var isTherePost = _postRepository.Get(x => x.Title == model.Title);
                if (isTherePost == null)
                {
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
            }
            

           
          
            return serviceStatus;

        }

        // Id ye göre 1 adet post ve onun yorumlarını çek ve sayfala
        public PostRowVM GetPostById(string searchText, int id, int currentPage = 1)
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
                model.apiRowVM = _apiService.GetApi(searchText);
            }

            _postRepository.SaveChanges();
            return model;
        }

        // Aramalar 
        public SearchVM SearchPost(SearchVM model)
        {
            var query = _postRepository.GetAll();

            if (!string.IsNullOrEmpty(model.text)) //Todo: IsNullOrEmpty metoduna ek olarak IsNullOrWhiteSpace de kullanabilirsin. model.text değişkeni "   " gibi gelirse bu sorgunun içine girer. boş bir içerik sorgulanmış olur. ya yukarıda model.text değişkenini trim() yap ki boşluklar silinsin. 
            {
                query = query.Where(x => x.Title.Contains(model.text));
            }
            if (model.startDate.HasValue || model.endDate.HasValue)
            {
                if (model.startDate.HasValue && model.endDate == null)
                {
                    query = query.Where(x => x.CreateDate >= model.startDate);
                }
                else if (model.startDate == null && model.endDate.HasValue)
                {
                    query = query.Where(x => x.CreateDate <= model.endDate);
                }
                else
                {
                    query = query.Where(x => (x.CreateDate >= model.startDate && x.CreateDate <= model.endDate));
                }
            }

            if (model.ranking == 1)
            {
                var Query = query.OrderByDescending(x => x.CreateDate);
                model.posts = (List<PostVM>)Query;
            }
            else
            {
                var Query = query.OrderBy(x => x.CreateDate);
                model.posts = (List<PostVM>)Query;
            }

            return model;
        }

        // Random post çekme
        public int GetRandomPost()
        {
            //Todo: bu metotta tüm post idlerini çekmek için (select * from post)tüm postları çekmeye gerek yok. select id from post demek doğru olur o da şöyle: _postRepository.GetAll().Select(x => x.Id).ToList(); bu sanaa List<int> döner. burada hem idlere ulaşmış olursun, hem de bu listenin count sayısı ile altta atadığın postCount değişken değerine.

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

        // Post beğenme 
        public void PostRating(int ratingPostId, string type)
        {
            var httpUser = _httpContextAccessor.HttpContext.User;
            var userId = int.Parse(httpUser.Claims.ToList().Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var rating = _ratingRepository.Get(x => x.UserId == userId && x.PostId == ratingPostId);

            //Todo: STRİNG olarak type değişkeninde "like" iyi bir çözüm ama daha profesyonel kodlamak istiyorum dersen burada string ifade kullanmak yerine common katmanına bir enums klasörü açıp içinde like, unlike adında propertylere saahip bir enum yaratabilirsin. yarın bir gün sadece beğeni almış entryleri veya yorumları ön yüzde görüntülemek istersen direkt Where(x.Type == RatingEnum.Like) gibi bir kontrol ile kolayca halledersin. Kod içerisinde string ifade tutmayı mümkün oldukça azalt. 
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


        // API den gelen verilerden seçilenlerin eklenmesi
        public ServiceStatus AddPostFromApi(string[] books)
        {
            var serviceStatus = new ServiceStatus();
            var httpUser = _httpContextAccessor.HttpContext.User;
            var claims = int.Parse(httpUser.Claims.ToList().Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);

            var postCategory = _postCategoryService.GetPostCategory("Kitap");
            if (postCategory == null)
            {
                serviceStatus = _postCategoryService.AddPostCategory("Kitap");
                postCategory = _postCategoryService.GetPostCategory("Kitap");
            }


            foreach (var item in books)
            {

                var isTherePost = _postRepository.Get(x => x.Title == (item + "(Kitap)"));
                if (isTherePost == null)
                {
                    var post = new Post();
                    post.Title = item + "(Kitap)";
                    post.CreateDate = DateTime.Now;
                    post.UserId = claims;
                    post.PostCategoryId = postCategory.Id;
                    post.ViewCount = 1;
                    _postRepository.Add(post);
                    try
                    {
                        _postRepository.SaveChanges();
                        serviceStatus.Status = true;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }

            return serviceStatus;

        }

        public List<PostApiVM> GetAll()
        {

            var posts = _postRepository.GetAll().Include("User").Include("PostCategory");
            var postApi = new List<PostApiVM>();
            foreach (var item in posts)
            {
                var post = new PostApiVM();
                post.categoryId = item.PostCategoryId;
                post.Id = item.Id;
                post.Title = item.Title;
                post.UserName = item.User.UserName;
                post.viewCount = item.ViewCount;
                postApi.Add(post);
            }
            return postApi;
        }
        public Post GetPost(string title)
        {
            return _postRepository.Get(x => x.Title == title);
        }

        // Bir gün öncenin tarihiyle bir adet post oluşturma
        public void AddLogPost()
        {
            var title = DateTime.Now.AddDays(-1).ToString("dd'/'MM'/'yyyy") + " günü loglistesi(log)";
            var isTherePost = _postRepository.Get(x => x.Title == title);
            if (isTherePost == null)
            {
                var post = new Post();
                post.Title = (title);
                post.CreateDate = DateTime.Now;
                post.UserId = 9;
                post.PostCategoryId = 10;
                post.ViewCount = 1;
                _postRepository.Add(post);


                try
                {
                    _postRepository.SaveChanges();
                    var postId = GetPost(title).Id;
                    _commentService.AddLogComment(postId);

                }
                catch (Exception ex)
                {
                    var errorMessage = ex.Message;
                    throw;
                }
            }
        }

        //Bir gün öncenin en fazla istek yapılan adresleri isimli post oluşturma
        public void AddLogPostFilter()
        {
            var title = DateTime.Now.AddDays(-1).ToString("dd'/'MM'/'yyyy") + " gününde en fazla istek yapılan adresler(log-request)";
            var isTherePost = _postRepository.Get(x => x.Title == title);
            if (isTherePost == null)
            {
                var post = new Post();
                post.Title = (title);
                post.CreateDate = DateTime.Now;
                post.UserId = 9;
                post.PostCategoryId = 10;
                post.ViewCount = 1;
                _postRepository.Add(post);


                try
                {
                    _postRepository.SaveChanges();
                    var postId = GetPost(title).Id;
                    _commentService.AddLogCommentFilter(postId);

                }
                catch (Exception ex)
                {
                    var errorMessage = ex.Message;
                    throw;
                }
            }
        }


    }
}
