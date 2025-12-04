using AutoMapper;
using Microsoft.Extensions.Logging;
using PresenceProject.Core.DTOs;
using PresenceProject.Core.Interfaces;
using PresenceProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenceProject.Service.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<LeaveRequestService> _logger;

        public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository, IMapper mapper, ILogger<LeaveRequestService> logger)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LeaveRequestDto> GetByIdAsync(int id)
        {
            _logger.LogInformation($"שליפת בקשת חופשה לפי מזהה: {id}");
            var entity = await _leaveRequestRepository.GetByIdAsync(id);

            if (entity == null)
            {
                _logger.LogWarning($"לא נמצאה בקשת חופשה עם מזהה: {id}");
                return null;
            }

            return _mapper.Map<LeaveRequestDto>(entity);
        }

        public async Task<IEnumerable<LeaveRequestDto>> GetAllAsync()
        {
            _logger.LogInformation("שליפת כל בקשות החופשה");
            var entities = await _leaveRequestRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<LeaveRequestDto>>(entities);
        }

        public async Task<IEnumerable<LeaveRequestDto>> GetByUserIdAsync(int userId)
        {
            _logger.LogInformation($"שליפת בקשות חופשה לפי מזהה משתמש: {userId}");
            var entities = await _leaveRequestRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<LeaveRequestDto>>(entities);
        }

        public async Task<LeaveRequestDto> AddRequestAsync(LeaveRequestCreateModel model, int userId)
        {
            try
            {
                // כל הלוגיקה שלך
         
            var entity = _mapper.Map<LeaveRequest>(model);
            entity.UserId = userId;
            await _leaveRequestRepository.AddRequestAsync(entity);
            return _mapper.Map<LeaveRequestDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "שגיאה בהוספת נוכחות");
                throw;
            }
        }


        public async Task<LeaveRequestDto> UpdateRequestAsync(int id, LeaveRequestDto leaveRequestDto)
        {
            _logger.LogInformation($"עדכון בקשת חופשה עם מזהה: {id}");
            var entity = _mapper.Map<LeaveRequest>(leaveRequestDto);
            await _leaveRequestRepository.UpdateRequestAsync(id, entity);
            _logger.LogInformation("העדכון בוצע בהצלחה");
            return _mapper.Map<LeaveRequestDto>(entity);
        }

        public async Task DeleteRequestAsync(int id)
        {
            _logger.LogInformation($"מחיקת בקשת חופשה עם מזהה: {id}");
            try
            {
                await _leaveRequestRepository.DeleteRequestAsync(id);
                _logger.LogInformation("המחיקה בוצעה בהצלחה");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"שגיאה במחיקת בקשת חופשה עם מזהה: {id}");
                throw;
            }
        }
    }
}
