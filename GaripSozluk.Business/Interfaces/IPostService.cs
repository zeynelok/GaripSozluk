using GaripSozluk.Common.ViewModels;
using GaripSozluk.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GaripSozluk.Business.Interfaces
{
    public interface IPostService
    {
        List<PostRowVM> GetAllByCategoryId(int selectedCategoryId);
        PostRowVM GetPostById(int id,int currentPage);
        SearchVM SearchPost(SearchVM model);
        //SearchVM DetailSearchPost(SearchVM model);
        //List<CommentCountVM> GetAllByCategoryIdCommentCount(int selectedCategoryId);

        ServiceStatus AddPost(PostVM model);



    }
}
