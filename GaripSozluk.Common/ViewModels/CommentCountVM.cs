using System;
using System.Collections.Generic;
using System.Text;

namespace GaripSozluk.Common.ViewModels
{
    public class CommentCountVM
    {
        public int postId { get; set; }
        public string Title { get; set; }
        public int commentCount { get; set; }
    }
}
