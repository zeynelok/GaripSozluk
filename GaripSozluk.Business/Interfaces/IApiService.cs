using GaripSozluk.Api.Models;
using GaripSozluk.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaripSozluk.Business.Interfaces
{
   public interface IApiService
    {
        ApiRowVM GetApi(string searchText,int? option=null);
        List<PostApiVM> GetPostFromMyApi();
    }
}
