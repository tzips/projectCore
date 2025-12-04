namespace PresenceProject.Core.Models
{

    public class Presence
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
        public User User { get; set; }




        public float TotalHours()
        {
            return (float)(End.ToTimeSpan() - Start.ToTimeSpan()).TotalHours;
        }
    }
}
