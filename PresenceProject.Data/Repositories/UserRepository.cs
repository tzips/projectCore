using Microsoft.EntityFrameworkCore;
using PresenceProject.Core.Interfaces;
using PresenceProject.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresenceProject.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Presence)
                .Include(u => u.LeaveRequests)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Presence)
                .Include(u => u.LeaveRequests)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(int id, User user)
        {
            var existingUser = await GetByIdAsync(id);
            if (existingUser == null)
                throw new KeyNotFoundException("User not found");

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;
            existingUser.Duty = user.Duty;

            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingUser = await GetByIdAsync(id);
            if (existingUser != null)
            {
                _context.Users.Remove(existingUser);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}

