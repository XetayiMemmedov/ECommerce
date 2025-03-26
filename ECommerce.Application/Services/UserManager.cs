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
    public class UserManager : IUserService
    {
        private readonly IUserRepository _repository;

        public UserManager(IUserRepository repository)
        {
            _repository = repository;
        }

        public void Add(UserCreateDto createDto)
        {
            var user = createDto.ToUser();

            _repository.Add(user);
        }

        public UserDto Get(Expression<Func<User, bool>> predicate)
        {
            var user = _repository.Get(predicate);

            var userDto = user.ToUserDto();

            return userDto;
        }
        
        public List<UserDto> GetAll(Expression<Func<User, bool>>? predicate = null, bool asNoTracking = false)
        {
            var users = _repository.GetAll(predicate, asNoTracking);

            var userDtoList = new List<UserDto>();

            foreach (var item in users)
            {
                userDtoList.Add(new UserDto
                {
                    Id = item.Id,
                    Name = item.UserName,
                    Email = item.Email,
                    Role = item.Role,
                    Password = item.Password

                });
            }

            return userDtoList;
        }

        public UserDto GetById(int id)
        {
            var user = _repository.GetById(id);

            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Role = user.Role,
            };

            return userDto;
        }

        public void Remove(int id)
        {
            var existEntity = _repository.GetById(id);

            if (existEntity == null) throw new Exception("Not found");

            _repository.Remove(existEntity);
        }

        public void Update(UserUpdateDto updateDto)
        {
            var user = new User
            {
                Id = updateDto.Id,
                UserName = updateDto.UserName,
                Email = updateDto.Email,
                Password = updateDto.Password,
                Role = updateDto.Role,
            };

            _repository.Update(user);
        }

        public string GetPasswordInput(string prompt)
        {
            Console.Write(prompt);
            string password = "";

            while (true)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }

                if (key.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password.Substring(0, password.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else if (!char.IsControl(key.KeyChar) && (char.IsLetterOrDigit(key.KeyChar) || char.IsPunctuation(key.KeyChar) || char.IsSymbol(key.KeyChar)))
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
            }

            return password;
        }
    }
}
