using MediCareCMS.Models;
using MediCareCMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace MediCareCMS.Controllers
{
    public class LabController : Controller
    {
        private readonly ILabService _svc;
        public LabController(ILabService svc) => _svc = svc;

        /* ───────────────────────── Dashboard ───────────────────────── */
        public IActionResult LabDashboard() => View();

        /* ──────────────────── Assigned‐Tests page ──────────────────── */
        // /Lab/AssignedLabTests?doctorId=D001
        public IActionResult AssignedLabTests(string? doctorId)
        {
            string empId = HttpContext.Session.GetInt32("UserId")?.ToString() ?? "";
            var list = _svc.GetAssignedTests(empId, doctorId);
            ViewBag.Filter = doctorId;
            return View(list);
        }

        [HttpPost]
        public IActionResult MarkTestDone(int requestId)
        {
            _svc.MarkTestCompleted(requestId);
            return RedirectToAction(nameof(AssignedLabTests));
        }

        /* ───────────── AJAX Endpoint to record result inline ─────────── */
        [HttpPost]
        public IActionResult AjaxSaveResult([FromBody] TestResults res)
        {
            if (res == null || res.RequestId <= 0)
                return BadRequest("Invalid result data");

            res.RecordedDate = DateTime.Now;
            _svc.RecordResult(res);               
            _svc.MarkTestCompleted(res.RequestId); 
            return Json(new { ok = true });
        }

        /* ─────────────────── Test Results Record page ───────────────── */
        //  << formerly TestRecord() — now named RecordTestResults >>
        public IActionResult RecordTestResults()
        {
            string empId = HttpContext.Session.GetInt32("UserId")?.ToString() ?? "";
            var results = _svc.GetAllResults(empId);
            return View(results);        // View = Views/Lab/RecordTestResults.cshtml
        }

        /* ──────────────────── Patient History Search ────────────────── */
        public IActionResult PatientHistory() => View();                 // View = Views/Lab/PatientHistory.cshtml

        [HttpPost]
        public IActionResult PatientHistory(string patientId)
        {
            var hist = _svc.GetPatientHistory(patientId);
            return View("PatientHistoryResult", hist);                   // View = Views/Lab/PatientHistoryResult.cshtml
        }

        /* ──────────────────── Bill History page ─────────────────────── */
        public IActionResult BillHistory()
        {
            string empId = HttpContext.Session.GetInt32("UserId")?.ToString() ?? "";
            return View(_svc.GetBills(empId));                           // View = Views/Lab/BillHistory.cshtml
        }

        /* ───────────────────────── Logout ──────────────────────────── */
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
    }
}
