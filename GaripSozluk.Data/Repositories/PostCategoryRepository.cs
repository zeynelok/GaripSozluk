using GaripSozluk.Data.Domain;
using GaripSozluk.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaripSozluk.Data.Repositories
{
    public class PostCategoryRepository : BaseRepository<PostCategory>, IPostCategoryRepository
    {
        private readonly GaripSozlukDbContext _context;
        public PostCategoryRepository(GaripSozlukDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
