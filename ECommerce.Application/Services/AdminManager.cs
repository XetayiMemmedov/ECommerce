using AutoMapper;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.EfCore;
using ECommerce.Infrastructure.EfCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Application.Services;

public class AdminManager
{
    private readonly ICategoryRepository categoryRepository;
    private readonly ICategoryService categoryService;
    private readonly IProductRepository productRepository;
    private readonly IProductService productService;
    private readonly IOrderRepository orderRepository;
    private readonly IOrderService orderService;
    private readonly IUserRepository userRepository;
    private readonly IUserService userService;
    private readonly IMapper _mapper;
    public AdminManager()
    {
        AppDbContext appDbContext = new AppDbContext();
        categoryRepository = new CategoryRepository(appDbContext);
        categoryService = new CategoryManager(categoryRepository);

        productRepository = new ProductRepository(appDbContext);
        productService = new ProductManager(productRepository);

        orderRepository = new OrderRepository(appDbContext);
        orderService = new OrderManager(orderRepository);

        userRepository = new UserRepository(appDbContext);
        userService = new UserManager(userRepository);
        RegisterCommands();
    }
    public AdminManager(IMapper mapper)
    {
        _mapper = mapper;
    }
    public Dictionary<int, Action> commands = new Dictionary<int, Action>();

