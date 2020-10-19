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
    public class LogRepository: BaseRepository<Log>,ILogRepository
    {
        private readonly GaripSozlukDbContextLog _context;
        public LogRepository(GaripSozlukDbContextLog context):base(context)
        {
            _context = context;
        }
       
    }
}
