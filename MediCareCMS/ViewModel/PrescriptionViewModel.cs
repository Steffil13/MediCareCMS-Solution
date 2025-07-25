﻿using MediCareCMS.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MediCareCMS.ViewModel
{
    public class PrescriptionViewModel
    {
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Symptoms are required")]
        public string Symptoms { get; set; }

        [Required(ErrorMessage = "Diagnosis is required")]
        public string Diagnosis { get; set; }

        public bool IsConsulted { get; set; }

        public List<PrescribedMedicine> PrescribedMedicines { get; set; } = new();
        public bool IsLabTestRequired { get; set; }
        public List<SelectListItem> LabTestList { get; set; } = new();
        public List<string> SelectedLabTestIds { get; set; } = new();

        public List<int> SelectedLabTestId { get; set; } = new List<int>();

        public List<int> SelectedMedicineIds { get; set; } = new();
        public List<string> SelectedDosages { get; set; } = new();
        public List<string> SelectedDurations { get; set; } = new();

        // These are used only for rendering dropdowns or display
        [BindNever]
        [ValidateNever]
        public List<SelectListItem> Medicines { get; set; } = new();

        [BindNever]
        [ValidateNever]
        public List<SelectListItem> Dosages { get; set; } = new();

        [BindNever]
        [ValidateNever]
        public List<SelectListItem> Durations { get; set; } = new();

        [BindNever]
        [ValidateNever]
        public List<SelectListItem> LabTests { get; set; } = new();

        [BindNever]
        [ValidateNever]
        public string PatientName { get; set; }

        [BindNever]
        [ValidateNever]
        public string Time { get; set; }

        [BindNever]
        [ValidateNever]
        public PatientSummary PatientSummary { get; set; }

        public int Token { get; set; }
        public int PatientId { get; set; }

        [BindNever]
        [ValidateNever]
        public int Age { get; set; }

        [BindNever]
        [ValidateNever]
        public string Contact { get; set; }

        [ValidateNever]
        public int DoctorId { get; set; }
    }
}