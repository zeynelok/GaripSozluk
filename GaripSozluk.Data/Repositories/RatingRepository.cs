using GaripSozluk.Data.Domain;
using GaripSozluk.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GaripSozluk.Data.Repositories
{
    public class RatingRepository : BaseRepository<Rating>,IRatingRepository
    {
        private readonly GaripSozlukDbContext _context;
        public RatingRepository(GaripSozlukDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
