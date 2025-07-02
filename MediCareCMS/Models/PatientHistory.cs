namespace MediCareCMS.Models
{
    public class PatientHistory
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string PastDisease { get; set; }
        public string MedicineTaken { get; set; }
        public DateTime RecordedDate { get; set; }
    }
}
