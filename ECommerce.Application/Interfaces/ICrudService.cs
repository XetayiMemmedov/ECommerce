using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace ECommerce.Application.Interfaces;

public interface ICrudService<TEntity, TDto, TCreateDto, TUpdateDto>
{
    TDto GetById(int id);
    TDto Get(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false,
    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null);
    List<TDto> GetAll(Expression<Func<TEntity, bool>>? predicate = null, bool asNoTracking = false,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);
    void Add(TCreateDto createDto);
    void Update(TUpdateDto updateDto);
    void Remove(int id);
}
