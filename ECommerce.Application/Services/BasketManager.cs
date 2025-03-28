using ECommerce.Application.Interfaces;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.EfCore.Context;
using ECommerce.Infrastructure.EfCore;
using ECommerce.UI;
using ECommerce.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using AutoMapper;

namespace ECommerce.Application.Services;

public class BasketManager
{
    private readonly Dictionary<int, Basket> _userBaskets = new Dictionary<int, Basket>();
    private readonly IOrderRepository orderRepository;
    private readonly IOrderService orderService;
    private int _userId;
    private readonly IMapper _mapper;

    public BasketManager()
    {
        AppDbContext appDbContext = new AppDbContext();
        orderRepository = new OrderRepository(appDbContext);
        orderService = new OrderManager(orderRepository);
    }
    public BasketManager(IMapper mapper)
    {
        _mapper = mapper;
    }


    public Basket GetBasketForUser(int userId)
    {
        _userId = userId;
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

        var orderCreateDto = _mapper.Map<OrderCreateDto>(basket);
        if (orderCreateDto == null)
        {
            Console.WriteLine("Failed to create order DTO.");
            return null;
        }
        orderService.Add(orderCreateDto);
        var orderDto = orderService.Get(
            predicate: x=>x.OrderDate == orderCreateDto.OrderDate && x.UserId==_userId,
            include: query => query
            .Include(o=>o.User!)
            .Include(o => o.Items!)
            .ThenInclude(oi => oi.Product!));
        
        _userBaskets.Remove(userId); 
        return orderDto;
    }
}
