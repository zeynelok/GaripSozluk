using GaripSozluk.Api.Models;
using GaripSozluk.Common.ViewModels;
using GaripSozluk.Data.Domain;
using Senparc.Weixin.MP.Entities.Request;
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
        PostRowVM GetPostById(string searchText, int id, int currentPage);
        SearchVM SearchPost(SearchVM model);
        int GetRandomPost();
        ServiceStatus AddPost(PostVM model);
        void PostRating(int ratingPostId, string type);
        ServiceStatus AddPostFromApi(string[] books);
        List<PostApiVM> GetAll();
        void AddLogPost();
        Post GetPost(string title);
        void AddLogPostFilter();
    }
}
