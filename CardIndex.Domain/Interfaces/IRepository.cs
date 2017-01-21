using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CardIndex.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> All();
        TEntity GetById(int id);
        TEntity GetBy(Expression<Func<TEntity, bool>> expression);
        IQueryable<TEntity> FilterBy(Expression<Func<TEntity, bool>> expression);

        TEntity Add(TEntity entity);
        bool Add(IEnumerable<TEntity> items);
        TEntity Update(TEntity entity);
        bool Update(IEnumerable<TEntity> entities);
        bool Delete(TEntity entity);
        bool Delete(IEnumerable<TEntity> entities);

        void SaveChanges();
    }
}
