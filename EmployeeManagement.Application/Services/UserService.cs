using EmployeeManagement.Application.DTOs.Users;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _hasher;
        public UserService(IUserRepository userRepository, IPasswordHasher<User> hasher)
        {
            _userRepository = userRepository;
            _hasher = hasher;            
        }
        public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
        {
            //validate role
            if (!Enum.IsDefined(typeof(UserRole), request.Role))
            {
                throw new ArgumentException("Invalid user role.");
            }

            //check whether email already exists
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            // validation employee information
            if (request.Role == UserRole.Employee)
            {
                if (string.IsNullOrWhiteSpace(request.FirstName) ||
                    string.IsNullOrWhiteSpace(request.LastName))
                {
                    throw new ArgumentException(
                       "First name and last name are required for employees.");
                }

                if (request.DepartmentId == null || request.DepartmentId<=0)
                {
                    throw new ArgumentException("A valid department is required for employees");
                }
                if (request.HireDate == null)
                {
                    throw new ArgumentException(
                        "Hire date is required for employees.");
                }

            }

            //create User Entity

            var user = new User
            {
                Name = request.Name.Trim(),
                Email = request.Email.Trim().ToLowerInvariant(),
                Role = request.Role,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
            };

            //Hash password

            user.PasswordHash = _hasher.HashPassword(user,request.Password);

            //create Employee profile if role is employee
            if (request.Role == UserRole.Employee)
            {
                user.Employee = new Employee
                { 
                    FirstName = request.FirstName!.Trim(),
                    LastName = request.LastName!.Trim(),
                    DepartmentId = request.DepartmentId!.Value,
                    HireDate = request.HireDate!.Value,
                    IsActive =true
                };
            }

            //save to database

            await _userRepository.AddAsync(user);

            var rowsAffected = await _userRepository.SavechangesAsync();

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException("User could not be created");
            }

            // return UserResponse dto

            return new UserResponse
            { 
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                EmployeeId = user.Employee?.Id,
            };

            
        }

        public async Task<UserResponse> GetUserByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid user Id");
            }

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) 
            {
                return null;
            }

            return new UserResponse 
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                EmployeeId = user.Employee?.Id
            };
        }
    }
}
