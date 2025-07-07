namespace MediCareCMS.Models
{
    public class TestResult
    {
        public int ResultId { get; set; }
        public int RequestId { get; set; }
        public string PatientValue { get; set; }
        public string ResultStatus { get; set; }

        // Display use
        public string PatientName { get; set; }
        public string TestName { get; set; }
        public string NormalRange { get; set; }
    }
}