    public void RegisterCommands()
    {
        commands.Add(1, AddUser);
        commands.Add(2, UpdateUser);
        commands.Add(3, RemoveUser);
        commands.Add(4, AddCategory);
        commands.Add(5, UpdateCategory);
        commands.Add(6, RemoveCategory);
        commands.Add(7, AddProduct);
        commands.Add(8, UpdateProduct);
        commands.Add(9, RemoveProduct);
        commands.Add(10, ShowUsers);
        commands.Add(11, ShowCategories);
        commands.Add(12, ShowProducts);
        commands.Add(13, ConfirmOrder);
        commands.Add(14, Exit);
        commands.Add(15, Logout);
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
    
    public void ShowUsers()
    {
        foreach (var user in userService.GetAll())
        {
            Console.WriteLine($"{user.Id}. {user.Name} {user.Email} {user.Role} {user.Password}");
        }
    }
    public void ShowCategories()
    {
        foreach(var category in categoryService.GetAll())
        {
            Console.WriteLine($"{category.Id}. {category.Name}");
        }
    }

    public void ShowProducts()
    {
        Console.WriteLine();
        Console.WriteLine(new string('-', 80));
        Console.WriteLine($"{"Id",-5}{"Product name",-30}{"Price",-10}{"Category",-30}{"FeedBack",-12}");
        foreach (var product in productService.GetAll(
            include:query=>query.Include(p=>p.Category), 
            orderBy: query=> query.OrderBy(p=>p.Id)))
        {
            Console.WriteLine(); Console.WriteLine($"{product.Id,-5}{product.Name,-30}{product.Price,-10}{product.CategoryName,-30}{product.FeedBack,-12}");
        }
        Console.WriteLine(new string('-', 80));
        Console.WriteLine();
    }
    public void ConfirmOrder()
    {
        Type enumtype = typeof(StatusType);

        Console.WriteLine("Available order statuses:");
        foreach (var enumvalue in Enum.GetValues(enumtype))
        {
            Console.WriteLine($"{enumvalue}: {(int)enumvalue}");
        }

        Console.WriteLine("Enter status number to see orders of that type:");
        string statusnuminput = Console.ReadLine();

        if (!string.IsNullOrEmpty(statusnuminput) && int.TryParse(statusnuminput, out int statusnumber))
        {
            if (Enum.IsDefined(typeof(StatusType), statusnumber))
            {
                StatusType status = (StatusType)statusnumber;

                var orders = orderService.GetAll(predicate: o => o.OrderStatus == status);

                if (orders != null && orders.Any())
                {
                    Console.WriteLine(new string('-', 101));
                    Console.WriteLine($"{"Order id",-11}{"Username",-30}{"Total amount",-20}{"Order date",-22}{"Order status",-12}");

                    foreach (var item in orders)
                    {
                        if (item.User != null)
                        {
                            Console.WriteLine($"{item.Id,-11}{item.User.UserName,-30}{item.TotalAmount,-20}{item.OrderDate,-22}{item.OrderStatus,-12}");
                        }
                    }
                    Console.WriteLine(new string('-', 101));

                    Console.WriteLine("Enter status number you want to change orders to:");
                    statusnuminput = Console.ReadLine();

                    if (!string.IsNullOrEmpty(statusnuminput) && int.TryParse(statusnuminput, out statusnumber))
                    {
                        if (Enum.IsDefined(typeof(StatusType), statusnumber))
                        {
                            status = (StatusType)statusnumber;

                            Console.WriteLine("Enter 0 to change status of all orders or enter the order number to change status of a specific order:");

                            string confirmnum = Console.ReadLine();
                            if (!string.IsNullOrEmpty(confirmnum) && int.TryParse(confirmnum, out int confirmationnumber))
                            {
                                if (confirmationnumber == 0)
                                {
                                    foreach (var item in orders)
                                    {
                                        item.OrderStatus = status;
                                        var updatedto = _mapper.Map<OrderUpdateDto>(item);
                                        orderService.Update(updatedto);
                                    }
                                    MessageHelper.SuccessOperation($"Status of all the given orders are changed to {status}.");
                                }
                                else
                                {
                                    bool isExist = false;
                                    foreach (var item in orders)
                                    {
                                        if (item.Id == confirmationnumber)
                                        {
                                            item.OrderStatus = status;
                                            var updatedto = _mapper.Map<OrderUpdateDto>(item);
                                            orderService.Update(updatedto);
                                            isExist = true;
                                            MessageHelper.SuccessOperation($"The order with ID {item.Id} has been updated to {status}!");
                                            break;
                                        }
                                    }

                                    if (!isExist)
                                    {
                                        MessageHelper.ErrorOperation($"No order found with ID {confirmationnumber}.");
                                    }
                                }
                            }
                            else
                            {
                                MessageHelper.ErrorOperation("Invalid input for order selection!");
                            }
                        }
                        else
                        {
                            MessageHelper.ErrorOperation($"{statusnumber} is not a valid status number.");
                        }
                    }
                    else
                    {
                        MessageHelper.ErrorOperation("Invalid input for status change.");
                    }
                }
                else
                {
                    MessageHelper.ErrorOperation("No orders found with the selected status.");
                }
            }
            else
            {
                MessageHelper.ErrorOperation($"{statusnumber} is not a valid status in the StatusType enum.");
            }
        }
        else
        {
            MessageHelper.ErrorOperation("Invalid status number entered.");
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
    public void AddUser()
    {

        Console.Write("Enter UserName:");
        string? username = Console.ReadLine();
        if (!username.IsNullOrEmpty())
        {
            string password;
            while (true)
            {
                password = MessageHelper.GetPasswordInput("Enter Password:");
                string cpassword = MessageHelper.GetPasswordInput("Confirm Password:");
                if (password != cpassword)
                {
                    MessageHelper.ErrorOperation("Password does not match!");
                    return;
                }
                else
                    break;
            }
            Console.Write("Enter User type:\n1 for Admin\n2 for Customer:");
            UserType usertype = (UserType)(int.Parse(Console.ReadLine()));
            if (!Enum.IsDefined(typeof(UserType), usertype))
            {
                usertype = UserType.Customer;
            }

            Console.Write("Enter Email:");
            string? email = Console.ReadLine();

            AppDbContext appDbContext = new AppDbContext();
            userService.Add(new UserCreateDto
            {
                UserName = username,
                Password = password,
                Role = usertype,
                Email = email
            });
            MessageHelper.SuccessOperation($"{username} is successfully added as {usertype}!");
        }
        else
        {
            MessageHelper.ErrorOperation("Username must not be empty!");
        }
    }
    public void UpdateUser()
    {
        var users = userService.GetAll().ToList();
        foreach (var user in users)
        {
            Console.WriteLine($"{user.Id}.{user.Name} {user.Email} {user.Role}");
        }
        while (true)
        {
            Console.Write("Enter user id you want to update: ");
            string? userinputid = Console.ReadLine();
            if (!userinputid.IsNullOrEmpty() && int.TryParse(userinputid, out int userid))
            {
                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].Id == userid)
                    {
                        Console.Write("Enter UserName:");
                        string? username = Console.ReadLine();
                        string password;
                        while (true)
                        {
                            password = MessageHelper.GetPasswordInput("Enter Password:");
                            string cpassword = MessageHelper.GetPasswordInput("Confirm Password:");
                            if (password != cpassword)
                            {
                                MessageHelper.ErrorOperation("Password does not match!");
                                return;
                            }
                            else
                                break;
                        }
                        Console.Write("Enter User type:\n1 for Admin\n2 for Customer:");
                        UserType usertype = (UserType)(int.Parse(Console.ReadLine()));
                        if (!Enum.IsDefined(typeof(UserType), usertype))
                        {
                            usertype = UserType.Customer;
                        }

                        Console.Write("Enter Email:");
                        string email = Console.ReadLine();

                        AppDbContext appDbContext = new AppDbContext();
                        userService.Update(new UserUpdateDto
                        {
                            UserName = username,
                            Password = password,
                            Role = usertype,
                            Email = email,
                            Id = userid,
                        });
                        MessageHelper.SuccessOperation($"{username} is successfully updated!");
                        break;
                    }

                }
                break;
            }
            else
            {
                MessageHelper.ErrorOperation("Invalid id!");
                return;
            }
        }
    }

