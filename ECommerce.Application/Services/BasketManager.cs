using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Extensions;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.EfCore.Context;
using ECommerce.Infrastructure.EfCore;
using ECommerce.UI;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Services
{
    public class BasketManager
    {
        private readonly Dictionary<int, Basket> _userBaskets = new Dictionary<int, Basket>();
        private readonly IUserRepository _repository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ICategoryService categoryService;
        private readonly IProductRepository productRepository;
        private readonly IProductService productService;
        private readonly IOrderRepository orderRepository;
        private readonly IOrderService orderService;
        private int _userId;

        public BasketManager()
        {
            AppDbContext appDbContext = new AppDbContext();
            productRepository = new ProductRepository(appDbContext);
            productService = new ProductManager(productRepository);
            categoryRepository = new CategoryRepository(appDbContext);
            categoryService = new CategoryManager(categoryRepository);
            orderRepository = new OrderRepository(appDbContext);
            orderService = new OrderManager(orderRepository);
        }

        public Basket GetBasketForUser(int userId)
        {
            if (!_userBaskets.ContainsKey(userId))
            {
                _userBaskets[userId] = new Basket(userId);
            }
            return _userBaskets[userId];
        }

        public void AddItemToBasket(int userId, int productId, string productName, int quantity, decimal price)
        {
            var basket = GetBasketForUser(userId);
            var basketItem = basket.Items.FirstOrDefault(i => i.ProductId == productId);
            if (basketItem != null)
            {
                basketItem.Quantity += quantity;
            }
            else
            {
                basket.Items.Add(new BasketItem
                {
                    ProductName = productName,
                    ProductId = productId,
                    Quantity = quantity,
                    Price = price
                });
            }
        }

        public void RemoveItemFromBasket(int userId, int productId)
        {
            var basket = GetBasketForUser(userId);
            var basketItem = basket.Items.FirstOrDefault(i => i.ProductId == productId);
            if (basketItem != null)
            {
                basket.Items.Remove(basketItem);
            }
        }

        public OrderDto ConfirmBasket(int userId)
        {
            Console.WriteLine("If you have a promocode enter:");
            string promocode = Console.ReadLine();
            
            
            var basket = GetBasketForUser(userId);
            if (basket.Items.Count == 0)
            {
                Console.WriteLine("No items in the basket to confirm.");
                return null;
            }
            basket.PromoCode = promocode;
            var orderCreateDto = MapExtensions.ToOrderCreateDto(basket);
            if (orderCreateDto == null)
            {
                Console.WriteLine("Failed to create order DTO.");
                return null;
            }
            orderService.Add(orderCreateDto);
            var orderDto = orderService.Get(x => x.User.Id == userId);
            
            _userBaskets.Remove(userId); 
            return orderDto;
        }
    }
}
