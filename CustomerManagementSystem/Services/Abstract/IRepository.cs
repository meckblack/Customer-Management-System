using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Services.Abstract
{
    public interface IRepository<T> where T : class, new()
    {
        IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> GetAll();
        Task<int> Count();
        T GetSingle(Expression<Func<T, bool>> predicate);
        T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteWhere(Expression<Func<T, bool>> predicate);
        Task CommitAsync();
        TType GetSingleByFields<TType>(Expression<Func<T, bool>> predicate, Expression<Func<T, TType>> select);
        Task<int> CountWhere(Expression<Func<T, bool>> predicate);
        Task<decimal> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector);
    }
}