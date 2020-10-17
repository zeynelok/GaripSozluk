using GaripSozluk.Data.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaripSozluk.Common.ViewModels
{
    public class PostRowVM
    {
        public int postId { get; set; }
        public string Title { get; set; }
        public List<CommentRowVM> Comments { get; set; }
        public int CommentCount { get; set; }
        public DateTime CreateDate { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public int ViewCount { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public int PreviousPage { get; set; }
        public int NextPage { get; set; }

        public ApiRowVM apiRowVM { get; set; }
    }
}
