using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.UI;

namespace ECommerce.Application.Extensions;

public static class MapExtensions
{
    public static Category ToCategory(this CategoryCreateDto createDto)
    {
        return new Category
        {
            Name = createDto.Name
        };
    }

    public static CategoryDto ToCategoryDto(this Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }
    public static Product ToProduct(this ProductCreateDto createDto)
    {
        return new Product
        {
            Name = createDto.Name,
            Price = createDto.Price,
            CategoryId = createDto.CategoryId,

        };
    }

    public static ProductDto? ToProductDto(this Product product)
    {
        if (product!= null)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CategoryName = product.Category?.Name
            };
        }
        else 
            return null;
    }

    public static OrderCreateDto ToOrderCreateDto(this Basket basket)
    {
        decimal totalamount = 0;
        if (basket.PromoCode == "NOVRUZ20")
        {
            Console.WriteLine($"{basket.PromoCode} promocode is applied! Discount 20%!");
            foreach (var item in basket.Items)
            {
                totalamount += item.Price * item.Quantity;
            }
            return new OrderCreateDto
            {
                UserId = basket.UserId,
                OrderDate = DateTime.Now,
                DeliveryDate= DateTime.Now.AddDays(10),
                OrderStatus = StatusType.Pending,
                Items = basket.Items
                .Select(x => new OrderItem
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList(),
                Quantity = basket.Items.Count,
                TotalAmount = totalamount * 0.8M
            };
        }
        else
        {
            foreach (var item in basket.Items)
            {
                totalamount += item.Price * item.Quantity;
            }
            return new OrderCreateDto
            {
                UserId = basket.UserId,
                OrderDate = DateTime.Now,
                DeliveryDate=DateTime.Now.AddDays(10),
                OrderStatus = StatusType.Pending,
                Items = basket.Items
                .Select(x => new OrderItem
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList(),
                Quantity = basket.Items.Count,
                TotalAmount = totalamount
            };
        }
    }
    public static Order ToOrder(this OrderCreateDto createDto)
    {
        return new Order
        {
            UserId = createDto.UserId,
            OrderDate = createDto.OrderDate,
            DeliveryDate=createDto.DeliveryDate,
            OrderStatus = createDto.OrderStatus,
            Items = createDto.Items
            .Select(x => new OrderItem
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                Product = x.Product,
                Order=x.Order
            }).ToList(),
            TotalAmount = createDto.TotalAmount

        };
    }

    public static OrderDto ToOrderDto(this Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            User = order.User,
            OrderDate = order.OrderDate,
            DeliveryDate= order.DeliveryDate,
            Items = order.Items
            .Select(x => new OrderItem
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                Product = x.Product,
                OrderId = x.OrderId,
                Order=x.Order,
            }).ToList(),
            OrderStatus = order.OrderStatus,
            TotalAmount = order.TotalAmount,

        };
    }
    public static OrderUpdateDto ToOrderUpdateDto(this OrderDto orderdto)
    {
        return new OrderUpdateDto
        {
            Id = orderdto.Id,
            OrderStatus = orderdto.OrderStatus,
        };
    }

    public static OrderItem ToOrderItem(this OrderItemCreateDto createDto)
    {
        return new OrderItem
        {
            OrderId = createDto.OrderId,
            ProductId = createDto.ProductId,
            Quantity = createDto.Quantity,
        };
    }

    public static OrderItemDto ToOrderItemDto(this OrderItem orderitem)
    {
        return new OrderItemDto
        {
            Order = orderitem.Order,
            Product = orderitem.Product,
            Quantity = orderitem.Quantity,
            
        };
    }


    public static User ToUser(this UserCreateDto createDto)
    {
        return new User
        {
            UserName = createDto.UserName,
            Password = createDto.Password,
            Email = createDto.Email,
            Role = createDto.Role,
        };
    }

    public static UserDto ToUserDto(this User user)
    {
        if (user == null)
        {
            return null;
        }
        else
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Role = user.Role,
            };
        }

    }


    public static FeedBack ToFeedBack(this FeedBackCreateDto createDto)
    {
        if (createDto == null)
        {
            return null;
        }
        else
        {
            return new FeedBack
            {
                UserId = createDto.UserId,
                FeedBackMark = createDto.FeedBackMark,
                CreatedAt =createDto.CreatedAt,
                UserFeedBacks = createDto.UserFeedBacks
                .Select(f => new UserFeedBack
                {
                    ProductId = f.ProductId,
                    FeedBack = f.FeedBack,
                    Product = f.Product,
                })
                .ToList(),
            };
        }
    }

    public static FeedBackDto ToFeedBackDto(this FeedBack feedback)
    {
        if (feedback == null)
        {
            return null;
        }
        else
        {
            return new FeedBackDto
            {
                Id = feedback.Id,
                UserId = feedback.UserId,
                FeedBackMark = feedback.FeedBackMark,
                CreatedAt = feedback.CreatedAt??DateTime.Now,
                UserFeedBacks = feedback.UserFeedBacks
                .Select(f => new UserFeedBack
                {
                    FeedBackId = f.FeedBackId,
                    ProductId = f.ProductId,
                    FeedBack = f.FeedBack,
                    Product = f.Product,
                })
                .ToList(),
            };
        }
    }

    public static UserFeedBack? ToUserFeedBack(this UserFeedBackCreateDto createDto)
    {
        if (createDto == null)
        {
            return null;
        }
        else
        {
            return new UserFeedBack
            {
                FeedBackId = createDto.FeedBackId,
                ProductId = createDto.ProductId,
                
            };
        }
    }

    public static UserFeedBackDto ToUserFeedBackDto(this UserFeedBack userfeedback)
    {
        if (userfeedback == null)
        {
            return null;
        }
        else
        {
            return new UserFeedBackDto
            {
                Id = userfeedback.Id,
                FeedBack = userfeedback.FeedBack,
                Product = userfeedback.Product,
            };
        }

    }
}

