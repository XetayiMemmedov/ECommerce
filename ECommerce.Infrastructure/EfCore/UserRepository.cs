using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.EfCore.Context;

namespace ECommerce.Infrastructure.EfCore;

public class UserRepository : EfCoreRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }
   
}
