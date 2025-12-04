using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenceProject.Core.DTOs
{
    public class PresenceDto
    {
        public DateOnly Date { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
        public int UserId { get; set; }
        public float TotalHours { get; set; }

    }
}
