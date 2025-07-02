namespace MediCareCMS.Models
{

    public class PrescribedMedicine
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }  // Optional for display
        public string Dosage { get; set; }
        public string Duration { get; set; }
    }

}