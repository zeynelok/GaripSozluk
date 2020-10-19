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
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set < TEntity>();
        }
        public TEntity Add(TEntity entity)
        {
            var entityEntry = _dbSet.Add(entity);
            return entityEntry.Entity;
        }

        public TEntity Get(Expression<Func<TEntity, bool>> expression)
        {
            var result= _dbSet.Where(expression).FirstOrDefault();
            return result;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsQueryable(); 
        }
        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression)
        {
            var result = _dbSet.Where(expression).AsQueryable();
            return result;
        }


        public TEntity Update(TEntity entity)
        {
            var entityEntry = _dbSet.Update(entity);
            return entityEntry.Entity;

        }
        public TEntity Remove(TEntity entity)
        {
            var entityEntry = _dbSet.Remove(entity);
            return entityEntry.Entity;

        }
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
