using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.EfCore.Context;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.EfCore
{
    public class FeedBackRepository : EfCoreRepository<FeedBack>, IFeedBackRepository
    {
        private readonly AppDbContext _context;

        public FeedBackRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public override FeedBack Get(Expression<Func<FeedBack, bool>> predicate)
        {
            IQueryable<FeedBack> query = _context.Set<FeedBack>();

            query = query
                .Include(f => f.User)
                .Include(f => f.UserFeedBacks)
                .ThenInclude(uf => uf.Product);
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query.FirstOrDefault();
        }
    }
}
