namespace MediCareCMS.Models
{

    public class PrescribedMedicine
    {
        public int Id { get; set; }  // Optional if not needed for inserts
        public int PrescriptionId { get; set; }
        public int MedicineId { get; set; }
        public string Dosage { get; set; }
        public string Duration { get; set; }

        // Optional helper (for displaying medicine name in views)
        public string MedicineName { get; set; }
    }

}