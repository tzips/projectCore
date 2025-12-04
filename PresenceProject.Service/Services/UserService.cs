using AutoMapper;
using Microsoft.Extensions.Logging;
using PresenceProject.Core.DTOs;
using PresenceProject.Core.Interfaces;
using PresenceProject.Core.Models;
using PresenceProject.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PresenceProject.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            _logger.LogInformation("שליפת כל המשתמשים");
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation($"שליפת משתמש לפי מזהה: {id}");
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                _logger.LogWarning($"לא נמצא משתמש עם מזהה: {id}");
                return null;
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> AddAsync(UserPostModel userModel)
        {
            _logger.LogInformation("הוספת משתמש חדש");
            var user = _mapper.Map<User>(userModel);
            var newUser = await _userRepository.AddAsync(user);
            _logger.LogInformation($"נוסף משתמש חדש עם מזהה: {newUser.Id}");
            return _mapper.Map<UserDto>(newUser);
        }

        public async Task<UserDto> UpdateAsync(UserDto userDto, int id)
        {
            _logger.LogInformation($"עדכון משתמש עם מזהה: {id}");
            var user = _mapper.Map<User>(userDto);
            var updatedUser = await _userRepository.UpdateAsync(id, user);
            _logger.LogInformation("העדכון בוצע בהצלחה");
            return _mapper.Map<UserDto>(updatedUser);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation($"מחיקת משתמש עם מזהה: {id}");

            try
            {
                await _userRepository.DeleteAsync(id);
                _logger.LogInformation("המחיקה בוצעה בהצלחה");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"שגיאה במחיקת משתמש עם מזהה: {id}");
                return false;
            }
        }
    }
}

