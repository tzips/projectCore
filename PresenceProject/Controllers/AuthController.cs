using Microsoft.AspNetCore.Authorization; // כבר קיים
using Microsoft.AspNetCore.Mvc;
using PresenceProject.Core.Interfaces;
using PresenceProject.Models;
using PresenceProject.Core.Interfaces;
using PresenceProject.Core.DTOs;

namespace PresenceProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            // *** התחלת בדיקת הולידציה של המודל ***
            // אם המודל לא תקין (לפי Data Annotations או בעיות Model Binding)
            // ModelState.IsValid יהיה false
            if (!ModelState.IsValid)
            {
                // רשום אזהרה בלוג שולידציה נכשלה
                _logger.LogWarning("Validation failed for user registration. Errors: {Errors}",
                    string.Join("; ", ModelState.Values
                                                .SelectMany(x => x.Errors)
                                                .Select(x => x.ErrorMessage)));

                // החזר 400 Bad Request עם פרטי השגיאות של הולידציה
                return BadRequest(ModelState);
            }
            // *** סיום בדיקת הולידציה של המודל ***


            _logger.LogInformation("Attempting to register user: {UserName}", model.Email);

            try
            {
                var token = await _authService.RegisterAsync(model);
                if (token == null)
                {
                    _logger.LogWarning("Registration failed - username already exists: {UserName}", model.Email);
                    return BadRequest("שם המשתמש כבר קיים");
                }

                _logger.LogInformation("User registered successfully: {UserName}", model.Email);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration for user: {UserName}", model.Email);
                return StatusCode(500, "אירעה שגיאה בלתי צפויה בשרת.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // ניתן להוסיף כאן גם בדיקת ModelState.IsValid ל-LoginModel אם יש ולידציות
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation failed for user login: {Errors}",
                    string.Join("; ", ModelState.Values
                                                .SelectMany(x => x.Errors)
                                                .Select(x => x.ErrorMessage)));
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Login attempt for user: {UserName}", model.Email);

            var token = await _authService.LoginAsync(model);
            if (token == null)
            {
                _logger.LogWarning("Login failed for user: {UserName}", model.Email);
                return Unauthorized();
            }

            _logger.LogInformation("Login successful for user: {UserName}", model.Email);
            return Ok(new { Token = token });
        }
    }
}
