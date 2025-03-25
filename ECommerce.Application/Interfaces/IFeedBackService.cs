using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using System.Linq.Expressions;

namespace ECommerce.Application.Interfaces;

public interface IFeedBackService
{
    FeedBackDto GetById(int id);
    FeedBackDto Get(Expression<Func<FeedBack, bool>> predicate);
    List<FeedBackDto> GetAll(Expression<Func<FeedBack, bool>>? predicate = null, bool asNoTracking = false);
    void Add(FeedBackCreateDto createDto);
    void Remove(int id);
}
