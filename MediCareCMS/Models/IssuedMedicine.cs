namespace MediCareCMS.Models
{
    public class IssueMedicine
    {
        public int IssueId { get; set; }
        public int BillId { get; set; }
        public string MedicineName { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal => Quantity * UnitPrice;
    }
}
