using GaripSozluk.Data.Domain;
using GaripSozluk.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaripSozluk.Data.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        private readonly GaripSozlukDbContext _context;

        public CommentRepository(GaripSozlukDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
