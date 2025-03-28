using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.EfCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ECommerce.Infrastructure.EfCore;

public class EfCoreRepository<T> : IRepository<T> where T : Entity
{
    private readonly AppDbContext _context;

    public EfCoreRepository(AppDbContext context)
    {
        _context = context;
    }

    public virtual void Add(T entity)
    {
        _context.Set<T>().Add(entity);
        _context.SaveChanges();
    }

    public virtual T Get(Expression<Func<T, bool>>? predicate = null, bool asNoTracking = false,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        IQueryable<T> queryable = _context.Set<T>();

        if (include != null)
            queryable = include(queryable);

        if (!asNoTracking)
            queryable = queryable.AsNoTracking();

        return queryable.FirstOrDefault(predicate);
    }

    public virtual List<T> GetAll(Expression<Func<T, bool>>? predicate = null, bool asNoTracking = false,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
    {
        IQueryable<T> queryable = _context.Set<T>();

        if (predicate != null)
            queryable = queryable.Where(predicate);

        if (include != null)
            queryable = include(queryable);

        if (orderBy != null)
            queryable = orderBy(queryable);

        if (!asNoTracking)
            queryable = queryable.AsNoTracking();

        return queryable.ToList();
    }

    public virtual void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
        _context.SaveChanges();
    }

    public virtual void Update(T entity)
    {
        _context.Set<T>().Update(entity);
        _context.SaveChanges();
    }

    public virtual void GiveFeedback(T entity)
    {
        _context.Set<T>().Update(entity);
        _context.SaveChanges();
    }

    public T GetById(int id)
    {
        T entity = _context.Set<T>().AsNoTracking().FirstOrDefault(x=>x.Id==id)!;
        
        return entity;
    }
}
