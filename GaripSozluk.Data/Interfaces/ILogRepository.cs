using GaripSozluk.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GaripSozluk.Data.Interfaces
{
   public interface ILogRepository
    {
        Log Add(Log entity);
        IQueryable<Log> GetAll();
        IQueryable<Log> GetAll(Expression<Func<Log, bool>> expression);
        Log Get(Expression<Func<Log, bool>> expression);
        int SaveChanges();

    }
}
