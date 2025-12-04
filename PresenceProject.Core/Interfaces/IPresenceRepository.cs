using PresenceProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenceProject.Core.Interfaces
{
    public interface IPresenceRepository
    {
        Task<Presence> GetByIdAsync(int id);
        Task<IEnumerable<Presence>> GetAllAsync();
        Task<IEnumerable<Presence>> GetByUserIdAsync(int userId);
        Task<Presence> AddPresenceAsync(Presence presence);
        Task<Presence> UpdatePresenceAsync(int id, Presence presence);
        Task DeletePresenceAsync(int id);
    }
}
