namespace MediCareCMS.Models
{
    public class Prescription
    {
        //public string AppointmentId { get; set; }
        //public string Symptoms { get; set; }
        //public string Diagnosis { get; set; }
        //public List<PrescribedMedicine> Medicines { get; set; } = new();

        public int PrescriptionId { get; set; }  // Optional for creation
        public int AppointmentId { get; set; }
        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<PrescribedMedicine> Medicines { get; set; } = new();
        public List<int> SelectedLabTestIds { get; set; } = new();


    }
}
