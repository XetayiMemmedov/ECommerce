using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using System.Linq.Expressions;

namespace ECommerce.Application.Interfaces;

public interface ICategoryService:ICrudService<Category,CategoryDto,CategoryCreateDto,CategoryUpdateDto>
{
    
}
