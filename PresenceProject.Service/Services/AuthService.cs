using AutoMapper;
using Microsoft.Extensions.Logging;
using PresenceProject.Core.DTOs;
using PresenceProject.Core.Interfaces;
using PresenceProject.Core.Models;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PresenceProject.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IMapper mapper, ITokenService tokenService, IUserRepository userRepository, ILogger<AuthService> logger)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<string> RegisterAsync(RegisterModel model)
        {
            try
            {
                if (await _userRepository.EmailExistsAsync(model.Email))
                {
                    _logger.LogWarning("ניסיון הרשמה עם אימייל שכבר קיים: {Email}", model.Email);
                    return "Email already exists.";
                }

                var user = _mapper.Map<User>(model);
                user.Password = HashPassword(model.Password);

                await _userRepository.AddAsync(user);

                _logger.LogInformation("משתמש חדש נרשם בהצלחה: {Email}", model.Email);
                return "Registration successful.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "שגיאה במהלך תהליך ההרשמה");
                return "An error occurred during registration.";
            }
        }

        public async Task<string> LoginAsync(LoginModel model)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(model.Email);
                if (user == null || !VerifyPassword(model.Password, user.Password))
                {
                    _logger.LogWarning("ניסיון התחברות כושל עבור האימייל: {Email}", model.Email);
                    return "Invalid email or password.";
                }

                _logger.LogInformation("התחברות מוצלחת עבור המשתמש: {Email}", model.Email);
                return _tokenService.CreateToken(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "שגיאה במהלך התחברות");
                return "An error occurred during login.";
            }
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            var inputHashed = HashPassword(inputPassword);
            return inputHashed == hashedPassword;
        }
    }
}


