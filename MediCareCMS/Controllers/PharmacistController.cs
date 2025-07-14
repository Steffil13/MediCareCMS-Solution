using MediCareCMS.Services;
using MediCareCMS.ViewModels.Pharmacist;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;

namespace MediCareCMS.Controllers
{
    public class PharmacistController : Controller
    {
        private readonly IPharmacistService _service;

        public PharmacistController(IPharmacistService service)
        {
            _service = service;
        }

        // ✅ Dashboard
        public IActionResult Dashboard()
        {
            ViewBag.Name = HttpContext.Session.GetString("PharmacistName") ?? "Pharmacist";
            return View();
        }

        public IActionResult DownloadBillPdf(int prescriptionId)
        {
            PharmacyBillVM bill = _service.GetBillByPrescriptionId(prescriptionId);
            if (bill == null)
                return NotFound();

            return new ViewAsPdf("BillPdf", bill)
            {
                FileName = $"PharmacyBill_{prescriptionId}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A5,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait
            };
        }


        // ✅ View Prescriptions
        public IActionResult ViewPrescriptions()
        {
            var prescriptions = _service.GetAllPrescriptions();
            return View(prescriptions);
        }

        // ✅ View Medicines for Prescription (AJAX Partial)
        public IActionResult ViewMedicines(int prescriptionId)
        {
            var meds = _service.GetMedicinesByPrescriptionId(prescriptionId);
            return PartialView("_MedicinesPartial", meds); // For modal
        }

        // ✅ Issue Medicines + Generate Bill
        [HttpPost]
        [HttpPost]
        public IActionResult Issue(int prescriptionId)
        {
            // Step 1: Get PharmacistId from session or fallback
            var pharmacistIdString = HttpContext.Session.GetString("PharmacistId");
            if (string.IsNullOrEmpty(pharmacistIdString))
                pharmacistIdString = "2"; // fallback for Vineesha

            int pharmacistId = Convert.ToInt32(pharmacistIdString);

            // Step 2: Issue medicines + generate bill
            _service.IssueMedicines(prescriptionId, pharmacistId);
            _service.GeneratePharmacyBill(prescriptionId, pharmacistId, 200); // test value

            return RedirectToAction("ViewBill", new { prescriptionId });

        }


        // ✅ Search Patient History (POST)
        [HttpPost]
        public IActionResult PatientHistory(string patientId)
        {
            var history = _service.GetPatientHistory(patientId);
            return View("PatientHistoryResult", history);
        }

        public IActionResult BillHistory()
        {
            var pharmacistId = Convert.ToInt32(HttpContext.Session.GetString("PharmacistId"));
            var bills = _service.GetBills(pharmacistId);
            return View(bills);
        }

        public IActionResult ViewBill(int prescriptionId)
        {
            var bill = _service.GetBillByPrescriptionId(prescriptionId);
            if (bill == null)
            {
                TempData["Message"] = "❌ Bill not found!";
                return RedirectToAction("ViewPrescriptions");
            }

            return View("ViewBill", bill); // This view shows the bill
        }




        // ✅ Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
    }
}
