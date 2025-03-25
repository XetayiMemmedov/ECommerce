using System.Linq.Expressions;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.EfCore.Context;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.EfCore;

public class OrderRepository : EfCoreRepository<Order>, IOrderRepository
{
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    public override Order Get(Expression<Func<Order, bool>> predicate)
    {
        IQueryable<Order> query = _context.Set<Order>();

        query = query
            .Include(o=>o.User)
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Product);
        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return query
            .OrderByDescending(o => o.OrderDate)
            .FirstOrDefault();
    }
    public override List<Order> GetAll(Expression<Func<Order, bool>>? predicate=null, bool asNoTracking = false)
    {
        IQueryable<Order> query = _context.Set<Order>();

        query = query
            .Include(o=>o.User)
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Product)
            .ThenInclude(oip=>oip.Category);
        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return query.ToList();
    }
}