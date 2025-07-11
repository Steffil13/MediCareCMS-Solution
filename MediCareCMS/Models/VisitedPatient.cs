namespace MediCareCMS.Models
{
    public class VisitedPatient
    {
        public int HistoryId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int Age { get; set; }
        public string Disease { get; set; }
        public string Medicines { get; set; }
        public string ContactNo { get; set; }
        public string TestName { get; set; }
        public DateTime DateOfConsultation { get; set; }
    }
}
