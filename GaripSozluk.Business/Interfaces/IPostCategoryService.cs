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
        List<SelectListItem> PostCategoryList(int selectedCategoryId);
        PostCategory GetPostCategory(int selectedCategoryId);
        ServiceStatus AddPostCategory(string categoryName);
        PostCategory GetPostCategory(string categoryName);
    }
}
