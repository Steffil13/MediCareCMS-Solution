namespace MediCareCMS.Models
{
    public class WeeklySchedule
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public DayOfWeek Day { get; set; }
        public bool IsAvailable { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}