namespace MediCareCMS.Models
{
    public class Prescription
    {
        public string AppointmentId { get; set; }
        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }
        public List<PrescribedMedicine> Medicines { get; set; } = new();
    }
}
