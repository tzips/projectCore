using PresenceProject.Core.Models;

namespace PresenceProject.Models
{
  
        public class UserPostModel
        {
            public string FirstName { get; set; } // Auto-property
            public string LastName { get; set; }
            public string Password { get; set; } // Auto-property
            public string Email { get; set; } // Auto-property
            public EDuty Duty { get; set; }
        }
    }
