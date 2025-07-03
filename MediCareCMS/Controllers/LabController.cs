using MediCareCMS.Models;
using MediCareCMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace MediCareCMS.Controllers
{
    public class LabController : Controller
    {
        private readonly ILabService _svc;
        public LabController(ILabService svc) => _svc = svc;

        // Dashboard
        public IActionResult LabDashboard() => View();

        // -------------------------------------------------
        public IActionResult AssignedLabTests()
        {
            string empId = HttpContext.Session.GetInt32("UserId")?.ToString() ?? "";
            if (string.IsNullOrEmpty(empId)) return RedirectToAction("Login", "Login");

            var tests = _svc.GetAssignedTests(empId);
            return View(tests);
        }

        [HttpPost]
        public IActionResult MarkTestDone(int requestId)
        {
            _svc.MarkTestCompleted(requestId);
            return RedirectToAction(nameof(AssignedLabTests));
        }

        // -------------------------------------------------
        public IActionResult RecordTestResults() => View();

        [HttpPost]
        public IActionResult RecordTestResults(TestResults result)
        {
            result.RecordedDate = DateTime.Now;
            _svc.RecordTestResult(result);
            TempData["Success"] = "Result saved & bill generated.";
            return RedirectToAction(nameof(RecordTestResults));
        }

        // -------------------------------------------------
        public IActionResult PatientHistory() => View(); // form

        [HttpPost]
        public IActionResult PatientHistory(string patientId)
        {
            var hist = _svc.GetPatientHistory(patientId);
            return View("PatientHistoryResult", hist);
        }

        // -------------------------------------------------
        public IActionResult BillHistory()
        {
            string empId = HttpContext.Session.GetInt32("UserId")?.ToString() ?? "";
            var bills = _svc.GetBills(empId);
            return View(bills);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
    }
}
