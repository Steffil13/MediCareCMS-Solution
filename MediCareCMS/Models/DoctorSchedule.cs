namespace MediCareCMS.Models
{
    public class DoctorSchedule
    {
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public bool IsAvailable { get; set; }
    }
}
