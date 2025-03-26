using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.EfCore.Context;
using ECommerce.Infrastructure.EfCore;
using Microsoft.IdentityModel.Tokens;
using ECommerce.Application.Extensions;
using System.Data;
using System.Globalization;

namespace ECommerce.Application.Services
{
    public class CustomerManager
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly ICategoryService categoryService;
        private readonly IProductRepository productRepository;
        private readonly IProductService productService;
        private readonly IOrderRepository orderRepository;
        private readonly IOrderService orderService;
        private readonly BasketManager basketManager;
        private readonly IFeedBackRepository feedbackRepository;
        private readonly IFeedBackService feedbackService;
        private readonly AppDbContext appDbContext = new AppDbContext();



        private int _userId;

       

        public CustomerManager()
        {
            productRepository = new ProductRepository(appDbContext);
            productService = new ProductManager(productRepository);
            categoryRepository = new CategoryRepository(appDbContext);
            categoryService = new CategoryManager(categoryRepository);
            orderRepository = new OrderRepository(appDbContext);
            orderService = new OrderManager(orderRepository);
            basketManager = new BasketManager();
            feedbackRepository = new FeedBackRepository(appDbContext);
            feedbackService = new FeedBackManager(feedbackRepository);

            RegisterCommands();
        }
        public Dictionary<int, Action> commands = new Dictionary<int, Action>();

        public void SetUserId(int userId)
        {
            _userId = userId;
        }
        public void RegisterCommands()
        {
            commands.Add(1, ShowCategories);
            commands.Add(2, ShowProductsByCategory);
            commands.Add(3, AddToBasket);
            commands.Add(4, ShowBasket);
            commands.Add(5, ConfirmBasket);
            commands.Add(6, ShowOrders);
            commands.Add(7, RemoveFromBasket);
            commands.Add(8, CancelOrder);
            commands.Add(9, GiveFeedBack);
            commands.Add(10, ConfirmDelivery);
            commands.Add(11, Logout);
            commands.Add(12, Exit);

        }

        public void ShowCommands()
        {
            Console.WriteLine();
            foreach (var command in commands)
            {
                Console.WriteLine($"{command.Key}. {command.Value.Method.Name}");
            }
            Console.WriteLine();
        }
        public void ShowCategories()
        {
            foreach (var category in categoryService.GetAll())
            {
                Console.WriteLine($"{category.Id}. {category.Name}");
            }
        }
        public void ShowProducts()
        {
            Console.WriteLine(new string('-',80));
            Console.WriteLine($"{"Id",-5}{"Product name",-30}{"Price",-10}{"Category",-22}{"FeedBack",-12}");
            foreach (var product in productService.GetAll())
            {
                Console.WriteLine(); Console.WriteLine($"{product.Id,-5}.{product.Name,-30}{product.Price,-10}{product.CategoryName,-22}{product.FeedBack,-12}");
            }
            Console.WriteLine(new string('-', 80));
        }
        public void ShowProductsByCategory()
        {
            ShowCategories();
            Console.Write("Enter category id:");
            string categoryinputid = Console.ReadLine();
            if (!categoryinputid.IsNullOrEmpty() && int.TryParse(categoryinputid, out int categoryid))
            {
                var products = productService.GetAll(x => x.CategoryId == categoryid);
                if (products.Any())
                {
                    Console.WriteLine();
                    Console.WriteLine(new string('-', 80));
                    Console.WriteLine($"{"Id",-5}{"Product name",-30}{"Price",-10}{"Category",-30}{"FeedBack",-12}");
                    foreach (var product in products)
                    {
                        Console.WriteLine(); Console.WriteLine($"{product.Id,-5}{product.Name,-30}{product.Price,-10}{product.CategoryName,-30}{product.FeedBack,-12}");
                    }
                    Console.WriteLine(new string('-', 80));
                    Console.WriteLine();
                }
                else
                {
                    MessageHelper.ErrorOperation("No product in this category");
                }
            }
            else
            {
                MessageHelper.ErrorOperation("Invalid input for category id");
            }
        }
        public void ShowBasket()
        {
            var basket = basketManager.GetBasketForUser(_userId);
            if (basket != null && basket.Items.Count>0)
            {
                decimal total = 0;
                foreach (var item in basket.Items)
                {
                    Console.WriteLine($"{item.ProductId} {item.ProductName} {item.Price} {item.Quantity}");
                    total += item.Price;
                }
                Console.WriteLine($"Total price is {total}");
            }
            else
            {
                MessageHelper.ErrorOperation("Basket is empty!");
            }

        }

