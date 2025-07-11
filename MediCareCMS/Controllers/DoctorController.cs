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
        public IActionResult TodayAppointments()
        {
            int doctorId = HttpContext.Session.GetInt32("DoctorId") ?? 0;
            var today = DateTime.Today;

            Console.WriteLine("DoctorId (from session): " + doctorId);
            Console.WriteLine("Today: " + today.ToString("yyyy-MM-dd"));

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
        public IActionResult Consult(int appointmentId)
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
                },
                // ✅ Lab Test dropdown/checkbox list
                //   LabTestList = new List<SelectListItem>
                //{
                //   new SelectListItem { Text = "Blood Test", Value = "1" },
                //   new SelectListItem { Text = "Urine Test", Value = "2" },
                //   new SelectListItem { Text = "X-Ray", Value = "3" },
                //   new SelectListItem { Text = "ECG", Value = "4" },
                //   new SelectListItem { Text = "MRI", Value = "5" }
                //   } ,
                LabTestList = doctorService.GetAllLabTests()
    .Select(t => new SelectListItem
    {
        Value = t.TestId.ToString(),
        Text = t.TestName
    }).ToList(),

            };


            return View("Consult", viewModel);
        }

        // ========================== CONSULT POST ==========================
        [HttpPost]
        public IActionResult Consult(PrescriptionViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Please fill all required fields correctly." });
                }
                if (model.PrescribedMedicines == null || !model.PrescribedMedicines.Any())
                {
                    // Log this
                    Console.WriteLine("PrescribedMedicines is empty or null!");
                }


                var prescription = new Prescription
                {
                    AppointmentId = model.AppointmentId,
                    Symptoms = model.Symptoms,
                    Diagnosis = model.Diagnosis,
                    Medicines = model.PrescribedMedicines?.Select(m => new PrescribedMedicine
                    {
                        MedicineId = m.MedicineId,
                        Dosage = m.Dosage,
                        Duration = m.Duration
                    }).ToList() ?? new List<PrescribedMedicine>()
                };

                int prescriptionId = doctorService.SavePrescription(prescription);

                if (model.IsLabTestRequired && model.SelectedLabTestIds != null && model.SelectedLabTestIds.Any())
                {
                    var labTestIds = model.SelectedLabTestIds.Select(int.Parse).ToList();
                    doctorService.SavePrescriptionLabTests(prescriptionId, labTestIds);
                }


                doctorService.MarkAppointmentAsConsulted(model.AppointmentId);

                var appointment = doctorService.GetAppointmentById(model.AppointmentId);

                return Json(new
                {
                    success = true,
                    message = "Prescription saved successfully.",
                    doctorId = appointment.DoctorId  // Pass for redirect
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }





        private void RebindDropdowns(PrescriptionViewModel model)
        {
            // Rebind dropdowns
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

            model.LabTestList = doctorService.GetAllLabTests()
     .Select(t => new SelectListItem
     {
         Value = t.TestId.ToString(),
         Text = t.TestName
     }).ToList();


            // Try populate view-only fields, if data is available
            var appointment = doctorService.GetAppointmentById(model.AppointmentId);
            if (appointment != null)
            {
                model.Time = appointment?.Time ?? "N/A";
                model.PatientName = appointment?.Name ?? "Unknown";
                model.Token = appointment.Token;
                model.IsConsulted = appointment.IsConsulted;
                model.PatientSummary = doctorService.GetPatientSummary(appointment.PatientId);
            }
        }


        //[HttpGet]
        //public IActionResult PatientHistory(int doctorId, string searchTerm = "")
        //{
        //    var history = doctorService.GetPatientHistory(doctorId, searchTerm);
        //    ViewBag.DoctorId = doctorId;
        //    ViewBag.SearchTerm = searchTerm;
        //    return View(history);
        //}

        public IActionResult Dashboard()
        {
            string doctorIdStr = HttpContext.Session.GetString("EmpId");

            if (string.IsNullOrEmpty(doctorIdStr) || !int.TryParse(doctorIdStr, out int doctorId))
            {
                return RedirectToAction("Login", "Account");
            }

            var appointments = doctorService.GetAppointmentsByDate(doctorId, DateTime.Today);
            var patientHistory = doctorService.GetHistoryByDoctorId(doctorId);

            ViewBag.DoctorId = doctorId;
            ViewBag.PatientHistory = patientHistory;

            return View(appointments);
        }






        // ========================== VIEW DOCTOR SCHEDULE ==========================
        public IActionResult DoctorSchedule(int doctorId)
        {
            var schedule = doctorService.GetDoctorSchedule(doctorId);
            ViewBag.DoctorId = doctorId;
            return View(schedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSchedule(DoctorSchedule schedule)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return BadRequest("Invalid form data");

                return View(schedule); // for full view form
            }

            doctorService.Add(schedule);

            // Return JSON if AJAX
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Ok(); // triggers JS success callback
            }

            // Else regular redirect (non-AJAX form)
            return RedirectToAction("DoctorSchedule", new { doctorId = schedule.DoctorId });
        }



        // ========================== EDIT SCHEDULE ==========================
        [HttpGet]
        public IActionResult EditSchedule(int id)
        {
            var schedule = doctorService.GetScheduleById(id);
            if (schedule == null)
                return NotFound();

            return View(schedule);
        }

        [HttpPost]
        public IActionResult EditSchedule(DoctorSchedule schedule)
        {
            if (ModelState.IsValid)
            {
                doctorService.Update(schedule);
                return RedirectToAction("DoctorSchedule", new { doctorId = schedule.DoctorId });
            }

            return View(schedule);
        }
        // ========================== DELETE SCHEDULE ==========================
        [HttpGet]
        public IActionResult DeleteSchedule(int id)
        {
            var schedule = doctorService.GetScheduleById(id);
            if (schedule == null)
                return NotFound();

            return View(schedule); // for confirmation
        }

        [HttpPost]
        public IActionResult DeleteSchedule(int id, int doctorId)
        {
            doctorService.Delete(id);
            return RedirectToAction("DoctorSchedule", new { doctorId });
        }


       

        // ========================== BULK UPDATE (IF USED) ==========================
        [HttpPost]
        public IActionResult UpdateDoctorSchedule(int doctorId, DateTime date, bool isAvailable)
        {
            doctorService.UpdateDoctorSchedule(doctorId, date, isAvailable);
            return RedirectToAction("DoctorSchedule", new { doctorId });
        }
    }
}
