namespace MediCareCMS.Models
{
    public class TestResults
    {
        public int ResultId { get; set; }
        public int RequestId { get; set; }
        public string ResultValue { get; set; } = "";
        public string Remarks { get; set; } = "";
        public DateTime RecordedDate { get; set; }
        public string TestName { get; set; }
    }
}