        public void ConfirmBasket()
        {
            var basket = basketManager.GetBasketForUser(_userId);
            if (basket != null)
            {
                if (basket.Items.Any())
                {
                    
                    var order = basketManager.ConfirmBasket(_userId);
                    Console.WriteLine($"Your order has been placed for the following items:");
                    foreach (var item in order.Items)
                    {
                        Console.WriteLine($"{item.Product.Name}  {item.Product.Price} {item.Quantity} {order.OrderDate}");
                    }
                    return;
                }
                else
                {
                    MessageHelper.ErrorOperation("Basket is empty!");
                }
            }
            else
            {
                MessageHelper.ErrorOperation("Basket is empty!");
            }
        }
        public void AddToBasket()
        {
            while (true)
            {
                Console.Write("Enter product Id:");
                string productidinput = Console.ReadLine();
                if (!productidinput.IsNullOrEmpty() && int.TryParse(productidinput, out int productid))
                {
                    var product = productService.Get(x => x.Id == productid);
                    if (product != null)
                    {
                        Console.Write("Enter quantity of product you want to buy:");
                        string quantityinput = Console.ReadLine();
                        if (!quantityinput.IsNullOrEmpty() && int.TryParse(quantityinput, out int quantity))
                        {
                            BasketManager basketManager = new BasketManager();
                            basketManager.AddItemToBasket(_userId, product.Id, product.Name, quantity, product.Price);
                            Console.WriteLine("Enter 1 to make an order or 2 to continue shopping");
                            string commandnumberinput = Console.ReadLine();
                            if (!commandnumberinput.IsNullOrEmpty() && int.TryParse(commandnumberinput, out int commandnumber))
                            {
                                if (commandnumber == 1)
                                {
                                    var order = basketManager.ConfirmBasket(_userId);
                                    Console.WriteLine();
                                    MessageHelper.SuccessOperation($"Your order has been placed for the following items:");
                                    int num = 1;
                                    Console.WriteLine(new string('-', 113));
                                    Console.WriteLine($"{"N№",-3}{"Product name",-30}{"Price",-10}{"Quantity",-12}{"Order date",-22}{"Order status",-12}{"Delivery date",-22}");
                                    foreach (var item in order.Items)
                                    {
                                        Console.WriteLine($"{num,-3}{item.Product.Name,-30}{item.Product.Price,-10}{item.Quantity,-12}{order.OrderDate,-22}{order.OrderStatus,-12}{order.DeliveryDate,-22}");
                                        num++;
                                    }
                                    Console.WriteLine(new string('-', 113));
                                    Console.WriteLine($"Total amount is {order.TotalAmount}");
                                    Console.WriteLine(new string('-', 113));
                                    Console.WriteLine();
                                    break;
                                }
                                else if (commandnumber == 2)
                                {
                                    return;
                                }
                            }
                            else
                            {
                                MessageHelper.ErrorOperation("Invalid choise!");
                            }
                        }
                        else
                        {
                            MessageHelper.ErrorOperation("Invalid quantity input!");
                        }
                    }
                    else
                    {
                        MessageHelper.ErrorOperation("Product not found!");
                    }
                }
                else
                {
                    MessageHelper.ErrorOperation("Invalid product id!");
                }
            }

        }
        public void RemoveFromBasket()
        {
            Console.WriteLine("Enter 1 to select basket item to remove or 2 to empty basket");
            string cominputnum = Console.ReadLine();
            if (!cominputnum.IsNullOrEmpty() && int.TryParse(cominputnum, out int comnum))
            {
                if (comnum == 1)
                {
                    ShowBasket();
                    Console.WriteLine("Enter product id to remove from basket");
                    string prinputid=Console.ReadLine();
                    if(!prinputid.IsNullOrEmpty()&& int.TryParse(prinputid, out int productid))
                    {
                        basketManager.RemoveItemFromBasket(_userId, comnum);
                    }
                    else
                    {
                        MessageHelper.ErrorOperation("Invalid id!");
                    }
                }
                else if(comnum == 2)
                {
                    var baskets = basketManager.GetBasketForUser(_userId);
                    baskets.Items.Clear();
                }
            }
            else
            {
                MessageHelper.ErrorOperation("Invalid command!");
            }
        }
        public void ShowOrders()
        {
            List <OrderDto> orders;
            Console.Write("Enter Status type:\n1 for Pending\n2 for Confirmed\n3 for Ontheway\n4 for Delivered\n5 for Cancelled\n6 for All:");
            StatusType statustype = (StatusType)(int.Parse(Console.ReadLine()));
            if (!Enum.IsDefined(typeof(StatusType), statustype))
            {
                orders = orderService.GetAll();
            }
            else
            {
                orders = orderService.GetAll(x => x.OrderStatus == statustype);
            }
            if (!orders.IsNullOrEmpty())
            {
                foreach (var order in orders)
                {
                    int num = 1;
                    Console.WriteLine(new string('-', 113));
                    Console.WriteLine($"{"N№",-3}{"Product name",-30}{"Price",-10}{"Quantity",-20}{"Order date",-30}{"Order status",-20}");
                    foreach (var item in order.Items)
                    {
                        Console.WriteLine($"{num,-3}{item.Product.Name,-30}{item.Product.Price,-10}{item.Quantity,-20}{item.Order.OrderDate,-30}{item.Order.OrderStatus,-20}");
                        num++;
                    }
                    Console.WriteLine(new string('-', 113));
                    Console.WriteLine($"Total amount is {order.TotalAmount}");
                    Console.WriteLine(new string('-', 113));
                    Console.WriteLine();
                }
            }
            else
            {
                MessageHelper.ErrorOperation("No order found!");
            }
        }

