namespace MediCareCMS.ViewModel
{
    public class BillViewModel
    {
        public int AppointmentId { get; set; }
        public string AppointmentNumber { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string Department { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public int Token { get; set; }
        public decimal Fee { get; set; } = 300; // Flat consultation fee
        public DateTime GeneratedDate { get; set; }

    }
}
