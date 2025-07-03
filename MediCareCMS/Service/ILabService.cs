using MediCareCMS.Models;

namespace MediCareCMS.Services
{
    public interface ILabService
    {
        List<LabTestRequest> GetAssignedTests(string empId);
        void MarkTestCompleted(int requestId);
        void RecordTestResult(TestResults result);
        List<TestResults> GetPatientHistory(string patientId);
        List<LabBill> GetBills(string empId);
    }
}
