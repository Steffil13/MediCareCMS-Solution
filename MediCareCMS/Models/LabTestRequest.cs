using System.ComponentModel.DataAnnotations;

namespace MediCareCMS.Models
{
    public class LabTestRequest
    {
        public int RequestId { get; set; }
        public int PatientId { get; set; } 
        public string DoctorId { get; set; } = "";
        public string LabEmpId { get; set; } = "";
        public int TestId { get; set; }
        public DateTime RequestedDate { get; set; }
        public string Status { get; set; }

        public int? BillId { get; set; }  // Nullable if bill not generated
        public decimal? Amount { get; set; }  // Amount of the bill


        /* projection for views */
        
        public string PatientName { get; set; } = "";
       
        public string DoctorName { get; set; } = "";
        public string TestName { get; set; } = "";
    }
}