using System.Linq.Expressions;
using AutoMapper;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Query;

namespace ECommerce.Application.Services;

public class CrudManager<TEntity, TDto, TCreateDto, TUpdateDto> : ICrudService<TEntity, TDto, TCreateDto, TUpdateDto> where TEntity : Entity
{
    private readonly IRepository<TEntity> _repository;
    private readonly IMapper _mapper;

    public CrudManager(IRepository<TEntity> repository)
    {
        _repository = repository;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    public virtual void Add(TCreateDto createDto)
    {
        TEntity entity = _mapper.Map<TEntity>(createDto);

        _repository.Add(entity);
    }

    public virtual TDto Get(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
    {
        var entity = _repository.Get(predicate, asNoTracking, include);
        var dto = _mapper.Map<TDto>(entity);

        return dto;
    }

    public virtual List<TDto> GetAll(Expression<Func<TEntity, bool>>? predicate = null, bool asNoTracking = false, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
    {
        var entities = _repository.GetAll(predicate, asNoTracking, include, orderBy);
        var dtos = _mapper.Map<List<TDto>>(entities);

        return dtos;
    }

    public virtual TDto GetById(int id)
    {
        var entity = _repository.GetById(id);
        var dto = _mapper.Map<TDto>(entity);

        return dto;
    }

    public virtual void Remove(int id)
    {
        var entity = _repository.GetById(id);

        if (entity == null)
        {
            throw new Exception("Entity not found");
        }

        _repository.Remove(entity);
    }

    public virtual void Update(TUpdateDto updateDto)
    {
        var entity = _mapper.Map<TEntity>(updateDto);

        _repository.Update(entity);
    }
}
