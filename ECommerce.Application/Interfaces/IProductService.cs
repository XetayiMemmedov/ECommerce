using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using System.Linq.Expressions;

namespace ECommerce.Application.Interfaces;

public interface IProductService
{
    ProductDto GetById(int id);
    ProductDto Get(Expression<Func<Product, bool>> predicate);
    List<ProductDto> GetAll(Expression<Func<Product, bool>>? predicate = null, bool asNoTracking = false);
    void Add(ProductCreateDto createDto);
    void Update(ProductUpdateDto updateDto);
    void Remove(int id);
}
