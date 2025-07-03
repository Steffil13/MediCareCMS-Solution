using MediCareCMS.Models;
using MediCareCMS.Service;
using MediCareCMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MediCareCMS.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorService doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            this.doctorService = doctorService;
        }

        // ========================== TODAY'S APPOINTMENTS ==========================
        public IActionResult TodayAppointments(int doctorId)
        {
            var today = DateTime.Today;
            var appointments = doctorService.GetAppointmentsByDate(doctorId, today);
            ViewBag.DoctorId = doctorId;
            ViewBag.AppointmentDateType = "Today";
            return View("DoctorDashboard", appointments);
        }

        public IActionResult TomorrowAppointments(int doctorId)
        {
            var tomorrow = DateTime.Today.AddDays(1);
            var appointments = doctorService.GetAppointmentsByDate(doctorId, tomorrow);
            ViewBag.DoctorId = doctorId;
            ViewBag.AppointmentDateType = "Tomorrow";
            return View("DoctorDashboard", appointments);
        }

        // ========================== CONSULT GET ==========================
        [HttpGet]
        public IActionResult Consult(string appointmentId)
        {
            var appointment = doctorService.GetAppointmentById(appointmentId);

            if (appointment == null || appointment.IsConsulted)
                return RedirectToAction("TodayAppointments", new { doctorId = appointment?.DoctorId });

            var patientSummary = doctorService.GetPatientSummary(appointment.PatientId);

            var viewModel = new PrescriptionViewModel
            {
                AppointmentId = appointment.AppointmentId,
                Token = appointment.Token,
                Time = appointment.Time,
                PatientName = appointment.Name,
                IsConsulted = appointment.IsConsulted,
                PatientSummary = patientSummary,

                Medicines = doctorService.GetMedicineInventory()
                    .Select(m => new SelectListItem { Value = m.MedicineId.ToString(), Text = m.MedicineName }).ToList(),

                Dosages = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Once a day", Text = "Once a day" },
                    new SelectListItem { Value = "Twice a day", Text = "Twice a day" },
                    new SelectListItem { Value = "Thrice a day", Text = "Thrice a day" }
                },

                Durations = new List<SelectListItem>
                {
                    new SelectListItem { Value = "3 days", Text = "3 days" },
                    new SelectListItem { Value = "5 days", Text = "5 days" },
                    new SelectListItem { Value = "7 days", Text = "7 days" }
                }
            };

            return View("Consult", viewModel);
        }

        // ========================== CONSULT POST ==========================
        [HttpPost]
        
        public IActionResult Consult(PrescriptionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Reload dropdowns
                model.Medicines = doctorService.GetMedicineInventory()
                    .Select(m => new SelectListItem { Value = m.MedicineId.ToString(), Text = m.MedicineName }).ToList();

                model.Dosages = new List<SelectListItem>
        {
            new SelectListItem { Value = "Once a day", Text = "Once a day" },
            new SelectListItem { Value = "Twice a day", Text = "Twice a day" },
            new SelectListItem { Value = "Thrice a day", Text = "Thrice a day" }
        };

                model.Durations = new List<SelectListItem>
        {
            new SelectListItem { Value = "3 days", Text = "3 days" },
            new SelectListItem { Value = "5 days", Text = "5 days" },
            new SelectListItem { Value = "7 days", Text = "7 days" }
        };

                // ✅ Reload appointment details to fix Token 0, missing PatientName, Time
                var appointment = doctorService.GetAppointmentById(model.AppointmentId);
                if (appointment != null)
                {
                    model.Token = appointment.Token;
                    model.Time = appointment.Time;
                    model.PatientName = appointment.Name;
                    model.IsConsulted = appointment.IsConsulted;
                    model.PatientSummary = doctorService.GetPatientSummary(appointment.PatientId);
                }

                return View("Consult", model);
            }

            var prescription = new Prescription
            {
                AppointmentId = model.AppointmentId,
                Symptoms = model.Symptoms,
                Diagnosis = model.Diagnosis,
                Medicines = new List<PrescribedMedicine>()
            };

            // Loop through multiple medicine selections
            for (int i = 0; i < model.SelectedMedicineIds.Count; i++)
            {
                prescription.Medicines.Add(new PrescribedMedicine
                {
                    MedicineId = model.SelectedMedicineIds[i],
                    Dosage = model.SelectedDosages[i],
                    Duration = model.SelectedDurations[i]
                });
            }

            // ✅ Save prescription
            doctorService.SavePrescription(prescription);

            // ✅ Mark the appointment as consulted
            doctorService.MarkAppointmentAsConsulted(model.AppointmentId);

            TempData["Success"] = "Prescription saved successfully!";
            return RedirectToAction("TodayAppointments", new { doctorId = doctorService.GetAppointmentById(model.AppointmentId).DoctorId });
        }


        // ========================== VIEW DOCTOR SCHEDULE ==========================
        public IActionResult DoctorSchedule(int doctorId)
        {
            var schedule = doctorService.GetDoctorSchedule(doctorId);
            ViewBag.DoctorId = doctorId;
            return View(schedule);
        }

        // ========================== UPDATE DOCTOR AVAILABILITY ==========================
        [HttpPost]
        public IActionResult UpdateDoctorSchedule(int doctorId, DateTime date, bool isAvailable)
        {
            doctorService.UpdateDoctorSchedule(doctorId, date, isAvailable);
            return RedirectToAction("DoctorSchedule", new { doctorId });
        }
    }
}