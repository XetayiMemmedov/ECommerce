using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.EfCore.Context;

namespace ECommerce.Infrastructure.EfCore;

public class FeedBackRepository : EfCoreRepository<FeedBack>, IFeedBackRepository
{
    private readonly AppDbContext _context;

    public FeedBackRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
   
}
