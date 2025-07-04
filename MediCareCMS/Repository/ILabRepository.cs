using MediCareCMS.Models;
namespace MediCareCMS.Repositories
{
    public interface ILabRepository
    {
        List<LabTestRequest> GetAssignedTests(string empId, string? doctorFilter);
        void MarkTestCompleted(int requestId);
        void SaveTestResult(TestResults result);
        List<TestResults> GetAllResults(string empId);
        List<TestResults> GetPatientHistory(string patientId);
        List<LabBill> GetBills(string empId);
        

    }
}
