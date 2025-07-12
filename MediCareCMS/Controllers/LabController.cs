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
        public IActionResult AssignedLabTests()
        {
            var list = _svc.GetAssignedTests();
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
        public JsonResult AjaxSaveResult([FromBody] TestResults result)
        {
            try
            {
                _svc.SaveTestResult(result);                   // 1. Save test result
                _svc.GenerateLabBill(result.RequestId);        // 2. Generate bill for that test
                return Json(new { ok = true });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, error = ex.Message });
            }
        }

        /* ────────────────── AJAX: Get Bill by RequestId ────────────────── */
        [HttpGet]
        public JsonResult GetBillByRequestId(int requestId)
        {
            var bill = _svc.GetBillByRequestId(requestId);
            if (bill == null)
                return Json(new { ok = false });

            return Json(new
            {
                ok = true,
                bill = new
                {
                    bill.BillId,
                    bill.Amount,
                    bill.Status
                }
            });
        }

        /* ─────────────────── Test Results Record page ───────────────── */
        public IActionResult RecordTestResults()
        {
            string empId = HttpContext.Session.GetInt32("UserId")?.ToString() ?? "";
            var results = _svc.GetAllResults(empId);
            return View(results);
        }

        /* ──────────────────── Patient History Search ────────────────── */
        public IActionResult PatientHistory() => View();

        [HttpPost]
        public IActionResult PatientHistory(string patientId)
        {
            var hist = _svc.GetPatientHistory(patientId);
            return View("PatientHistoryResult", hist);
        }

        /* ──────────────────── Bill History page ─────────────────────── */
        public IActionResult BillHistory()
        {
            string empId = HttpContext.Session.GetInt32("UserId")?.ToString() ?? "";
            return View(_svc.GetBills(empId));
        }

        /* ───────────────────────── Logout ──────────────────────────── */
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
    }
}
