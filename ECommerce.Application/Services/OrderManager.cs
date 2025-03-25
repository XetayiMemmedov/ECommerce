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
    public class OrderManager : IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderManager(IOrderRepository repository)
        {
            _repository = repository;
        }

        public void Add(OrderCreateDto createDto)
        {
            var order = createDto.ToOrder();

            _repository.Add(order);
        }

        public OrderDto Get(Expression<Func<Order, bool>> predicate)
        {
            var order = _repository.Get(predicate);

            var orderDto = order.ToOrderDto();

            return orderDto;
        }

        public List<OrderDto> GetAll(Expression<Func<Order, bool>>? predicate = null, bool asNoTracking = false)
        {
            var orders = _repository.GetAll(predicate, asNoTracking);

            var orderDtoList = new List<OrderDto>();

            foreach (var item in orders)
            {
                orderDtoList.Add(new OrderDto
                {
                    Id = item.Id,
                    User=item.User,
                    Items=item.Items,
                    TotalAmount=item.TotalAmount,
                    OrderDate=item.OrderDate,
                    OrderStatus=item.OrderStatus,

                });
            }

            return orderDtoList;
        }

        public OrderDto GetById(int id)
        {
            var order = _repository.GetById(id);

            var orderDto = new OrderDto
            {
                Id = order.Id,
                User = order.User,
                Items = order.Items,
                OrderDate=order.OrderDate,
                OrderStatus = order.OrderStatus,
            };

            return orderDto;
        }

        public void Remove(int id)
        {
            var existEntity = _repository.GetById(id);

            if (existEntity == null) throw new Exception("Not found");

            _repository.Remove(existEntity);
        }

        public void Update(OrderUpdateDto updateDto)
        {
            var order = _repository.GetById(updateDto.Id);
            if (order != null)
            {
                order.Id = updateDto.Id;
                order.OrderStatus = updateDto.OrderStatus;
                _repository.Update(order);
            }
        }
    }
}

