using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediCareCMS.ViewModels.Pharmacist
{
    public class PharmacistLoginVM
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class PrescriptionVM
    {
        public int PrescriptionId { get; set; }

        public string PatientName { get; set; }
        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<PrescribedMedicineVM> Medicines { get; set; } = new();
    }

    public class PrescribedMedicineVM
    {
        public string MedicineName { get; set; }
        public string Dosage { get; set; }
        public string Duration { get; set; }
    }

    public class PatientHistoryVM
    {
        public string PatientName { get; set; }
        public List<PrescriptionVM> Prescriptions { get; set; } = new();
    }

    public class PharmacyBillVM
    {
        public int BillId { get; set; }
        public string PatientName { get; set; }
        public int PrescriptionId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime IssuedDate { get; set; }
    }

    public class MedicineStockVM
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public int Quantity { get; set; }
    }
}
