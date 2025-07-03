namespace MediCareCMS.Models
{
    public class DoctorSchedule
    {
        public int ScheduleId { get; set; }         // Optional if needed, based on DB identity
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public bool IsAvailable { get; set; }
    }
}
