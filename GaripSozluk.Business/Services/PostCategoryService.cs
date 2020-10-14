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




namespace GaripSozluk.Business.Services
{
    public class PostCategoryService : IPostCategoryService
    {
        private readonly IPostCategoryRepository _postCategoryRepository;
        public PostCategoryService(IPostCategoryRepository postCategoryRepository)
        {
            _postCategoryRepository = postCategoryRepository;       
        }

        //seçilen categoriyi veritabanından alıyoruz postların üstünde yazmak için
        public PostCategory GetPostCategory(int selectedCategoryId)
        {
            return _postCategoryRepository.Get(x=>x.Id==selectedCategoryId);
        }


        //kategori listesi alıyoruz selectbox için 
        public List<SelectListItem> PostCategoryList(int selectedCategoryId)
        {
            var list = new List<SelectListItem>();
            var postCategories = _postCategoryRepository.GetAll();

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