        public void CancelOrder()
        {
            var orders = orderService.GetAll(x=>x.OrderStatus == StatusType.Pending);
            if (orders != null)
            {
                Console.WriteLine("You can cancel your pending orders!");
                foreach (var order in orders)
                {
                    Console.WriteLine($"{order.Id}. {order.OrderDate}");
                }
                Console.Write("Enter order id to cancel:");
                string inputorderid = Console.ReadLine();
                if (!inputorderid.IsNullOrEmpty() && int.TryParse(inputorderid, out int orderid))
                {
                   var order = orders.Find(x=> x.Id == orderid);
                    if (order != null)
                    {
                        order.OrderStatus = StatusType.Cancelled;
                        var orderupdatedto = MapExtensions.ToOrderUpdateDto(order);
                        orderService.Update(orderupdatedto);
                        MessageHelper.SuccessOperation("Your order calcelled!");
                    }
                    else
                    {
                        MessageHelper.ErrorOperation("Invalid order id!");
                    }
                }
                else
                {
                    MessageHelper.ErrorOperation("Invalid order id!");
                }
            }
            else
            {
                MessageHelper.ErrorOperation("No order found to cancel!");
            }
        }

        public void GiveFeedBack()
        {
            var orders = orderService.GetAll(x => x.OrderStatus == StatusType.Delivered);
            bool isexist = false;
            var orderitems = new List<OrderItem>();

            foreach (var order in orders)
            {
                foreach (var item in order.Items)
                {
                    if (item.Order.UserId == _userId)
                    {
                        var userfeedback = appDbContext.UserFeedBacks
                            .Any(x => x.FeedBack.UserId == _userId && x.ProductId == item.ProductId);

                        if (!userfeedback)
                        {
                            orderitems.Add(item);
                            Console.WriteLine($"{item.ProductId}. {item.Product.Name}");
                            isexist = true;
                        }
                    }
                }
            }

            if (!isexist)
            {
                MessageHelper.ErrorOperation("No product found in your orders to feedback!");
                return;
            }

            Console.Write("Enter product id to feedback: ");
            string prinputid = Console.ReadLine();

            if (!prinputid.IsNullOrEmpty() && int.TryParse(prinputid, out int productid))
            {
                isexist = false;

                foreach (var order in orders)
                {
                    foreach (var item in orderitems)
                    {
                        if (item.ProductId == productid)
                        {
                            Console.Write("Enter your FeedBack from 1 to 5[ex: 4.7, 3.2]: ");
                            string inputmark = Console.ReadLine();
                            inputmark = inputmark?.Trim();

                            if (!inputmark.IsNullOrEmpty() && double.TryParse(inputmark, NumberStyles.Any, CultureInfo.InvariantCulture, out double productfeedback) && productfeedback > 0 && productfeedback <= 5)
                            {

                                var feedbackcreate = new FeedBackCreateDto
                                {
                                    UserId = _userId,
                                    FeedBackMark = productfeedback,
                                    CreatedAt= DateTime.Now
                                };
                                
                                feedbackService.Add(feedbackcreate);
                                if (item.Product != null)
                                {
                                    double feedbackmark;
                                    if (item.Product.FeedBack != 0)
                                    {
                                        feedbackmark = (item.Product.FeedBack + productfeedback) / 2;
                                    }
                                    else
                                    {
                                        feedbackmark = productfeedback;
                                    }
                                    item.Product.FeedBack = feedbackmark;

                                    var product = productService.GetById(item.ProductId);
                                    var updatedto = new ProductUpdateDto
                                    {
                                        Id= product.Id,
                                        Price = product.Price,
                                        CategoryId = item.Product.CategoryId,
                                        Name = product.Name,
                                        FeedBack = feedbackmark

                                    };
                                    productService.Update(updatedto);

                                }
                                var feedback = feedbackService.Get(x => x.UserId == _userId);

                                var userfeedbackcreate = new UserFeedBackCreateDto
                                {
                                    FeedBackId = feedback.Id, 
                                    ProductId = item.ProductId
                                };
                                
                                var userfeedback = MapExtensions.ToUserFeedBack(userfeedbackcreate);

                                appDbContext.UserFeedBacks.Add(userfeedback);
                                appDbContext.SaveChanges(); 

                                feedback.UserFeedBacks.Add(userfeedback);
                                appDbContext.SaveChanges(); 

                                item.UserFeedBacks.Add(userfeedback);

                                MessageHelper.SuccessOperation($"Your {productfeedback}* feedback is set!");
                                isexist = true;
                                break;
                            }
                        }
                    }

                    if (isexist)
                    {
                        break;
                    }
                }
                if (!isexist)
                {
                    MessageHelper.ErrorOperation("No product found in your orders to give feedback on!");
                }
            }
            
            else
            {
                MessageHelper.ErrorOperation("Invalid product id!");
            }
        }

