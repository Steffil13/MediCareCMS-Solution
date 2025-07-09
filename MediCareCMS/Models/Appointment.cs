namespace MediCareCMS.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
       
       public string Name { get; set; }
      
        public int Token { get; set; }
        public bool IsConsulted { get; set; }
    }
}
