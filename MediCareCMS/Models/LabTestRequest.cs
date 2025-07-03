namespace MediCareCMS.Models
{
    public class LabTestRequest
    {
        public int RequestId { get; set; }
        public string PatientId { get; set; } = "";
        public string DoctorName { get; set; } = "";   // projection only
        public string PatientName { get; set; } = "";   // projection only
        public string TestName { get; set; } = "";   // projection only
        public int TestId { get; set; }
        public string LabEmpId { get; set; } = "";
        public DateTime RequestedDate { get; set; }
        public string Status { get; set; } = "Pending";
    }
}