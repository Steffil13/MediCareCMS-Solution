namespace MediCareCMS.Models
{
    public class LabBill
    {
        public int BillId { get; set; }
        public int RequestId { get; set; }
        public string PatientId { get; set; } = "";
        public int TestId { get; set; }
        public decimal Amount { get; set; }
        public DateTime BillDate { get; set; }

        /* projection */
        public string TestName { get; set; } = "";
    }
}
