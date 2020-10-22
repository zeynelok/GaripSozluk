using GaripSozluk.Common.ViewModels;
using GaripSozluk.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaripSozluk.Data.Interfaces
{
    public interface IPostRepository:IBaseRepository<Post>
    {
        IQueryable<Post> GetAllByCategoryId(int id);
        ServiceStatus AddPost(PostVM model);
    }
}
