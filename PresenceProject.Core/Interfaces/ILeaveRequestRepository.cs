using PresenceProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenceProject.Core.Interfaces
{
    public interface ILeaveRequestRepository
    {
        Task<LeaveRequest> GetByIdAsync(int id);
        Task<IEnumerable<LeaveRequest>> GetAllAsync();
        Task<IEnumerable<LeaveRequest>> GetByUserIdAsync(int userId);
        Task<LeaveRequest> AddRequestAsync(LeaveRequest leaveRequest);
        Task<LeaveRequest> UpdateRequestAsync(int id, LeaveRequest leaveRequest);
        Task DeleteRequestAsync(int id);
    }
}
