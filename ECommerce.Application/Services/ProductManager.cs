using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Application.Services;

public class ProductManager : CrudManager<Product, ProductDto, ProductCreateDto, ProductUpdateDto>, IProductService
{
    public ProductManager(IRepository<Product> repository) : base(repository)
    {
    }
}
