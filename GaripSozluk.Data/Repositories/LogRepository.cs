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
    public class LogRepository:ILogRepository
    {
        private readonly GaripSozlukDbContextLog _context;
        private readonly DbSet<Log> _dbSet;
        public LogRepository(GaripSozlukDbContextLog context)
        {
            _context = context;
            _dbSet = _context.Set<Log>();
        }
        public Log Add(Log entity)
        {
            var entityEntry = _dbSet.Add(entity);
            return entityEntry.Entity;
        }

        public Log Get(Expression<Func<Log, bool>> expression)
        {
            var result = _dbSet.Where(expression).FirstOrDefault();
            return result;
        }

        public IQueryable<Log> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<Log> GetAll(Expression<Func<Log, bool>> expression)
        {
            var result = _dbSet.Where(expression).AsQueryable();
            return result;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
