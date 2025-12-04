using System.Text.Json.Serialization;

namespace PresenceProject.Core.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EDuty
    {
        Administer,
        Employee
    }
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Presence> Presence { get; set; }
        public List<LeaveRequest> LeaveRequests { get; set; }

        public EDuty Duty { get; set; }
    }
}

