using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;
        public UserRepository(AppDbContext db)
        {
            _db = db;            
        }
        public async Task AddAsync(User user)
        {
            await _db.Users.AddAsync(user);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == email);
            return user;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var user = await _db.Users.Include(u=>u.Employee).FirstOrDefaultAsync(u=>u.Id==id);
            return user;
        }

        public async Task<int> SavechangesAsync()
        {
           return await _db.SaveChangesAsync();
        }
    }
}
