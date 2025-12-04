using PresenceProject.Core.Models;

namespace PresenceProject.Core.DTOs
{
    public class RegisterModel
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }    // ← חדש
        public string LastName { get; set; }     // ← חדש
        public EDuty Duty { get; set; }          // ← אם עוד לא הוספת
    }
}
