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

        public PostCategoryVM AddPostCategory(PostCategoryVM model)
        {
            throw new NotImplementedException();
        }

        public PostCategory GetPostCategory(int selectedCategoryId)
        {
            return _postCategoryRepository.Get(x=>x.Id==selectedCategoryId);
        }

        public List<SelectListItem> PostCategoryList(int selectedCategoryId=0)
        {
            var hebele = new List<SelectListItem>();
            var postCategories = _postCategoryRepository.GetAll();

            foreach (var item in postCategories)
            {
                var list = new SelectListItem();
                if (item.Id == selectedCategoryId)
                {
                    list.Selected = true;
                }

                list.Text = item.Title;
                list.Value = item.Id.ToString();
                hebele.Add(list);
            }
            return hebele;
            //return  _postCategoryRepository.GetAll();
        }

        //public IQueryable<PostCategory> PostCategoryList()
        //{
        //    return _postCategoryRepository.GetAll();
        //}

        public PostCategoryVM UpdatePostCategory(PostCategoryVM model)
        {
            throw new NotImplementedException();
        }
    }
}
