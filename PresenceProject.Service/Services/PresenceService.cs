using AutoMapper;
using Microsoft.Extensions.Logging;
using PresenceProject.Core.DTOs;
using PresenceProject.Core.Interfaces;
using PresenceProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace PresenceProject.Service.Services
{
    public class PresenceService : IPresenceService
    {
        private readonly IPresenceRepository _presenceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PresenceService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PresenceService(IPresenceRepository presenceRepository, IMapper mapper, ILogger<PresenceService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _logger.LogInformation("PresenceService נוצר");
            _presenceRepository = presenceRepository;
            _mapper = mapper;
           
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userId, out var id) ? id : 0;
        }

        private bool IsAdmin()
        {
            return _httpContextAccessor.HttpContext?.User?.IsInRole("Admin") == true;
        }

        public async Task<PresenceDto> GetByIdAsync(int id)
        {
            _logger.LogInformation($"שליפת נוכחות לפי מזהה: {id}");

            var entity = await _presenceRepository.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning($"לא נמצאה נוכחות עם מזהה: {id}");
                return null;
            }

            return _mapper.Map<PresenceDto>(entity);
        }

        public async Task<IEnumerable<PresenceDto>> GetAllAsync()
        {
            _logger.LogInformation("שליפת כל רשומות הנוכחות");
            var entities = await _presenceRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PresenceDto>>(entities);
        }

        public async Task<IEnumerable<PresenceDto>> GetByUserIdAsync(int userId)
        {
            _logger.LogInformation($"שליפת נוכחות עבור משתמש עם מזהה: {userId}");
            var entities = await _presenceRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<PresenceDto>>(entities);
        }

        public async Task<float> CalculateTotalHoursAsync(int userId)
        {
            _logger.LogInformation($"חישוב סך כל השעות עבור משתמש: {userId}");
            var presences = await _presenceRepository.GetByUserIdAsync(userId);
            float totalHours = presences.Sum(p => p.TotalHours());
            _logger.LogInformation($"סך השעות שחושב: {totalHours}");
            return totalHours;
        }

        public async Task<Presence> AddPresenceAsync(Presence presence)
        {
            _logger.LogInformation("הוספת רשומת נוכחות חדשה");

            await _presenceRepository.AddPresenceAsync(presence);

            _logger.LogInformation($"נוכחות נוספה בהצלחה עם מזהה: {presence.Id}");
            return presence;
        }


        public async Task<PresenceDto> UpdatePresenceAsync(int id, PresenceDto presenceDto)
        {
            _logger.LogInformation($"עדכון נוכחות עם מזהה: {id}");

            if (!IsAdmin())
                throw new UnauthorizedAccessException("רק אדמין רשאי לעדכן רשומות נוכחות");

            var existing = await _presenceRepository.GetByIdAsync(id);
            if (existing == null)
                return null;

            // לא משנים את UserId
            presenceDto.UserId = existing.UserId;

            var entity = _mapper.Map<Presence>(presenceDto);
            var updated = await _presenceRepository.UpdatePresenceAsync(id, entity);

            _logger.LogInformation("העדכון בוצע בהצלחה");
            return _mapper.Map<PresenceDto>(updated);
        }

        public async Task DeletePresenceAsync(int id)
        {
            _logger.LogInformation($"מחיקת נוכחות עם מזהה: {id}");

            var currentUserId = GetCurrentUserId();
            var presence = await _presenceRepository.GetByIdAsync(id);

            if (presence == null)
                return;

            if (!IsAdmin() && presence.UserId != currentUserId)
                throw new UnauthorizedAccessException("אין הרשאה למחוק רשומה זו");

            await _presenceRepository.DeletePresenceAsync(id);
            _logger.LogInformation("המחיקה בוצעה בהצלחה");
        }
    }
}
