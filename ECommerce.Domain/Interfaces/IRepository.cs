using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ECommerce.Domain.Interfaces;

public interface IRepository<T> where T : Entity
{
    T GetById(int id);
    T Get(Expression<Func<T, bool>> predicate, bool asNoTracking = false,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    List<T> GetAll(Expression<Func<T, bool>>? predicate = null, bool asNoTracking = false,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);

    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
}