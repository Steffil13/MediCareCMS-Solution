using Microsoft.AspNetCore.Mvc;
using MediCareCMS.Service;
using MediCareCMS.Models;
using MediCareCMS.ViewModel;

namespace MediCareCMS.Controllers
{
    public class ReceptionistController : Controller
    {
        private readonly IReceptionistService _receptionistService;
        private readonly IAdminService _adminService;
        private readonly IDoctorService _doctorService;

        public ReceptionistController(IReceptionistService receptionistService, IAdminService adminService, IDoctorService doctorService)
        {
            _receptionistService = receptionistService;
            _adminService = adminService;
            _doctorService = doctorService;
        }

        // ========== DASHBOARD ==========
        public IActionResult ReceptionistDashboard()
        {
            return View();
        }

        // ========== PATIENT MANAGEMENT ==========

        public IActionResult ManagePatients()
        {
            var patients = _receptionistService.GetAllPatients();
            return View("ManagePatients", patients);
        }

        public IActionResult Patients()
        {
            var patients = _receptionistService.GetAllPatients();
            return View(patients);
        }

        [HttpPost]
        public IActionResult AddPatient(Patient patient)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _receptionistService.AddPatient(patient);
                    TempData["Message"] = "Patient added.";
                    return RedirectToAction("ManagePatients");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error while saving patient: " + ex.Message);
                }
            }

            var patients = _receptionistService.GetAllPatients();
            return View("ManagePatients", patients);
        }

        [HttpPost]
        public IActionResult EditPatient(Patient patient)
        {
            if (ModelState.IsValid)
            {
                _receptionistService.UpdatePatient(patient);
                TempData["Message"] = "Patient updated.";
            }
            else
            {
                TempData["Error"] = "Invalid patient data.";
            }
            return RedirectToAction("ManagePatients");
        }

        public IActionResult DeletePatient(int id)
        {
            _receptionistService.DeletePatient(id);
            TempData["Message"] = "Patient deleted.";
            return RedirectToAction("ManagePatients");
        }

        // ========== NEW: APPOINTMENT SEARCH FLOW ==========

        // Show search box and list of patients (default: top 10)
        public IActionResult ManageAppointments(string keyword = "")
        {
            List<Patient> patients = string.IsNullOrWhiteSpace(keyword)
                ? _receptionistService.GetAllPatients().Take(10).ToList()
                : _receptionistService.SearchPatients(keyword);

            return View("ManageAppointments", patients); // Modified view with search + patient list
        }

        // Show appointment booking modal for selected patient
        public IActionResult BookAppointment(int patientId)
        {
            var patient = _receptionistService.GetPatientById(patientId);
            var model = new AppointmentBookingViewModel
            {
                PatientId = patient.PatientId,
                PatientName = patient.Name,
                Address = patient.Address,
                Doctors = _doctorService.GetAllDoctors()
            };
            return PartialView("_BookAppointmentModal", model); // Load modal with pre-filled data
        }

        // Post: Save booked appointment
        [HttpPost]
        public IActionResult BookAppointment(AppointmentBookingViewModel model)
        {
            // Show ModelState errors (useful during debugging)
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    TempData["Error"] += $"<br/>Field: {state.Key} — {error.ErrorMessage}";
                }
            }

            if (ModelState.IsValid)
            {
                var appointmentVm = new AppointmentViewModel
                {
                    PatientId = model.PatientId,
                    DoctorId = model.DoctorId,
                    Date = model.Date,
                    Time = model.Time
                };

                var appointmentId = _receptionistService.AddAppointment(appointmentVm); // Will return ID
                TempData["Message"] = "Appointment booked successfully!";
                return RedirectToAction("ManageAppointments");
            }

            // Reload doctors if validation fails
            model.Doctors = _doctorService.GetAllDoctors();
            return PartialView("_BookAppointmentModal", model);
        }

        // ========== BILLING ==========
        public IActionResult GenerateBill(int id)
        {
            var bill = _receptionistService.GetBillByAppointmentId(id);
            if (bill == null)
            {
                TempData["Error"] = "Bill not found.";
                return RedirectToAction("ManageAppointments");
            }
            return View("Bill", bill);
        }

        // ========== LEGACY (If You Still Want Table of All Appointments) ==========

        public IActionResult Appointments()
        {
            var appointments = _receptionistService.GetAllAppointments();
            return View(appointments);
        }

        [HttpPost]
        public IActionResult EditAppointment(AppointmentViewModel appointment)
        {
            if (ModelState.IsValid)
            {
                _receptionistService.UpdateAppointment(appointment);
                TempData["Message"] = "Appointment updated.";
            }
            else
            {
                TempData["Error"] = "Invalid appointment data.";
            }
            return RedirectToAction("Appointments");
        }

        public IActionResult DeleteAppointment(int appointmentId)
        {
            _receptionistService.DeleteAppointment(appointmentId);
            TempData["Message"] = "Appointment deleted.";
            return RedirectToAction("Appointments");
        }
    }
}
