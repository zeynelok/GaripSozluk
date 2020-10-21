using GaripSozluk.Business.Interfaces;
using GaripSozluk.Common.ViewModels;
using GaripSozluk.Data.Domain;
using GaripSozluk.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
//using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace GaripSozluk.Business.Services
{
    public class PostCategoryService : IPostCategoryService
    {
        private readonly IPostCategoryRepository _postCategoryRepository;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PostCategoryService(IPostCategoryRepository postCategoryRepository, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _postCategoryRepository = postCategoryRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public ServiceStatus AddPostCategory(string categoryName)
        {
            var serviceStatus = new ServiceStatus();
            var postCategory = new PostCategory();
            postCategory.Title = categoryName;
            postCategory.CreateDate = DateTime.Now;
            _postCategoryRepository.Add(postCategory);
            try
            {
                _postCategoryRepository.SaveChanges();
                serviceStatus.Status = true;
                return serviceStatus;
            }
            catch (Exception)
            {

                throw;
            }
        }



        //seçilen categoriyi veritabanından alıyoruz postların üstünde yazmak için
        public PostCategory GetPostCategory(int selectedCategoryId)
        {
            return _postCategoryRepository.Get(x => x.Id == selectedCategoryId);
        }
        public PostCategory GetPostCategory(string categoryName)
        {
            return _postCategoryRepository.Get(x => x.Title == categoryName);
        }



        //kategori listesi alıyoruz ve selectbox için seçili olanı çekiyoruz
        public List<SelectListItem> PostCategoryList(int selectedCategoryId)
        {
            var isAdmin = false;

            var httpUser = _httpContextAccessor.HttpContext.User;
            if (httpUser.Claims.Any())
            {
                var user = _userManager.GetUserAsync(httpUser).Result;
                var roles = _userManager.GetRolesAsync(user).Result;
                if (roles.Contains("Admin"))
                {
                    isAdmin = true;
                }
            }

            var list = new List<SelectListItem>();
            var postCategories = _postCategoryRepository.GetAll();
            if (isAdmin == false)
            {
                postCategories = postCategories.Where(x => x.Title != "Log");
            }
            foreach (var item in postCategories)
            {
                var category = new SelectListItem();
                if (item.Id == selectedCategoryId)
                {
                    category.Selected = true;
                }

                category.Text = item.Title;
                category.Value = item.Id.ToString();
                list.Add(category);
            }
            return list;
        }


    }
}
