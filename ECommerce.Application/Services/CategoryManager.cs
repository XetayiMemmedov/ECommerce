using ECommerce.Application.DTOs;
using ECommerce.Application.Extensions;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using System.Linq.Expressions;

namespace ECommerce.Application.Services;

public class CategoryManager : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryManager(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public void Add(CategoryCreateDto createDto)
    {
        var category = createDto.ToCategory();

        _repository.Add(category);
    }

    public CategoryDto Get(Expression<Func<Category, bool>> predicate)
    {
        var category = _repository.Get(predicate);

        var categoryDto = category.ToCategoryDto();

        return categoryDto;
    }

    public List<CategoryDto> GetAll(Expression<Func<Category, bool>>? predicate = null, bool asNoTracking = false)
    {
        var categories = _repository.GetAll(predicate, asNoTracking);

        var categoryDtoList = new List<CategoryDto>();

        foreach (var item in categories)
        {
            categoryDtoList.Add(new CategoryDto
            {
                Id = item.Id,
                Name = item.Name
            });
        }

        return categoryDtoList;
    }

    public CategoryDto GetById(int id)
    {
        var category = _repository.GetById(id);

        var categoryDto = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        };

        return categoryDto;
    }

    public void Remove(int id)
    {
        var existEntity = _repository.GetById(id);

        if (existEntity == null) throw new Exception("Not found");

        _repository.Remove(existEntity);
    }

    public void Update(CategoryUpdateDto updateDto)
    {
        var category = new Category
        {
            Id = updateDto.Id,
            Name = updateDto.Name
        };

        _repository.Update(category);
    }
}
