using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.DTOs;
using ECommerce.Application.Extensions;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Application.Services
{
    public class ProductManager : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductManager(IProductRepository repository)
        {
            _repository = repository;
        }

        public void Add(ProductCreateDto createDto)
        {
            var product = createDto.ToProduct();

            _repository.Add(product);
        }

        public ProductDto Get(Expression<Func<Product, bool>> predicate)
        {
            var product = _repository.Get(predicate);

            var productDto = product.ToProductDto();

            return productDto;
        }

        public List<ProductDto> GetAll(Expression<Func<Product, bool>>? predicate = null, bool asNoTracking = false)
        {
            var products = _repository.GetAll(predicate, asNoTracking);

            var productDtoList = new List<ProductDto>();

            foreach (var item in products)
            {
                productDtoList.Add(new ProductDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    CategoryName = item.Category.Name
                });
            }

            return productDtoList;
        }

        public ProductDto GetById(int id)
        {
            var product = _repository.GetById(id);

            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                FeedBack= product.FeedBack,
                CategoryName = product.Category.Name
            };

            return productDto;
        }

        public void Remove(int id)
        {
            var existEntity = _repository.GetById(id);

            if (existEntity == null) throw new Exception("Not found");

            _repository.Remove(existEntity);
        }

        public void Update(ProductUpdateDto updateDto)
        {
            var product = _repository.GetById(updateDto.Id);
            if (product != null)
            {
                product.Name = updateDto.Name;
                product.Price = updateDto.Price;
                product.CategoryId = updateDto.CategoryId;
                product.FeedBack = updateDto.FeedBack;
            
                _repository.Update(product);

            }

        }
        
    }
}