    public void RemoveUser()
    {
        var users = userService.GetAll().ToList();
        foreach (var user in users)
        {
            Console.WriteLine($"{user.Id}.{user.Name} {user.Role}");
        }
        while (true)
        {
            Console.Write("Enter user id you want to update: ");
            string? userinputid = Console.ReadLine();
            if (!userinputid.IsNullOrEmpty() && int.TryParse(userinputid, out int userid))
            {
                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].Id == userid)
                    {
                        userService.Remove(userid);
                        MessageHelper.SuccessOperation($"{users[i].Name} is scuccessfully removed!");
                        return;
                    }
                }

            }
            else
            {
                MessageHelper.ErrorOperation("Invalid user id");
            }
        }
        
    }
    public void AddCategory()
    {
        Console.Write("Enter Category name:");
        string? categoryname = Console.ReadLine();
        if(!categoryname.IsNullOrEmpty())
        {
            categoryService.Add(new CategoryCreateDto
            {
                Name = categoryname
            });
            MessageHelper.SuccessOperation($"Category {categoryname} is successfully added!");
        }
        else
        {
            MessageHelper.ErrorOperation("Invalid input");
        }
       
    }
    public void UpdateCategory()
    {
        var categories = categoryService.GetAll().ToList();
        foreach (var category in categories)
        {
            Console.WriteLine($"{category.Id}.{category.Name}");
        }
        while (true)
        {
            Console.Write("Enter Category id to update:");
            string categoryidinput = Console.ReadLine();
            if (!categoryidinput.IsNullOrEmpty() && int.TryParse(categoryidinput, out var categoryid))
            {
                for (int i = 0; i < categories.Count; i++)
                {
                    if (categories[i].Id == categoryid)
                    {

                        while (true)
                        {
                            Console.Write("Enter Category name:");
                            string? categoryname = Console.ReadLine();
                            if (!categoryname.IsNullOrEmpty())
                            {
                                categoryService.Update(new CategoryUpdateDto
                                {
                                    Name = categoryname,
                                    Id = categoryid
                                });
                                MessageHelper.SuccessOperation($"Category {categoryname} is successfully added!");
                                return;
                            }
                            else
                            {
                                MessageHelper.ErrorOperation("Invalid input for category name!");
                            }
                        }
                    }

                }
                MessageHelper.ErrorOperation("Invalid category id!");

            }
            else
            {
                MessageHelper.ErrorOperation("Invalid input for category id!");
            }
        }
        
    }

    public void RemoveCategory()
    {
        var categories = categoryService.GetAll().ToList();
        foreach (var category in categories)
        {
            Console.WriteLine($"{category.Id}.{category.Name}");
        }
        while (true)
        {
            Console.Write("Enter Category id to update:");
            string? categoryidinput = Console.ReadLine();
            if (!categoryidinput.IsNullOrEmpty() && int.TryParse(categoryidinput, out var categoryid))
            {
                for (int i = 0; i < categories.Count; i++)
                {
                    if (categories[i].Id == categoryid)
                    {
                        categoryService.Remove(categoryid);
                        Console.WriteLine($"{categories[i].Name} is successfully removed!");
                        return;
                    }
                }

                Console.WriteLine("Invalid input for category id!");

            }
            else
            {
                Console.WriteLine("Invalid input for category id!");

            }
        }
    }
    public void AddProduct()
    {
        Console.Write("Enter Product name:");
        string? productname = Console.ReadLine();
        if (!productname.IsNullOrEmpty())
        {
            ShowCategories();
            Console.Write("Enter Category id:");
            string? categoryidinput = Console.ReadLine();
            if (int.TryParse(categoryidinput, out int categoryid))
            {
                Console.Write("Enter product price:");
                string? productpriceinput= Console.ReadLine();
                if (Decimal.TryParse(productpriceinput, out decimal productprice))
                {
                    productService.Add(new ProductCreateDto
                    {
                        Name = productname,
                        CategoryId = categoryid,
                        Price = productprice

                    });
                    Console.WriteLine($"Product {productname} is successfully added!");
                }
                else
                {
                    Console.WriteLine("Invalid input!");
                }

            }
            else
            {
                Console.WriteLine("Invalid input!");
            }
        }
        else
        {
            Console.WriteLine("Invalid input!");
        }
    }

    public void UpdateProduct()
    {
        var products = productService.GetAll(include: query=>query.Include(p=>p.Category), orderBy: query=>query.OrderBy(p=>p.Id)).ToList();
        foreach (var product in products)
        {
            Console.WriteLine($"{product.Id}.{product.Name} {product.Price} {product.CategoryName}");
        }
        while (true)
        {
            Console.Write("Enter Product id to update:");
            string? productidinput = Console.ReadLine();
            if (!productidinput.IsNullOrEmpty() && int.TryParse(productidinput, out int productid))
            {
                for (int i = 0; i < products.Count; i++)
                {
                    if (products[i].Id == productid)
                    {

                        while (true)
                        {
                            Console.Write("Enter Product name:");
                            string? productname = Console.ReadLine();
                            if (!productname.IsNullOrEmpty())
                            {
                                ShowCategories();
                                Console.Write("Enter Category id:");
                                string? categoryidinput = Console.ReadLine();
                                if (!categoryidinput.IsNullOrEmpty())
                                {
                                    if (int.TryParse(categoryidinput, out int categoryid))
                                    {
                                        Console.Write("Enter product price:");
                                        string? productpriceinput = Console.ReadLine();
                                        if (!productpriceinput.IsNullOrEmpty())
                                        {
                                            if (Decimal.TryParse(productpriceinput, out decimal productprice))
                                            {
                                                productService.Update(new ProductUpdateDto
                                                {
                                                    Id = productid,
                                                    Name = productname,
                                                    CategoryId = categoryid,
                                                    Price = productprice

                                                });
                                                MessageHelper.SuccessOperation($"Product {productname} is successfully updated!");
                                                return;
                                            }
                                            else
                                            {
                                                MessageHelper.ErrorOperation("Invalid input!");
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    MessageHelper.ErrorOperation("Invalid category id!");
                                }
                            }
                            else
                            {
                                MessageHelper.ErrorOperation("Invalid input!");
                            }
                        }
                    }
                }

                MessageHelper.ErrorOperation("Invalid product id!");
            }
            else
            {
                MessageHelper.ErrorOperation("Invalid input for product id!");
            }
        }
    }
    public void RemoveProduct()
    {
        var products = productService.GetAll().ToList();
        foreach (var product in products)
        {
            Console.WriteLine($"{product.Id}.{product.Name} {product.Price} {product.CategoryName}");
        }
        while (true)
        {
            Console.Write("Enter Product id to update:");
            string productidinput = Console.ReadLine();
            if (!productidinput.IsNullOrEmpty() && int.TryParse(productidinput, out int productid))
            {
                for (int i = 0; i < products.Count; i++)
                {
                    if (products[i].Id == productid)
                    {
                        productService.Remove(productid);
                        MessageHelper.SuccessOperation($"{products[i].Name} is successfully removed!");
                        return;
                    }
                }
                MessageHelper.ErrorOperation("Invalid product id!");
            }
            else
            {
                MessageHelper.ErrorOperation("Invalid input for product id!");

            }

        }

    }

    
}


