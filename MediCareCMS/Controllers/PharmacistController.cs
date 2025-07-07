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
        public IActionResult Issue(int prescriptionId)
        {
            var pharmacistId = Convert.ToInt32(HttpContext.Session.GetString("PharmacistId"));
            _service.IssueMedicines(prescriptionId, pharmacistId);

            // 💰 Optional: Replace with actual amount logic
            decimal totalAmount = 100;
            _service.GeneratePharmacyBill(prescriptionId, pharmacistId, totalAmount);

            TempData["Message"] = "Medicines Issued and Bill Generated!";
            return RedirectToAction("ViewPrescriptions");
        }

        // ✅ Search Patient History (GET)
        public IActionResult PatientHistory()
        {
            return View();
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


        // ✅ Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
    }
}
