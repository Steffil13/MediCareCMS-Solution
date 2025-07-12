namespace MediCareCMS.Models
{
    public class TestResults
    {
       
            public int ResultId { get; set; }
            public int RequestId { get; set; }
            public string ResultValue { get; set; } = "";
            public string Remarks { get; set; } = "";
        public DateTime RecordedDate { get; set; }
        public string PatientId { get; set; }  // foreign key
        public string PatientName { get; set; } // 👈 Required for Razor view

        public string TestName { get; set; } = "";
        public string ResultStatus
        {
            get
            {
                // Sample logic to detect abnormality
                if (ResultValue.ToLower().Contains("high") || ResultValue.ToLower().Contains("low") || ResultValue.ToLower().Contains("abnormal"))
                    return "Abnormal";
                return "Normal";
            }
        }

    }
}
