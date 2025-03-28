using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using System.Linq.Expressions;

namespace ECommerce.Application.Interfaces;

public interface IProductService:ICrudService<Product,ProductDto,ProductCreateDto,ProductUpdateDto>
{
}
