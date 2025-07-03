// ... existing usings ...
using MediCareCMS.Models;
using MediCareCMS.Service;
using MediCareCMS.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace MediCareCMS.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public IActionResult Index()
        {
            ViewBag.Receptionists = _adminService.GetReceptionists();
            ViewBag.Doctors = _adminService.GetDoctors();
            ViewBag.Pharmacists = _adminService.GetPharmacists();
            ViewBag.Lab = _adminService.GetLabTechnicians();
            return View();
        }

        // ================= MANAGE VIEWS ===================
        public IActionResult ManageReceptionists()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Error = TempData["Error"];
            var receptionists = _adminService.GetReceptionists();
            return View(receptionists);
        }

        public IActionResult ManageDoctors()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Error = TempData["Error"];
            var doctors = _adminService.GetDoctors();
            return View(doctors);
        }

        public IActionResult ManagePharmacists()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Error = TempData["Error"];
            var pharmacists = _adminService.GetPharmacists();
            return View(pharmacists);
        }

        public IActionResult ManageLabTechnicians()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Error = TempData["Error"];
            var labs = _adminService.GetLabTechnicians();
            return View(labs);
        }

        // ================= ADD ===================
        [HttpPost]
        public IActionResult AddReceptionist(StaffCreateViewModel receptionist)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _adminService.AddReceptionist(receptionist);
                    TempData["Message"] = "Receptionist added successfully.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error adding receptionist: " + ex.Message;
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                TempData["Error"] = "Invalid data for receptionist. " + string.Join(" | ", errors);
            }

            return RedirectToAction("ManageReceptionists");
        }

        [HttpPost]
        public IActionResult AddDoctor(StaffCreateViewModel doctor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _adminService.AddDoctor(doctor);
                    TempData["Message"] = "Doctor added successfully.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error adding doctor: " + ex.Message;
                }
            }
            else
            {
                TempData["Error"] = "Invalid data for doctor.";
            }

            return RedirectToAction("ManageDoctors");
        }

        [HttpPost]
        public IActionResult AddPharmacist(StaffCreateViewModel pharmacist)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _adminService.AddPharmacist(pharmacist);
                    TempData["Message"] = "Pharmacist added successfully.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error adding pharmacist: " + ex.Message;
                }
            }
            else
            {
                TempData["Error"] = "Invalid data for pharmacist.";
            }

            return RedirectToAction("ManagePharmacists");
        }

        [HttpPost]
        public IActionResult AddLabTechnician(StaffCreateViewModel lab)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _adminService.AddLabTechnician(lab);
                    TempData["Message"] = "Lab Technician added successfully.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error adding lab technician: " + ex.Message;
                }
            }
            else
            {
                TempData["Error"] = "Invalid data for lab technician.";
            }

            return RedirectToAction("ManageLabTechnicians");
        }

        // ================= EDIT ===================
        [HttpPost]
        public IActionResult EditReceptionist(StaffCreateViewModel receptionist)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _adminService.UpdateReceptionist(receptionist);
                    TempData["Message"] = "Receptionist updated successfully.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error updating receptionist: " + ex.Message;
                }
            }
            else
            {
                TempData["Error"] = "Invalid data for receptionist.";
            }

            return RedirectToAction("ManageReceptionists");
        }

        [HttpPost]
        public IActionResult EditDoctor(StaffCreateViewModel doctor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _adminService.UpdateDoctor(doctor);
                    TempData["Message"] = "Doctor updated successfully.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error updating doctor: " + ex.Message;
                }
            }
            else
            {
                TempData["Error"] = "Invalid data for doctor.";
            }

            return RedirectToAction("ManageDoctors");
        }

        [HttpPost]
        public IActionResult EditPharmacist(StaffCreateViewModel pharmacist)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _adminService.UpdatePharmacist(pharmacist);
                    TempData["Message"] = "Pharmacist updated successfully.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error updating pharmacist: " + ex.Message;
                }
            }
            else
            {
                TempData["Error"] = "Invalid data for pharmacist.";
            }

            return RedirectToAction("ManagePharmacists");
        }

        [HttpPost]
        public IActionResult EditLabTechnician(StaffCreateViewModel lab)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _adminService.UpdateLabTechnician(lab);
                    TempData["Message"] = "Lab Technician updated successfully.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error updating lab technician: " + ex.Message;
                }
            }
            else
            {
                TempData["Error"] = "Invalid data for lab technician.";
            }

            return RedirectToAction("ManageLabTechnicians");
        }

        // ================= DELETE ===================
        public IActionResult DeleteReceptionist(int id)
        {
            _adminService.DeactivateReceptionist(id);
            TempData["Message"] = "Receptionist deleted.";
            return RedirectToAction("ManageReceptionists");
        }

        public IActionResult DeleteDoctor(int id)
        {
            _adminService.DeactivateDoctor(id);
            TempData["Message"] = "Doctor deleted.";
            return RedirectToAction("ManageDoctors");
        }

        public IActionResult DeletePharmacist(int id)
        {
            _adminService.DeactivatePharmacist(id);
            TempData["Message"] = "Pharmacist deleted.";
            return RedirectToAction("ManagePharmacists");
        }

        public IActionResult DeleteLabTechnician(int id)
        {
            _adminService.DeactivateLabTechnician(id);
            TempData["Message"] = "Lab Technician deleted.";
            return RedirectToAction("ManageLabTechnicians");
        }

        //Logout

        public IActionResult Logout()
        {
            // Clear all session data 
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Login");
        }
    }
}
