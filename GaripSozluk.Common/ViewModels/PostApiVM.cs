using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaripSozluk.Api.Models
{
    public class PostApiVM
    {
        public string Title { get; set; }
        public string UserName { get; set; }
        public int viewCount { get; set; }
        public int Id { get; set; }
        public int categoryId { get; set; }

    }
}
