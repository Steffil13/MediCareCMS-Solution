namespace MediCareCMS.Models
{
    public class Appointment
    {
        public string AppointmentId { get; set; }
        public string PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
       // public string appId { get; set; }
        //public string DoctorName { get; set; }
       public string Name { get; set; }
       // public string DeptName { get; set; }
        public int Token { get; set; }
        public bool IsConsulted { get; set; }
    }
}
