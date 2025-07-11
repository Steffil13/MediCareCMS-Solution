namespace MediCareCMS.Models
{
    public class PatientHistory
    {
        public int HistoryId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int Age { get; set; }
        public string Contact { get; set; }
        public string Disease { get; set; }
        public string Medicines { get; set; }
        public int DoctorId { get; set; }
        public DateTime DateOfConsultation { get; set; }
        public string TestName { get; set; }
    }
  }

