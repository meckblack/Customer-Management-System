using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CustomerManagementSystem.Data;
using CustomerManagementSystem.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CustomerManagementSystem.Services.Concrete
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual async Task Add(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry(entity);
            await _context.Set<T>().AddAsync(entity);
        }

        public virtual IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties) query = query.Include(includeProperty);
            return query.AsEnumerable();
        }

        public virtual async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public virtual async Task<int> Count()
        {
            return await _context.Set<T>().CountAsync();
        }

        public virtual void Delete(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry(entity);
            dbEntityEntry.State = EntityState.Detached;
        }

        public virtual void DeleteWhere(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> entities = _context.Set<T>().Where(predicate);

            foreach (var entity in entities) _context.Entry(entity).State = EntityState.Deleted;
        }

        public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsEnumerable();
        }

        public virtual T GetSingle(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public virtual T GetSingle(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties) query = query.Include(includeProperty);

            return query.Where(predicate).FirstOrDefault();
        }

        public virtual void Update(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual TType GetSingleByFields<TType>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, TType>> select)
        {
            return _context.Set<T>().Where(predicate).Select(select).FirstOrDefault();
        }

        public virtual async Task<int> CountWhere(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).CountAsync();
        }

        public virtual async Task<decimal> SumAsync<TSource>(IQueryable<TSource> source,
            Expression<Func<TSource, decimal>> selector)
        {
            return await source.Select(selector).SumAsync();
        }
    }
}