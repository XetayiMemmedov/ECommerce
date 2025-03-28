using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services
{
    public class FeedBackManager : CrudManager<FeedBack, FeedBackDto, FeedBackCreateDto, FeedBackUpdateDto>, IFeedBackService
    {
        public FeedBackManager(IRepository<FeedBack> repository) : base(repository)
        {
        }
    }
}
