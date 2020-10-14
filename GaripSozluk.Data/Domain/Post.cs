using System;
using System.Collections.Generic;
using System.Text;

namespace GaripSozluk.Data.Domain
{
    public class Post : BaseEntity
    {

        public string Title { get; set; }
        public int ViewCount { get; set; }
        public int UserId { get; set; }
        public int PostCategoryId { get; set; }

        public virtual PostCategory PostCategory { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }



    }
}
