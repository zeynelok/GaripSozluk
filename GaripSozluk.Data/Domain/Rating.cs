using System;
using System.Collections.Generic;
using System.Text;

namespace GaripSozluk.Data.Domain
{
    public class Rating:BaseEntity
    {

        public int PostId { get; set; }
        public int UserId { get; set; }
        public bool? Isliked { get; set; }
        public bool? IsDisliked { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }

    }
}
