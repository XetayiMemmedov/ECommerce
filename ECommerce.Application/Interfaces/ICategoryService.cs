using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using System.Linq.Expressions;

namespace ECommerce.Application.Interfaces;

public interface ICategoryService
{
    CategoryDto GetById(int id);
    CategoryDto Get(Expression<Func<Category, bool>> predicate);
    List<CategoryDto> GetAll(Expression<Func<Category, bool>>? predicate = null, bool asNoTracking = false);
    void Add(CategoryCreateDto createDto);
    void Update(CategoryUpdateDto updateDto);
    void Remove(int id);
}
