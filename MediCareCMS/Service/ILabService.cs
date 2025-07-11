using MediCareCMS.Models;

namespace MediCareCMS.Services
{
    public interface ILabService
    {
        List<LabTestRequest> GetAssignedTests();
        void MarkTestCompleted(int requestId);
        void SaveTestResult(TestResults result);
        List<TestResults> GetAllResults(string empId);
        List<TestResults> GetPatientHistory(string patientId);
        List<LabBill> GetBills(string empId);

        LabBill GetBillByRequestId(int requestId);
        void GenerateLabBill(int requestId);
    }
}
