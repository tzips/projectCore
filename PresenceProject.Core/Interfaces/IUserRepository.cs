using PresenceProject.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PresenceProject.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(int id, User user);
        Task<bool> DeleteAsync(int id);

        // נוספו עבור AuthService:
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
    }
}

