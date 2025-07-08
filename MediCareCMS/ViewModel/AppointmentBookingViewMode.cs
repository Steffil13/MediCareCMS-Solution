using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MediCareCMS.Models;
using MediCareCMS.ViewModels; // Make sure this matches your namespace

namespace MediCareCMS.ViewModel
{
    public class AppointmentBookingViewModel
    {
        // From patient record (pre-filled)
        public int PatientId { get; set; }

        public string PatientName { get; set; }

        public string Address { get; set; }

        // Doctor selection
        [Required(ErrorMessage = "Please select a doctor.")]
        public int DoctorId { get; set; }

        // Appointment date (limit in controller to today/tomorrow if needed)
        [Required(ErrorMessage = "Please select a date.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        // Time input (string, e.g., "10:30")
        [Required(ErrorMessage = "Please enter appointment time.")]
        public string Time { get; set; }

        // Doctor dropdown list (populated in controller)
        public List<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
