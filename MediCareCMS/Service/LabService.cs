using MediCareCMS.Models;
using MediCareCMS.Repositories;

namespace MediCareCMS.Services
{
    public class LabService : ILabService
    {
        private readonly ILabRepository _repo;
        public LabService(ILabRepository repo) => _repo = repo;

        public List<LabTestRequest> GetAssignedTests(string id, string? doc) => _repo.GetAssignedTests(id, doc);
        public void MarkTestCompleted(int reqId) => _repo.MarkTestCompleted(reqId);
        public void RecordResult(TestResults r) => _repo.SaveTestResult(r);
        public List<TestResults> GetAllResults(string id) => _repo.GetAllResults(id);
        public List<TestResults> GetPatientHistory(string pid) => _repo.GetPatientHistory(pid);
        public List<LabBill> GetBills(string id) => _repo.GetBills(id);
    }
}