        public void Exit()
        {
            Console.WriteLine("Exiting....");
            return;

        }
        public void Logout()
        {
            Console.WriteLine("Logging out....");

        }
        public void ConfirmDelivery()
        {
            var orders = orderService.GetAll(o => o.UserId == _userId && o.OrderStatus == StatusType.Ontheway);
            if (!orders.IsNullOrEmpty())
            {
                Console.WriteLine(new string('-', 100));
                Console.WriteLine($"{"Order id",-10}{"Total amount",-10}{"Order date",-30}{"Order status",-20}");
                foreach (var order in orders)
                {
                    Console.WriteLine($"{order.Id,-3}. {order.TotalAmount,-10}{order.OrderDate,-30}{order.OrderStatus,20}");
                    int num = 1;
                    Console.WriteLine(new string('-', 113));
                    Console.WriteLine($"{"N№",-3}{"Product name",-30}{"Price",-10}{"Quantity",-20}{"Order date",-30}{"Order status",-20}");
                    foreach (var item in order.Items)
                    {
                        Console.WriteLine($"{num,-3}{item.Product.Name,-30}{item.Product.Price,-10}{item.Quantity,-20}{item.Order.OrderDate,-30}{item.Order.OrderStatus,-20}");
                        num++;
                    }
                    Console.WriteLine(new string('-', 113));
                    Console.WriteLine($"Total amount is {order.TotalAmount}");
                    Console.WriteLine(new string('-', 113));
                    Console.WriteLine();
                }
                Console.WriteLine(new string('-', 100));
                Console.WriteLine();
                Console.Write("Enter 0 to confirm delivery of all on the way orders or enter order id to confirm delivery of that one:");
                string? confirmnum = Console.ReadLine();
                if (!confirmnum.IsNullOrEmpty() && int.TryParse(confirmnum, out int confirmationnumber))
                {
                    if (confirmationnumber == 0)
                    {
                        foreach (var item in orders)
                        {
                            item.OrderStatus = StatusType.Delivered;
                            var updatedto = MapExtensions.ToOrderUpdateDto(item);
                            orderService.Update(updatedto);
                        }
                        MessageHelper.SuccessOperation("All products of on the way orders are delivered!");
                    }
                    else if (confirmationnumber > 0)
                    {
                        bool isexist = false;
                        foreach (var item in orders)
                        {
                            if (item.Id == confirmationnumber)
                            {
                                item.OrderStatus = StatusType.Delivered;
                                var updatedto = MapExtensions.ToOrderUpdateDto(item);
                                orderService.Update(updatedto);
                                isexist = true;
                                MessageHelper.SuccessOperation($"All products of the order with {item.Id} id is delivered!");
                            }
                        }
                        if (!isexist)
                        {
                            MessageHelper.ErrorOperation($"No order found with {confirmationnumber} id!");
                        }
                    }
                }
            }
            else
            {
                MessageHelper.ErrorOperation("No order (on the way) found");
            }
        }

    }
}
