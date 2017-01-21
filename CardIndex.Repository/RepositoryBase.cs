using CardIndex.Domain.Interfaces;
using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace CardIndex.Repository
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        //private IRepositoryContext _db;
        internal IRepositoryContext _context;
        internal DbSet<T> _dbSet;

        public RepositoryBase(IRepositoryContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual bool Add(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public virtual T Add(T entity)
        {
            return _dbSet.Add(entity);
        }

        public virtual IQueryable<T> All()
        {
            return _dbSet;
        }

        public virtual bool Delete(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public virtual bool Delete(T entity)
        {
            _dbSet.Remove(entity);
            return true;
        }

        public virtual IQueryable<T> FilterBy(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return All().Where(expression).AsQueryable();
        }

        public virtual T GetBy(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();   
        }

        public virtual T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual void SaveChanges()
        {
            _context.SaveChanges();
        }

        public virtual bool Update(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public virtual T Update(T entity)
        {
            _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            return entity;
        }
    }
}
