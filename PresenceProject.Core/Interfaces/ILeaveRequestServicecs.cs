using PresenceProject.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PresenceProject.Core.Interfaces
{
    public interface ILeaveRequestService
    {
        Task<IEnumerable<LeaveRequestDto>> GetAllAsync();
        Task<LeaveRequestDto> GetByIdAsync(int id);
        Task<IEnumerable<LeaveRequestDto>> GetByUserIdAsync(int userId);

        // POST
        Task<LeaveRequestDto> AddRequestAsync(LeaveRequestCreateModel model, int userId);

        // PUT
        Task<LeaveRequestDto> UpdateRequestAsync(int id, LeaveRequestDto model);

        // DELETE
        Task DeleteRequestAsync(int id);
    }
}

