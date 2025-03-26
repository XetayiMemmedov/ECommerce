using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata;
using ECommerce.Application.DTOs;
using ECommerce.Application.Extensions;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Services;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.EfCore;
using ECommerce.Infrastructure.EfCore.Context;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.UI;

public partial class Program
{
    public static int userId;
    static void Main(string[] args)
    {
        var appDbContext = new AppDbContext();
        var userRepository = new UserRepository(appDbContext);
        var userService = new UserManager(userRepository);
        bool isLoggedIn = false;
        bool isexit = false;
        while (true)
        {
            if (isexit)
            {
                break;
            }
            if (!isLoggedIn)
            {
                Console.Write("Enter UserName:");
                string? username = Console.ReadLine();
                string password = userService.GetPasswordInput("Enter Password:");

                if (!username.IsNullOrEmpty() && !password.IsNullOrEmpty())
                {
                    var user = userService.Get(x => x.UserName == username && x.Password == password);
                    if (user != null)
                    {
                        var status = user.Role;
                        SuccessOperation($"Welcome {status} {user.Name}");
                        userId = user.Id;
                        isLoggedIn = true;
                        isexit = false;
                        if (status == UserType.Admin)
                        {
                            while (true)
                            {
                                AdminManager adminManager = new AdminManager();
                                adminManager.ShowCommands();
                                var commandnumber = Console.ReadLine();
                                switch (commandnumber)
                                {
                                    case "1":
                                        adminManager.AddUser();
                                        break;
                                    case "2":
                                        adminManager.UpdateUser();
                                        break;
                                    case "3":
                                        adminManager.RemoveUser();
                                        break;
                                    case "4":
                                        adminManager.AddCategory();
                                        break;
                                    case "5":
                                        adminManager.UpdateCategory();
                                        break;
                                    case "6":
                                        adminManager.RemoveCategory();
                                        break;
                                    case "7":
                                        adminManager.AddProduct();
                                        break;
                                    case "8":
                                        adminManager.UpdateProduct();
                                        break;
                                    case "9":
                                        adminManager.RemoveProduct();
                                        break;
                                    case "10":
                                        adminManager.ShowUsers();
                                        break;
                                    case "11":
                                        adminManager.ShowCategories();
                                        break;
                                    case "12":
                                        adminManager.ShowProducts();
                                        break;
                                    case "13":
                                        adminManager.ConfirmOrder();
                                        break;
                                    case "14":
                                        adminManager.Exit();
                                        isLoggedIn = false;
                                        isexit = true;
                                        break;
                                    case "15":
                                        adminManager.Logout();
                                        isLoggedIn = false;
                                        break;

                                    default:
                                        ErrorOperation("Invalid command!");
                                        break;
                                }
                                if (!isLoggedIn)
                                {
                                    break;
                                }

                            }
                        }
                        else
                        {
                            while (true)
                            {

                                CustomerManager customerManager = new CustomerManager();
                                customerManager.SetUserId(userId);
                                customerManager.ShowCommands();
                                var commandnumber = Console.ReadLine();
                                switch (commandnumber)
                                {
                                    case "1":
                                        customerManager.ShowCategories();
                                        break;
                                    case "2":
                                        customerManager.ShowProductsByCategory();
                                        break;
                                    case "3":
                                        customerManager.AddToBasket();
                                        break;
                                    case "4":
                                        customerManager.ShowBasket();
                                        break;
                                    case "5":
                                        customerManager.ConfirmBasket();
                                        break;
                                    case "6":
                                        customerManager.ShowOrders();
                                        break;
                                    case "7":
                                        customerManager.RemoveFromBasket();
                                        break;
                                    case "8":
                                        customerManager.CancelOrder();
                                        break;
                                    case "9":
                                        customerManager.GiveFeedBack();
                                        break;
                                    case "10":
                                        customerManager.ConfirmDelivery();
                                        break;
                                    case "11":
                                        isLoggedIn = false;
                                        break;
                                    case "12":
                                        isLoggedIn = false;
                                        isexit=true;
                                        break;
                                    default:
                                        ErrorOperation("Invalid command!");
                                        break;
                                }
                                if (!isLoggedIn)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        ErrorOperation("Invalid username or password!");
                    }
                }
                else
                {
                    ErrorOperation("Invalid username or password!");
                }
            }
            
        }
    }
    static void SuccessOperation(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    static void ErrorOperation(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

}
