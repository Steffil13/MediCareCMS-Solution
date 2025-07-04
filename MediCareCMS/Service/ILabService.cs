using MediCareCMS.Models;
namespace MediCareCMS.Services
{
    public interface ILabService
    {
        List<LabTestRequest> GetAssignedTests(string empId, string? doctorFilter);
        void MarkTestCompleted(int requestId);
        void RecordResult(TestResults result);
        List<TestResults> GetAllResults(string empId);
        List<TestResults> GetPatientHistory(string patientId);
        List<LabBill> GetBills(string empId);
    }
}
