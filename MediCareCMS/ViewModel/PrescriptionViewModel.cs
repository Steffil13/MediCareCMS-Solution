using MediCareCMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MediCareCMS.ViewModel
{
    public class PrescriptionViewModel
    {
        public string AppointmentId { get; set; }
        public int Token { get; set; }
        public string PatientName { get; set; }
        public string Time { get; set; }
        public bool IsConsulted { get; set; }

        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }

        public int SelectedMedicineId { get; set; }
        public string SelectedDosage { get; set; }
        public string SelectedDuration { get; set; }

        public List<SelectListItem> Medicines { get; set; }
        public List<SelectListItem> Dosages { get; set; }
        public List<SelectListItem> Durations { get; set; }

        public PatientSummary PatientSummary { get; set; }

    }
}
