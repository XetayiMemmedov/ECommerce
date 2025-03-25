using System.Linq.Expressions;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.EfCore.Context;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.EfCore;

public class ProductRepository : EfCoreRepository<Product>, IProductRepository
{
    private readonly AppDbContext _context;
    
    public ProductRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    public override List<Product> GetAll(Expression<Func<Product, bool>>? predicate = null, bool asNoTracking = false)
    {
        IQueryable<Product> query = _context.Set<Product>();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        query = query.Include(p => p.Category);

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return query.ToList();
    }
    public override Product Get(Expression<Func<Product, bool>> predicate)
    {
        IQueryable<Product> query = _context.Set<Product>();

        query = query.Include(p => p.Category);

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return query.FirstOrDefault();
    }
}