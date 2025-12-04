using PresenceProject.Core.DTOs;
using PresenceProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenceProject.Core.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto> AddAsync( UserPostModel usePostModel);
        Task<UserDto> UpdateAsync(UserDto userDto, int id);
        Task<bool> DeleteAsync(int id);
    }
}
