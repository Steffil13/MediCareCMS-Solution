
namespace MediCareCMS.Models
{
    public class PharmacyBill
    {
        public int BillId { get; set; }
        public int PrescriptionId { get; set; }
        public int PharmacistId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime IssuedDate { get; set; }
    }
}
