using MediCareCMS.Models;
using MediCareCMS.Repositories;

namespace MediCareCMS.Services
{
    public class LabService : ILabService
    {
        private readonly ILabRepository _repo;
        public LabService(ILabRepository repo) => _repo = repo;

        public List<LabTestRequest> GetAssignedTests(string id) => _repo.GetAssignedTests(id);
        public void MarkTestCompleted(int reqId) => _repo.MarkTestCompleted(reqId);
        public void RecordTestResult(TestResults r) => _repo.SaveTestResult(r);
        public List<TestResults> GetPatientHistory(string p) => _repo.GetPatientHistory(p);
        public List<LabBill> GetBills(string id) => _repo.GetBills(id);
    }
}
