using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public UserType Role { get; set; }
        public string? Password { get; set; }
    }

    public class UserCreateDto
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required UserType Role { get; set; }
        public string? Email { get; set; }
    }

    public class UserUpdateDto
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required UserType Role { get; set; }
        public string? Email { get; set; }
    }
}
