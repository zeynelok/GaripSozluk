using GaripSozluk.Common.ViewModels;
using GaripSozluk.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GaripSozluk.Business.Interfaces
{
   public interface ICommentService
    {
        List<CommentRowVM> GetAllByPostId(int postId);

        ServiceStatus AddComment(CommentVM model,int postId);

    }
}
