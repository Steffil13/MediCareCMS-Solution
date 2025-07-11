using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MediCareCMS.ViewModel
{
    public class AppointmentViewModel
    {
        public int AppointmentId { get; set; }
        [BindNever]
        [ValidateNever]
        public string AppointmentNumber { get; set; }  // APT0001
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        [BindNever]
        [ValidateNever]
        public int Token { get; set; }

        [BindNever]
        [ValidateNever]
        public string PatientName { get; set; }  // Display
        [BindNever]
        [ValidateNever]
        public string DoctorName { get; set; }   // Display
        public bool GenerateBill { get; set; } // default = false

    }

}
