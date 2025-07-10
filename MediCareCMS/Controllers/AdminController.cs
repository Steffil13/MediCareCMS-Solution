using MediCareCMS.Models;
using MediCareCMS.Service;
using MediCareCMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MediCareCMS.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IDoctorService _doctorService;

        public AdminController(IAdminService adminService, IDoctorService doctorService)
        {
            _adminService = adminService;
            _doctorService = doctorService;
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
            return View(_adminService.GetReceptionists());
        }

        public IActionResult ManageDoctors()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Error = TempData["Error"];
            ViewBag.Departments = _doctorService.GetAllDepartments()
                .Select(d => new SelectListItem
                {
                    Value = d.DepartmentId.ToString(),
                    Text = d.DepartmentName
                }).ToList();

            return View(_adminService.GetDoctors());
        }

        public IActionResult ManagePharmacists()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Error = TempData["Error"];
            return View(_adminService.GetPharmacists());
        }

        public IActionResult ManageLabTechnicians()
        {
            var labs = _adminService.GetLabTechnicians();
            var tests = _adminService.GetLabTests(); // Fetch tests

            ViewBag.LabTests = tests;
            return View(labs);
        }


        // ================= ADD DOCTOR FORM (GET) ===================
        //[HttpGet]
        //public IActionResult AddDoctorForm()
        //{
        //    var viewModel = new StaffCreateViewModel
        //    {
        //        DepartmentList = _doctorService.GetAllDepartments()
        //            .Select(d => new SelectListItem
        //            {
        //                Value = d.DepartmentId.ToString(),
        //                Text = d.DepartmentName
        //            }).ToList()
        //    };

        //    return View("AddDoctorForm", viewModel);
        //}

        // ================= ADD DOCTOR ===================
        [HttpPost]
        public IActionResult AddDoctor(StaffCreateViewModel doctor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _adminService.AddDoctor(doctor);
                    TempData["Message"] = "Doctor added successfully.";
                    return RedirectToAction("ManageDoctors");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error adding doctor: " + ex.Message;
                    return RedirectToAction("ManageDoctors"); // 👈 Don't return AddDoctorForm
                }
            }

            // 👇 Populate dropdown if you want to re-render form (but you're not doing that now)
            doctor.DepartmentList = _doctorService.GetAllDepartments()
                .Select(d => new SelectListItem
                {
                    Value = d.DepartmentId.ToString(),
                    Text = d.DepartmentName
                }).ToList();

            TempData["Error"] = "Invalid data for doctor.";
            return RedirectToAction("ManageDoctors"); // 👈 Not View("AddDoctorForm")
        }




        // ================= ADD OTHERS ===================
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

        [HttpPost]
        public IActionResult AddMedicine(MedicineViewModel m)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["Error"] = "Invalid medicine data: " + string.Join(", ", errors);
                return RedirectToAction("ManagePharmacists");
            }

            try
            {
                _adminService.AddMedicine(m);
                TempData["Message"] = "Medicine added successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
            }

            return RedirectToAction("ManagePharmacists");
        }


        [HttpPost]
        public IActionResult AddLabTest(LabTestViewModel test)
        {
            if (ModelState.IsValid)
            {
                _adminService.AddLabTest(test);
                TempData["Message"] = "Test added successfully.";
            }
            else
            {
                TempData["Error"] = "Invalid test data.";
            }
            return RedirectToAction("ManageLabTechnicians");
        }

        public IActionResult DeleteLabTest(int id)
        {
            _adminService.SoftDeleteLabTest(id);
            TempData["Message"] = "Test deleted (soft) successfully.";
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


        // View: Manage Departments
        public IActionResult ManageDepartments()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Error = TempData["Error"];
            var departments = _doctorService.GetAllDepartments();
            return View(departments);
        }

        // Post: Add Department
        [HttpPost]
        public IActionResult AddDepartment(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _doctorService.AddDepartment(new Department
                    {
                        DepartmentName = model.DepartmentName
                    });
                    TempData["Message"] = "Department added successfully.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error adding department: " + ex.Message;
                }
            }
            else
            {
                TempData["Error"] = "Invalid department name.";
            }

            return RedirectToAction("ManageDepartments");
        }
        // ================= LOGOUT ===================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
    }
}
