using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using System.Linq.Expressions;

namespace ECommerce.Application.Services;

public class CategoryManager : CrudManager<Category, CategoryDto, CategoryCreateDto, CategoryUpdateDto>, ICategoryService
{
    public CategoryManager(IRepository<Category> repository) : base(repository)
    {
    }
}
