using MediCareCMS.Models;

namespace MediCareCMS.Repositories
{
    public interface ILabRepository
    {
        List<LabTestRequest> GetAssignedTests(string labEmpId);
        void MarkTestCompleted(int requestId);
        void SaveTestResult(TestResults result);
        List<TestResults> GetPatientHistory(string patientId);
        List<LabBill> GetBills(string labEmpId);
    }
}
