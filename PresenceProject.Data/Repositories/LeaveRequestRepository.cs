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
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly DbContext _context;

        public LeaveRequestRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<LeaveRequest> GetByIdAsync(int id)
        {
            return await _context.Set<LeaveRequest>()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<LeaveRequest>> GetAllAsync()
        {
            return await _context.Set<LeaveRequest>()
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetByUserIdAsync(int userId)
        {
            return await _context.Set<LeaveRequest>()
                .Where(p => p.UserId == userId)
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<LeaveRequest> AddRequestAsync(LeaveRequest leaveRequest)
        {
            await _context.Set<LeaveRequest>().AddAsync(leaveRequest);
            await _context.SaveChangesAsync();
            return leaveRequest;
        }

        public async Task<LeaveRequest> UpdateRequestAsync(int id, LeaveRequest leaveRequest)
        {
            var existingLeaveRequest = await GetByIdAsync(id);
            if (existingLeaveRequest == null)
            {
                return null;
            }

            // עדכון הערכים הדרושים
            existingLeaveRequest.Id = leaveRequest.Id;
            existingLeaveRequest.StartDate = leaveRequest.StartDate;
            existingLeaveRequest.EndDate = leaveRequest.EndDate;
            existingLeaveRequest.UserId = leaveRequest.UserId;

            _context.Set<LeaveRequest>().Update(existingLeaveRequest);
            await _context.SaveChangesAsync();

            return existingLeaveRequest;
        }

        public async Task DeleteRequestAsync(int id)
        {
            var leaveRequest = await GetByIdAsync(id);
            if (leaveRequest != null)
            {
                _context.Set<LeaveRequest>().Remove(leaveRequest);
                await _context.SaveChangesAsync();
            }
        }
    }
}
