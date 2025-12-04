using PresenceProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenceProject.Core.DTOs
{
    public class LeaveRequestDto
    {
        public int ReqId { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveType LeaveType { get; set; } // הצגה כטקסט ברור
    }
}
