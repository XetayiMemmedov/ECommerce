using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public static class MessageHelper
    {
        public static void SuccessOperation(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();  
        }

        public static void ErrorOperation(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;  
            Console.WriteLine(message);
            Console.ResetColor();  
        }
    }
}
