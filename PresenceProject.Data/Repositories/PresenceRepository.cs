using Microsoft.EntityFrameworkCore;
using PresenceProject.Core.Interfaces;
using PresenceProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenceProject.Data.Repositories
{
    public class PresenceRepository : IPresenceRepository
    {
        private readonly DbContext _context;

        public PresenceRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<Presence> GetByIdAsync(int id)
        {
            return await _context.Set<Presence>()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Presence>> GetAllAsync()
        {
            return await _context.Set<Presence>()
                .Include(p => p.User) // Include User if needed
                .ToListAsync();
        }

        public async Task<IEnumerable<Presence>> GetByUserIdAsync(int userId)
        {
            return await _context.Set<Presence>()
                .Where(p => p.UserId == userId)
                .Include(p => p.User) // Include User if needed
                .ToListAsync();
        }

        public async Task<Presence> AddPresenceAsync(Presence presence) // שינוי לשם הנכון
        {
            await _context.Set<Presence>().AddAsync(presence);
            await _context.SaveChangesAsync();
            return presence; // מחזירים את ה-Presence המתווסף
        }

        public async Task<Presence> UpdatePresenceAsync(int id, Presence presence) // שינוי לשם הנכון
        {
            var existingPresence = await GetByIdAsync(id);
            if (existingPresence == null)
            {
                return null; // אם לא נמצא, מחזירים null
            }

            // עדכון הערכים הדרושים
            existingPresence.Date = presence.Date;
            existingPresence.Start = presence.Start;
            existingPresence.End = presence.End;
            existingPresence.UserId = presence.UserId;

            _context.Set<Presence>().Update(existingPresence);
            await _context.SaveChangesAsync();

            return existingPresence; // מחזירים את ה-Presence המעודכן
        }

        public async Task DeletePresenceAsync(int id)
        {
            var presence = await GetByIdAsync(id);
            if (presence != null)
            {
                _context.Set<Presence>().Remove(presence);
                await _context.SaveChangesAsync();
            }
        }
    }
}
