using GaripSozluk.Common.ViewModels;
using GaripSozluk.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GaripSozluk.Business.Interfaces
{
    public interface IPostCategoryService
    {
        //IQueryable<PostCategory> PostCategoryList();
        List<SelectListItem> PostCategoryList(int selectedCategoryId = 0);
        PostCategory GetPostCategory(int selectedCategoryId);
      
        PostCategoryVM AddPostCategory(PostCategoryVM model);
        PostCategoryVM UpdatePostCategory(PostCategoryVM model);
    }
}
