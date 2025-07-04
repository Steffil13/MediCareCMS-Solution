using MediCareCMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MediCareCMS.ViewModel
{
    public class PrescriptionViewModel
    {
        public string AppointmentId { get; set; }
        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }

        // NEW: Lists for multiple medicines
        public List<int> SelectedMedicineIds { get; set; } = new();
        public List<string> SelectedDosages { get; set; } = new();
        public List<string> SelectedDurations { get; set; } = new();

        // Dropdowns
        public List<SelectListItem> Medicines { get; set; }
        public List<SelectListItem> Dosages { get; set; }
        public List<SelectListItem> Durations { get; set; }

        // Appointment display
        public int Token { get; set; }
        public string Time { get; set; }
        public string PatientName { get; set; }
        public bool IsConsulted { get; set; }
        public int DoctorId { get; set; }


        // Summary
        public PatientSummary PatientSummary { get; set; }

        // Lab test fields
        public bool IsLabTestRequired { get; set; }
        public List<int> SelectedLabTestIds { get; set; } = new();
        public bool IsLabTestNeeded { get; set; }
        public int? SelectedLabTestId { get; set; }
        public List<SelectListItem> LabTests { get; set; }
        public List<PrescribedMedicine> PrescribedMedicines { get; set; } = new List<PrescribedMedicine>();
        public class PrescribedMedicine
        {
            public int MedicineId { get; set; }
            public string Dosage { get; set; }
            public string Duration { get; set; }
        }

    }
}