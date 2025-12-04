using PresenceProject.Core.DTOs;
using PresenceProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenceProject.Core.Interfaces
{
    public interface IPresenceService
    {
        // פעולות שזמינות לכל משתמש
        Task<PresenceDto> GetByIdAsync(int id);
        Task<IEnumerable<PresenceDto>> GetAllAsync(); // רק לאדמין ברמת הקונטרולר
        Task<IEnumerable<PresenceDto>> GetByUserIdAsync(int userId);
        Task<float> CalculateTotalHoursAsync(int userId);
        Task<Presence> AddPresenceAsync(Presence presence);


        // פעולות שזמינות לאדמין בלבד
        Task<PresenceDto> UpdatePresenceAsync(int id, PresenceDto presenceDto);
        Task DeletePresenceAsync(int id);
    }
}
